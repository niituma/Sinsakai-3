using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{

    protected EnemyHPBar _myhp = default;
    public enum State
    {
        Walk,
        Wait,
        Hit,
        BHit,
        SHit
    }
    public State _stateMode;
    // Start is called before the first frame update
    void Start()
    {
        _myhp = GetComponent<EnemyHPBar>();
    }

    // Update is called once per frame
    void Update()
    {
        BossDamage();
    }
    void BossDamage()
    {
        if (_stateMode == State.SHit)
        {
            _myhp.Damage(5, 10);
            _stateMode = State.Wait;
        }
        else if (_stateMode == State.Hit)
        {
            _myhp.Damage(30, 50);
            _stateMode = State.Wait;
        }
        else if (_stateMode == State.BHit)
        {
            _myhp.Damage(100, 150);
            _stateMode = State.Wait;
        }
    }
}
