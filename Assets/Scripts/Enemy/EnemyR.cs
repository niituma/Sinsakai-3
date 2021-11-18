using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyR : MonoBehaviour
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
    void Update()
    {
        if (_ishit)
        {
            _ishit = false;
            _myhp.Damage();
        }

    }
    private void LateUpdate()
    {
        _anim.SetBool("Hit", _ishit);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PMagicBall")
        {
            _anim.SetBool("Hit", true);
        }
    }
}
