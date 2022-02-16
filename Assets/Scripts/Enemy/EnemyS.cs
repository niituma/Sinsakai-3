using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyS : EnemyBase
{
    Animator _anim = default;
    [SerializeField] float _attacktimer = 0;
    [SerializeField] float _doattacktime = 5f;
    [SerializeField] float _isGroundedLength = 1.1f;
    [SerializeField] LayerMask _layerMask = default;
    GameObject _player;
    [SerializeField] AudioClip _attackSound = default;
    AudioSource _audio;
    bool _turn = default;
    //向くスピード(秒速)
    float _speedturn = 8.0f;
    bool _isattack;
    // Start is called before the first frame update
    private new void Start()
    {
        _audio = GetComponent<AudioSource>();
        _anim = GetComponent<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player");
        base.Start();
    }

    // Update is called once per frame
    private new void Update()
    {
        base.Update();
        AttackTime();
        if (Ishit || Isbighit || IsShoothit)
        {
            StartCoroutine(LookEnemy());
        }
        if (_turn && _player)
        {
            var dir = _player.transform.position - transform.position;
            dir.y = 0;

            var lookRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * _speedturn);
            StartCoroutine(DelayMethod(0.3f, () =>
            {
                if (!player)
                {
                    _anim.SetBool("Sarch", _turn);
                }
                else
                {
                    _anim.SetBool("Sarch", false);
                }
            }));
        }
    }
    private void LateUpdate()
    {
        _anim.SetFloat("Speed", _animationspeed);
        _anim.SetBool("Attack", _isattack);
        _anim.SetBool("Ground", IsGrounded());
        _anim.SetBool("Hit", Ishit);
        if (Ishit)
            Ishit = false;
        _anim.SetBool("Big Hit", Isbighit);
        if (Isbighit)
            Isbighit = false;
        _anim.SetBool("Shoot Hit", IsShoothit);
        if (IsShoothit)
            IsShoothit = false;

    }
    private new void FixedUpdate()
    {
        base.FixedUpdate();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PMagic")
        {
            _stateMode = State.Hit;
        }
        else if (other.tag == "PBigMagic")
        {
            _stateMode = State.BHit;
            _myhp.Damage(40, 50);
            _anim.SetBool("Big Hit", true);
        }
    }
    IEnumerator LookEnemy()
    {
        yield return new WaitForSeconds(0.5f);
        _turn = true;
        yield return new WaitForSeconds(2f);
        _anim.SetBool("Sarch", false);
        _turn = false;
    }
    IEnumerator DelayMethod(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action?.Invoke();
    }
    bool IsGrounded()
    {
        
        // Physics.Linecast() を使って足元から線を張り、そこに何かが衝突していたら true とする
        SphereCollider col = GetComponent<SphereCollider>();
        Vector3 start = this.transform.position + col.center;   // start: 体の中心
        Vector3 end = start + Vector3.down * _isGroundedLength;  // end: start から真下の地点
        Debug.DrawLine(start, end); // 動作確認用に Scene ウィンドウ上で線を表示する
        bool isGrounded = Physics.Linecast(start, end, _layerMask); // 引いたラインに何かがぶつかっていたら true とする
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
    void AttackSound()
    {
        _audio.PlayOneShot(_attackSound);
    }
}
