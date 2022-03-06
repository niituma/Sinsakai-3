using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

public class BossController : MonoBehaviour
{
    GameObject _player;
    float _playerDis;
    [SerializeField] float _targetspeed = 2f;
    [SerializeField] float _shortDis = 3f;
    [SerializeField] float _longDis = 10f;
    [SerializeField] float _turnSpeed = 8f;
    bool _waitAttackTimer = false;
    float _attackTimer = 0;
    [SerializeField] float _attacklimitTime = 5f;
    [SerializeField] bool _isFly = false;
    [SerializeField] float _flyingTime = 20f;
    bool _isScream = false;
    [SerializeField] float _screamTime = 5f;
    float _downHP = 0;
    bool _island = default;
    bool _isDown;
    public enum State
    {
        Normal,
        Hit,
        BHit,
        SHit
    }
    public State _stateMode;
    protected EnemyHPBar _myhp = default;
    Animator _anim;
    NavMeshAgent _agent;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _myhp = GetComponent<EnemyHPBar>();
        _agent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _downHP = _myhp.MaxHp - 400;
    }

    // Update is called once per frame
    void Update()
    {
        _playerDis = Vector3.Distance(_player.transform.position, transform.position);

        if (!_waitAttackTimer)
        {
            _attackTimer += Time.deltaTime;
            if (_attackTimer >= _attacklimitTime)
            {
                Attack();
                _waitAttackTimer = true;
                _attackTimer = 0;
            }
        }
        if (!_isDown)
        {
            Move();
        }

        //それぞれの攻撃された時のダメージ量
        BossDamage();
        //一定以上のHPになったらHITアニメーションをする
        if (!_isFly && _downHP >= _myhp.CurrentHp)
        {
            _isDown = true;
            _downHP = _myhp.CurrentHp - 400;
            _anim.CrossFade("Get Hit", 0.2f);
            StartCoroutine(DelayMethod(3f, () => _isDown = false));
        }
        //一定時間になったら降りるようにする
        if (_isFly && !_island)
        {
            _island = true;
            StartCoroutine(DelayMethod(_flyingTime, () =>
            {
                _isFly = false;
                _island = false;
                _attackTimer = 0;
            }));
        }

        if (_isScream)
        {
            StartCoroutine(DelayMethod(_screamTime, () => _isScream = false));
        }
    }
    private void LateUpdate()
    {
        _anim.SetBool("IsFly", _isFly);
    }
    void Move()
    {
        var dir = _player.transform.position - transform.position;
        dir.y = 0;

        var lookRotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * _turnSpeed);

        if (Vector3.Distance(transform.position, _player.transform.position) >= _shortDis)
        {
            _agent.destination = _player.transform.position; ;
        }
    }
    void Attack()
    {
        if (_isFly)
        {
            _anim.CrossFade("Fly Fireball Shoot", 0.2f);
        }
        else
        {
            if (!_isDown)
            {
                if (_playerDis <= _shortDis && !_isScream)
                {
                    _anim.CrossFade("Basic Attack", 0.2f);
                }
                else if (_playerDis >= _longDis && !_isScream)
                {
                    _anim.CrossFade("Fireball Shoot", 0.2f);
                }
                else
                {
                    _anim.CrossFade("Scream", 0.2f);
                }
            }
        }

        StartCoroutine(DelayMethod(5f, () => _waitAttackTimer = false));
    }
    void BossDamage()
    {
        if (_stateMode == State.SHit)
        {
            _myhp.Damage(5, 10);
            _stateMode = State.Normal;
        }
        else if (_stateMode == State.Hit)
        {
            _myhp.Damage(30, 50);
            _stateMode = State.Normal;
        }
        else if (_stateMode == State.BHit)
        {
            _myhp.Damage(100, 150);
            _stateMode = State.Normal;
        }
    }
    IEnumerator DelayMethod(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action?.Invoke();
    }
}
