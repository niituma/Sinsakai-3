using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyR : EnemyBase
{
    Animator _anim = default;
    EnemyHPBar _myhp = default;
    [SerializeField] float _attacktimer = 0;
    [SerializeField] float _doattacktime = 5f;
    bool _isattack;
    public bool _ishit = default;

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
    void OnAnimatorIK(int layerIndex)
    {
        // LookAt の重みとターゲットを指定する
        if (player && !_stopmove)
        {
            _anim.SetLookAtWeight(_weight, _bodyWeight, _headWeight, _eyesWeight, _clampWeight);
            _anim.SetLookAtPosition(player.transform.position + new Vector3(0, 1, 0));
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
