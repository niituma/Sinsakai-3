﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float _movePower = 3;
    [SerializeField] float _dashmovePower = 6;
    [SerializeField] float _avdPower = 1.1f;
    [SerializeField] float _jumpPower = 3;
    [SerializeField] float _gravityPower = 0.3f;
    [SerializeField] float _turnSpeed = 8.0f;
    [SerializeField] float _isGroundedLength = 1.1f;
    [SerializeField] float _nextButtonDownTime = 0.3f;
    [SerializeField] GameObject _magiceff = default;
    [SerializeField] GameObject _rightattackmuzzle = default;
    [SerializeField] Vector3 _gettargetsRangeCenter = default;
    [SerializeField] Vector3 _attackRangeCenter = default;
    /// <summary>敵のターゲットロックできる範囲の半径</summary>
    [SerializeField] float _targetsRangeRadius = 1f;
    /// <summary>攻撃範囲の半径</summary>
    [SerializeField] float _attackRangeRadius = 1f;
    [SerializeField] public List<Collider> _currentenemy = new List<Collider>();
    GameObject _crosshair;
    [SerializeField] GameObject _crosshaircanvas;
    float h, v;
    float _nowTime;
    public float _avdTime;
    float _animationspeed;
    [SerializeField] bool _stopmove = default;
    bool _isjump = default;
    bool _push = default;
    bool _avd = default;
    public bool _lookon = default;
    public bool _onavd = default;

    [SerializeField] CinemachineVirtualCamera _mousecamera;
    [SerializeField] CinemachineVirtualCamera _targetcamera;
    [SerializeField] float _changeVerticalAxisValue = 5f;
    CinemachinePOV aim;
    ControllerSystem _input;
    TargetLookOn target;
    Rigidbody _rb = default;
    Animator _anim = default;
    /// <summary>入力された方向の XZ 平面でのベクトル</summary>
    Vector3 _dir;
    public Vector2 _movedir;
    Vector2 _move1dir;
    Vector2 _move2dir;


    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        _input = GetComponent<ControllerSystem>();
        //_mousecamera = GetComponent<CinemachineVirtualCamera>();
        aim = _mousecamera.GetCinemachineComponent<CinemachinePOV>();
        _crosshair = GameObject.Find("CrossHair");
        target = _crosshaircanvas.GetComponent<TargetLookOn>();
    }

    void Update()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        _movedir = new Vector2(h, v);
        _dir = new Vector3(h, 0, v);
        _dir = Camera.main.transform.TransformDirection(_dir);
        // カメラは斜め下に向いているので、Y 軸の値を 0 にして「XZ 平面上のベクトル」にする
        _dir.y = 0;
        // キャラクターを「現在の（XZ 平面上の）進行方向」に向ける
        if (_input.move != Vector2.zero && !_stopmove)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_dir);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, Time.deltaTime * _turnSpeed);
        }

        if (_lookon && _input.move == Vector2.zero && !_stopmove)
        {
            var direction = _crosshair.transform.position - transform.position;
            direction.y = 0;

            var lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * _turnSpeed);
        }

        Vector3 velo = new Vector3(_rb.velocity.x, _rb.velocity.y, _rb.velocity.x);
        velo.y = _rb.velocity.y;
        _rb.velocity = velo;

        _isjump = _input.jump;
        TargetLookOn();
        Jump();
        if (!_stopmove && IsGrounded()) { Avodance(); }
        Targets();
    }
    private void LateUpdate()
    {
        _anim.SetBool("Grounded", IsGrounded());
        _anim.SetBool("Jump", _isjump);
        //　押した方向がリミットの角度を越えていない　かつ　制限時間内に移動キーが押されていれば走る
        if (_avd && _move1dir == _move2dir)
        {
            _anim.SetBool("Avoidance", true);
            _avd = false;
            _onavd = true;
        }
        else
        {
            _anim.SetBool("Avoidance", false);
        }
        AttackMotion();
    }

    void FixedUpdate()
    {
        Move();
    }
    void OnDrawGizmosSelected()
    {
        // 攻撃範囲を赤い線でシーンビューに表示する
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(GetTargetsRangeCenter(), _targetsRangeRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GetAttackRangeCenter(), _attackRangeRadius);
    }
    Vector3 GetAttackRangeCenter()
    {
        Vector3 center = this.transform.position + this.transform.forward * _attackRangeCenter.z
            + this.transform.up * _attackRangeCenter.y
            + this.transform.right * _attackRangeCenter.x;
        return center;
    }
    Vector3 GetTargetsRangeCenter()
    {
        Vector3 center = this.transform.position + this.transform.forward * _gettargetsRangeCenter.z
            + this.transform.up * _gettargetsRangeCenter.y
            + this.transform.right * _gettargetsRangeCenter.x;
        return center;
    }
    void Attack()
    {
        var hit = Physics.OverlapSphere(GetAttackRangeCenter(), _attackRangeRadius);
        foreach (var c in hit)
        {
            EnemyR enemy = c.gameObject.GetComponent<EnemyR>();

            if (enemy)
            {
                enemy._ishit = true;
            }
        }

    }
    void Targets()
    {
        _currentenemy = FilterTargetObject(Physics.OverlapSphere(GetTargetsRangeCenter(), _targetsRangeRadius).ToList());
    }
    protected List<Collider> FilterTargetObject(List<Collider> detection)
    {
        return detection.Where(h => h.tag == "Enemy").Where(h =>
        {
            Vector3 screenPoint = Camera.main.WorldToViewportPoint(h.transform.position);
            return screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > -0.25 && screenPoint.y < 0.9;
        }).ToList();
    }
    void Move()
    {

        float _targetSpeed = _input.sprint ? _dashmovePower : _movePower;

        // 「力を加える」処理は力学的処理なので FixedUpdate で行うこと
        if (!_stopmove)
            _rb.AddForce(_dir.normalized * _targetSpeed, ForceMode.Impulse);

        if (_input.move == Vector2.zero)
        {
            _targetSpeed = 0.0f;
        }
        _animationspeed = Mathf.Lerp(_animationspeed, _targetSpeed, Time.deltaTime * 10f);

        _anim.SetFloat("Speed", _animationspeed);
    }
    void Avodance()
    {
        if (!_push)
        {
            if (Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical"))
            {
                _move1dir = _movedir;
                _push = true;
                _nowTime = 0f;
            }
        }
        else
        {
            //　時間計測
            _nowTime += Time.deltaTime;

            if (_nowTime > _nextButtonDownTime)
            {
                _push = false;
            }
            if (Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical") && _nowTime <= _nextButtonDownTime)
            {
                _move2dir = _movedir;
                _avd = true;
            }
        }

        if (_onavd)
        {
            _avdTime += Time.deltaTime;
            _rb.AddForce(transform.forward * _avdPower, ForceMode.Impulse);
        }

        if (_avdTime >= 0.5f || _input.attack)
        {
            _onavd = false;
            _avdTime = 0;
        }
    }

    void TargetLookOn()
    {
        if (target.isneartarget)
        {
            if (_input.lockon)
            {
                _lookon = !_lookon;
                if (_lookon)
                {
                    target.targeton = false;
                    _targetcamera.Priority = 100;
                }
                else
                {
                    _targetcamera.Priority = 9;
                    target.targeton = true;
                    aim.m_HorizontalAxis.Value = _targetcamera.transform.localEulerAngles.y;
                    aim.m_VerticalAxis.Value = _changeVerticalAxisValue;
                }
                _input.lockon = false;
            }
        }
        else
        {
            _input.lockon = false;
            _lookon = false;
            target.targeton = true;
            _targetcamera.Priority = 9;
        }
        if (_input.change)
        {
            target.ChangeTarget();
            _input.change = false;
        }
        _crosshair.SetActive(_lookon);
    }
    void Jump()
    {
        Vector3 velosity = _rb.velocity;
        if (IsGrounded())
        {
            if (_input.jump)
            {
                _rb.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
            }
            else if (!_input.jump && velosity.y > 0)
                velosity.y *= _gravityPower;
        }

        _rb.velocity = velosity;
        _input.jump = false;
    }
    void AttackMotion()
    {
        _anim.SetBool("Punch", _input.attack);
        _anim.SetBool("Magic", _input.fire);
        _input.attack = false;
        _input.fire = false;
    }
    void Magic()
    {
        Instantiate(_magiceff, _rightattackmuzzle.transform.position, this.transform.rotation);
    }
    bool IsGrounded()
    {
        // Physics.Linecast() を使って足元から線を張り、そこに何かが衝突していたら true とする
        CapsuleCollider col = GetComponent<CapsuleCollider>();
        Vector3 start = this.transform.position + col.center;   // start: 体の中心
        Vector3 end = start + Vector3.down * _isGroundedLength;  // end: start から真下の地点
        Debug.DrawLine(start, end); // 動作確認用に Scene ウィンドウ上で線を表示する
        bool isGrounded = Physics.Linecast(start, end); // 引いたラインに何かがぶつかっていたら true とする
        return isGrounded;
    }
    void StopMoveSwitch()
    {
        _stopmove = !_stopmove;
    }
}

