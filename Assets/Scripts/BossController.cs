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
        if(_stateMode == State.SHit)
        {
            _myhp.Damage(2,5);
            _stateMode = State.Wait;
        }
    }
}
