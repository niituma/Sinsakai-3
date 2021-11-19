using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] float _turnSpeed = 8.0f;
    /// <summary>Player検知範囲の半径</summary>
    [SerializeField] float _sarchsRangeRadius = 1f;
    [SerializeField] Vector3 _sarchRangeCenter = default;
    /// <summary>攻撃範囲の半径</summary>
    [SerializeField] float _attackRangeRadius = 1f;
    [SerializeField] Vector3 _attackRangeCenter = default;
    [SerializeField] public float _movingdis = 1f;
    [SerializeField] public Collider player;
    GameObject _player = default;
    [SerializeField] float _speed = 1.0f;
    public float _animationspeed;
    public float _targetspeed = 0;
    public bool _stopmove = default;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    public void Update()
    {
        Sarch();
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
        var hit = Physics.OverlapSphere(AttackRangeCenter(), _attackRangeRadius);
        foreach (var c in hit)
        {
            PlayerMove target = c.gameObject.GetComponent<PlayerMove>();

            if (target)
            {
                target._ishit = true;
            }
        }

    }
    private void Move()
    {
        if (GetComponent<Rigidbody>().IsSleeping())
        {
            _targetspeed = 0;
        }
        else
        {
            _targetspeed = _speed;
        }

        if (player && !_stopmove)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < _movingdis)
                _targetspeed = 0;

            if (Vector3.Distance(transform.position, player.transform.position) >= _movingdis)
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, _targetspeed * Time.deltaTime);
        }

        _animationspeed = Mathf.Lerp(_animationspeed, _targetspeed, Time.deltaTime * 10f);
    }
    void StopMoveSwitch()
    {
        _stopmove = !_stopmove;
    }
}
