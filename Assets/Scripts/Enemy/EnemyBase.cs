using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    NavMeshAgent _agent;
    protected EnemyHPBar _myhp = default;
    [SerializeField] float _turnSpeed = 8.0f;
    /// <summary>Player検知範囲の半径</summary>
    [SerializeField] float _sarchsRangeRadius = 1f;
    [SerializeField] Vector3 _sarchRangeCenter = default;
    /// <summary>攻撃範囲の半径</summary>
    [SerializeField] float _attackRangeRadius = 1f;
    [SerializeField] Vector3 _attackRangeCenter = default;
    [SerializeField] public float _movingdis = 1f;
    [SerializeField] public Collider player;
    [SerializeField] float _speed = 1.0f;
    [SerializeField] float _changespeed = 10f;
    public float _animationspeed;
    public float _targetspeed = 0;
    public bool _stopmove = default;
    bool _ishit = default;
    public bool Ishit { get => _ishit; set => _ishit = value; }
    bool _isbighit = default;
    public bool Isbighit { get => _isbighit; set => _isbighit = value; }
    bool _isShoothit = default;
    public bool IsShoothit { get => _isShoothit; set => _isShoothit = value; }


    //ランダムで決める数値の最大値
    [SerializeField] float _radius = 3;
    //設定した待機時間
    [SerializeField] float _waitTime = 2;
    //待機時間を数える
    [SerializeField] float _time = 0;
    [SerializeField] Transform central;

    public enum EnemyAction
    {
        Wandering,
        Standing
    }
    public EnemyAction _actionMode;
    public enum State
    {
        Walk,
        Wait,
        Tracking,
        Hit,
        BHit,
        SHit
    }
    public State _stateMode;


    // Start is called before the first frame update
    public void Start()
    {
        _myhp = GetComponent<EnemyHPBar>();
        _agent = GetComponent<NavMeshAgent>();
        if (_actionMode == EnemyAction.Wandering)
        {
            //目標地点に近づいても速度を落とさなくなる
            _agent.autoBraking = false;
            //目標地点を決める
            GotoNextPoint();
        }
    }

    // Update is called once per frame
    public void Update()
    {
        if (_stateMode == State.Hit)
        {
            if (player)
                _time = 0;
            if (!Ishit)
                Ishit = true;
            _myhp.Damage(10, 15);
            _stateMode = State.Wait;
        }
        else if (_stateMode == State.SHit)
        {
            IsShoothit = true;
            _myhp.Damage(5, 10);
            _stateMode = State.Wait;
        }
        else if (_stateMode == State.BHit)
        {
            _time = 0;
            Isbighit = true;
            StartCoroutine(ChangeModeWait(2));
        }
        else if (Ishit)
        {
            Ishit = false;
        }
        Sarch();
        if (_actionMode == EnemyAction.Wandering)
        {
            //経路探索の準備ができておらず
            //目標地点までの距離が0.5m未満ならNavMeshAgentを止める
            if (_agent.pathStatus != NavMeshPathStatus.PathInvalid && !_agent.pathPending && _agent.remainingDistance < 0.5f || _stateMode != State.Walk)
                StopHere();

            if (_stateMode == State.Tracking && !player && _stateMode != State.Hit)
            {
                GotoNextPoint();
                _time = 0;
            }
        }
        Debug.Log(_stateMode);
    }
    public void GotoNextPoint()
    {
        if (_agent.pathStatus != NavMeshPathStatus.PathInvalid)
        {
            //navMeshAgentの操作
            if (_stateMode != State.Hit)
            {
                if (_stateMode != State.Walk)
                    _stateMode = State.Walk;
                //NavMeshAgentのストップを解除
                _agent.isStopped = false;

                //目標地点のX軸、Z軸をランダムで決める
                float posX = Random.Range(-1 * _radius, _radius);
                float posZ = Random.Range(-1 * _radius, _radius);

                //CentralPointの位置にPosXとPosZを足す
                Vector3 pos = central.position;
                pos.x += posX;
                pos.z += posZ;

                //NavMeshAgentに目標地点を設定する
                _agent.destination = pos;
            }
        }
    }

    void StopHere()
    {
        if (_agent.pathStatus != NavMeshPathStatus.PathInvalid)
        {
            if (_stateMode != State.Wait && !player)
                _stateMode = State.Wait;
            //NavMeshAgentを止める
            _agent.isStopped = true;

            if (_stateMode == State.Wait && !player)
            {
                //待ち時間を数える
                _time += Time.deltaTime;

                //待ち時間が設定された数値を超えると発動
                if (_time > _waitTime)
                {
                    //目標地点を設定し直す
                    GotoNextPoint();
                    _time = 0;
                }
            }
        }
    }
    public void FixedUpdate()
    {
        Move();
    }
    public void OnDrawGizmosSelected()
    {
        // 攻撃範囲を赤い線でシーンビューに表示する
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(PlayerSarchRangeCenter(), _sarchsRangeRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(AttackRangeCenter(), _attackRangeRadius);
    }
    Vector3 PlayerSarchRangeCenter()
    {
        Vector3 center = this.transform.position + this.transform.forward * _sarchRangeCenter.z
            + this.transform.up * _sarchRangeCenter.y
            + this.transform.right * _sarchRangeCenter.x;
        return center;
    }
    void Sarch()
    {
        player = Physics.OverlapSphere(PlayerSarchRangeCenter(), _sarchsRangeRadius).Where(p => p.tag == "Player").FirstOrDefault();
        if (player && !_stopmove)
        {
            var dir = player.transform.position - transform.position;
            dir.y = 0;

            var lookRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * _turnSpeed);
        }
        else
        {
            if (_stateMode != State.Wait && _actionMode == EnemyAction.Standing)
                _stateMode = State.Wait;
        }
    }
    Vector3 AttackRangeCenter()
    {
        Vector3 center = this.transform.position + this.transform.forward * _attackRangeCenter.z
            + this.transform.up * _attackRangeCenter.y
            + this.transform.right * _attackRangeCenter.x;
        return center;
    }
    void Attack()
    {
        if (_stateMode != State.Tracking)
            _stateMode = State.Wait;
        var hit = Physics.OverlapSphere(AttackRangeCenter(), _attackRangeRadius);
        foreach (var c in hit)
        {
            PlayerController target = c.gameObject.GetComponent<PlayerController>();

            if (target)
            {
                target._ishit = true;
            }
        }
    }
    private void Move()
    {
        if (_stateMode == State.Tracking || _stateMode == State.Walk)
        {
            _targetspeed = _speed;
        }
        else if (GetComponent<Rigidbody>().IsSleeping() || _stateMode == State.Wait)
        {
            _targetspeed = 0;
        }


        if (player && !_stopmove)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < _movingdis)
                _targetspeed = 0;
            if (Vector3.Distance(transform.position, player.transform.position) >= _movingdis)
            {
                if (_actionMode == EnemyAction.Standing || _actionMode == EnemyAction.Wandering && _agent.enabled)
                {
                    if (_stateMode != State.Tracking && _stateMode != State.Hit && _stateMode != State.BHit && _stateMode != State.SHit)
                        _stateMode = State.Tracking;

                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, _targetspeed * Time.deltaTime);
                }
            }

        }

        _animationspeed = Mathf.Lerp(_animationspeed, _targetspeed, Time.deltaTime * _changespeed);
    }
    IEnumerator ChangeModeWait(float time)
    {
        yield return new WaitForSeconds(time);
        _stateMode = State.Wait;
    }
    private void ResetTimer()
    {
        _time = 0;
    }
    void StopMoveSwitch()
    {
        _stopmove = !_stopmove;
    }
}
