using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyR : EnemyBase
{
    Animator _anim = default;
    EnemyHPBar _myhp = default;
    [SerializeField] float _attacktimer = 0;
    float _doattacktime = 5f;
    bool _isattack;
    public bool _ishit = default;
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _myhp = GetComponent<EnemyHPBar>();
    }

    // Update is called once per frame
    private new void Update()
    {
        base.Update();
        if (_ishit)
        {
            _ishit = false;
            _myhp.Damage();
        }
        AttackTime();
    }
    private new void FixedUpdate()
    {
        base.FixedUpdate();
    }
    private void LateUpdate()
    {
        _anim.SetBool("Attack", _isattack);
        _anim.SetBool("Hit", _ishit);
        _anim.SetFloat("Speed", _animationspeed);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PMagicBall")
        {
            _anim.SetBool("Hit", true);
        }
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
    void StopMoveSwitch()
    {
        _stopmove = !_stopmove;
    }
}
