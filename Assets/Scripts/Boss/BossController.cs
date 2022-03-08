using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

public class BossController : MonoBehaviour
{
    GameObject _player;
    float _playerDis;
    [SerializeField] float _speed = 2f;
    [SerializeField] float _shortDis = 3f;
    [SerializeField] float _turnSpeed = 8f;
    [SerializeField] float _changespeed = 10f;
    bool _waitAttackTimer = false;
    float _attackTimer = 0;
    [SerializeField] float _attacklimitTime = 5f;
    bool _isFly = false;
    [SerializeField] float _flyingTime = 20f;
    float _flyTimer = 0;
    [SerializeField] GameObject _fireBall = default;
    [SerializeField] GameObject _flameFire = default;
    [SerializeField] GameObject _mouse = default;
    AudioSource _audio;
    [SerializeField] AudioClip _wingSound = default;
    [SerializeField] AudioClip _landSound = default;
    [SerializeField] AudioClip _screamSound = default;
    [SerializeField] AudioClip _TailSound = default;
    [SerializeField] AudioClip _downSound = default;
    float _downHP = 0;
    float _animationspeed;
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
        _audio = GetComponent<AudioSource>();
        _anim = GetComponent<Animator>();
        _myhp = GetComponent<EnemyHPBar>();
        _agent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _downHP = _myhp.MaxHp - 400;
    }

    // Update is called once per frame
    void Update()
    {
        if (_player)
        {
            _playerDis = Vector3.Distance(_player.transform.position, transform.position);
        }


        if (!_isFly)
        {
            _flyTimer += Time.deltaTime;
            if (_flyTimer >= _flyingTime)
            {
                _isFly = true;
                _attackTimer = 2;
                _flyTimer = 0;
            }
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

        if (!_waitAttackTimer)
        {
            _attackTimer += Time.deltaTime;
            if (_attackTimer >= _attacklimitTime)
            {
                _waitAttackTimer = true;
                _attackTimer = 0;
                Attack();
            }
        }

        //それぞれの攻撃された時のダメージ量
        BossDamage();
        //一定以上のHPになったらHITアニメーションをする
        if (!_isFly && _downHP >= _myhp.CurrentHp)
        {
            _isDown = true;
            _downHP = _myhp.CurrentHp - 400;
            _anim.CrossFade("Get Hit", 0.2f);
            _audio.PlayOneShot(_downSound);
            StartCoroutine(DelayMethod(1f, () => _isDown = false));
        }

        if (!_isDown)
        {
            Move();
        }
    }
    private void LateUpdate()
    {
        _anim.SetBool("IsFly", _isFly);
        _anim.SetFloat("Speed", _animationspeed);
    }
    void Move()
    {
        if (!_waitAttackTimer)
        {
            var dir = _player.transform.position - transform.position;
            dir.y = 0;

            var lookRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * _turnSpeed);
        }

        if (_playerDis >= _shortDis && !_isFly && !_waitAttackTimer)
        {
            _agent.destination = _player.transform.position;
            _agent.speed = _speed;
        }
        else
        {
            _agent.destination = transform.position;
            _agent.speed = 0;
        }

        _animationspeed = Mathf.Lerp(_animationspeed, _agent.speed, Time.deltaTime * _changespeed);
    }
    void Attack()
    {
        if (_isFly)
        {
            _anim.CrossFade("Fly Fireball Shoot", 0.2f);
            StartCoroutine(DelayMethod(1f, () => _waitAttackTimer = false));
            return;
        }
        else
        {
            if (!_isDown)
            {
                int number = UnityEngine.Random.Range(0, 10);
                if (number < 2)
                {
                    _anim.CrossFade("Scream", 0.2f);
                    _flameFire.SetActive(true);
                    StartCoroutine(DelayMethod(4f, () => _waitAttackTimer = false));
                    return;
                }

                if (_playerDis <= _shortDis)
                {
                    _anim.CrossFade("Tail Attack", 0.2f);
                }
                else if (_playerDis > _shortDis)
                {
                    _anim.CrossFade("Fireball Shoot", 0.2f);
                }
            }
            StartCoroutine(DelayMethod(1.5f, () => _waitAttackTimer = false));
        }

    }
    void FireBallAttack()
    {
        var obj = Instantiate(_fireBall, _mouse.transform.position, this.transform.rotation);
        obj.transform.parent = this.transform.root;
    }
    void Wing()
    {
        _audio.PlayOneShot(_wingSound);
    }
    void Scream()
    {
        _audio.PlayOneShot(_screamSound);
    }
    void Land()
    {
        _audio.PlayOneShot(_landSound);
    }
    void Tail()
    {
        _audio.PlayOneShot(_TailSound);
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
            _myhp.Damage(20, 30);
            _stateMode = State.Normal;
        }
        else if (_stateMode == State.BHit)
        {
            _myhp.Damage(100, 120);
            _stateMode = State.Normal;
        }
    }
    IEnumerator DelayMethod(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action?.Invoke();
    }
}
