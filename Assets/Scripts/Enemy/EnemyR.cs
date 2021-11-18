using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyR : EnemyBase
{
    Animator _anim = default;
    EnemyHPBar _myhp = default;
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

    }
    private new void FixedUpdate()
    {
        base.FixedUpdate();
    }
    private void LateUpdate()
    {
        _anim.SetBool("Hit", _ishit);
        _anim.SetFloat("Speed", _targetspeed);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PMagicBall")
        {
            _anim.SetBool("Hit", true);
        }
    }
}
