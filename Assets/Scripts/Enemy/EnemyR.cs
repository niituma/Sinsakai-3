using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyR : EnemyBase
{
    Animator _anim = default;
    [SerializeField] float _attacktimer = 0;
    [SerializeField] float _doattacktime = 5f;
    [SerializeField] float _isGroundedLength = 1.1f;
    bool _isattack;

    /// <summary>どれくらい見るか</summary>
    [SerializeField, Range(0f, 1f)] float _weight = 0;
    /// <summary>身体をどれくらい向けるか</summary>
    [SerializeField, Range(0f, 1f)] float _bodyWeight = 0;
    /// <summary>頭をどれくらい向けるか</summary>
    [SerializeField, Range(0f, 1f)] float _headWeight = 0;
    /// <summary>目をどれくらい向けるか</summary>
    [SerializeField, Range(0f, 1f)] float _eyesWeight = 0;
    /// <summary>関節の動きをどれくらい制限するか</summary>
    [SerializeField, Range(0f, 1f)] float _clampWeight = 0;
    // Start is called before the first frame update
    private new void Start()
    {
        _anim = GetComponent<Animator>();
        base.Start();
    }

    // Update is called once per frame
    private new void Update()
    {
        base.Update();

        AttackTime();
    }
    private new void FixedUpdate()
    {
        base.FixedUpdate();
    }
    private void LateUpdate()
    {
        _anim.SetBool("Attack", _isattack);
        _anim.SetBool("Ground", IsGrounded());
        _anim.SetFloat("Speed", _animationspeed);
        _anim.SetBool("Hit", Ishit);
        _anim.SetBool("Big Hit", Isbighit);
        _anim.SetBool("Shoot Hit", IsShoothit);
        if (Isbighit)
            Isbighit = false;
        if (Ishit)
            Ishit = false;
        if (IsShoothit)
            IsShoothit = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PMagic")
        {
            mode = Action.Hit;
            _anim.SetBool("Hit", true);
        }
        else if (other.tag == "PBigMagic")
        {
            mode = Action.BHit;
            _myhp.Damage(40, 50);
            _anim.SetBool("Big Hit", true);
        }
    }
    void OnAnimatorIK(int layerIndex)
    {
        // LookAt の重みとターゲットを指定する
        if (player && !_stopmove)
        {
            _anim.SetLookAtWeight(_weight, _bodyWeight, _headWeight, _eyesWeight, _clampWeight);
            _anim.SetLookAtPosition(player.transform.position + new Vector3(0, 1, 0));
        }
    }
    bool IsGrounded()
    {
        int layerNo = LayerMask.NameToLayer("Ground");
        // マスクへの変換（ビットシフト）
        int layerMask = 1 << layerNo;
        // Physics.Linecast() を使って足元から線を張り、そこに何かが衝突していたら true とする
        CapsuleCollider col = GetComponent<CapsuleCollider>();
        Vector3 start = this.transform.position + col.center;   // start: 体の中心
        Vector3 end = start + Vector3.down * _isGroundedLength;  // end: start から真下の地点
        Debug.DrawLine(start, end); // 動作確認用に Scene ウィンドウ上で線を表示する
        bool isGrounded = Physics.Linecast(start, end, layerMask); // 引いたラインに何かがぶつかっていたら true とする
        return isGrounded;
    }
    void AttackTime()
    {
        if (player)
        {
            _attacktimer += Time.deltaTime;

            if (_attacktimer > _doattacktime && Vector3.Distance(transform.position, player.transform.position) < _movingdis)
            {
                _isattack = true;
                _attacktimer = 0;
            }
            else
            {
                _isattack = false;
            }
        }
    }
    void NavimashOn()
    {
        gameObject.GetComponent<Rigidbody>().useGravity = false;
        gameObject.GetComponent<NavMeshAgent>().enabled = true;
    }
    void StopMoveSwitch(int movenum)
    {
        switch (movenum)
        {
            case 1:
                _stopmove = true;
                break;
            case 2:
                _stopmove = false;
                break;
            default:
                Debug.LogWarning("movenumが指定の範囲外です。Animationのイベントから指定してください。");
                break;
        }
    }
}
