using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float _movePower = 3;
    [SerializeField] float _dashmovePower = 6;
    [SerializeField] float _jumpPower = 3;
    [SerializeField] float _gravityPower = 0.3f;
    [SerializeField] float _turnSpeed = 8.0f;
    [SerializeField] float m_isGroundedLength = 1.1f;
    float h, v;
    float _animationspeed;
    bool isjump = default;

    ControllerSystem _input;
    Rigidbody _rb = default;
    Animator _anim = default;
    /// <summary>入力された方向の XZ 平面でのベクトル</summary>
    Vector3 _dir;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        _input = GetComponent<ControllerSystem>();
    }

    void Update()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        _dir = new Vector3(h ,0, v);
        _dir = Camera.main.transform.TransformDirection(_dir);
        // カメラは斜め下に向いているので、Y 軸の値を 0 にして「XZ 平面上のベクトル」にする
        _dir.y = 0;
        // キャラクターを「現在の（XZ 平面上の）進行方向」に向ける
        if (_input.move != Vector2.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_dir);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, Time.deltaTime * _turnSpeed);
        }

        Vector3 velo = new Vector3(_rb.velocity.x, _rb.velocity.y, _rb.velocity.x);
        velo.y = _rb.velocity.y;
        _rb.velocity = velo;

        isjump = _input.jump;
        Jump();


    }
    private void LateUpdate()
    {
        _anim.SetBool("Grounded", IsGrounded());
        _anim.SetBool("Jump", isjump);
        //Attack();
    }

    void FixedUpdate()
    {
        Move();
    }
    void Move()
    {
        float _targetSpeed = _input.sprint ? _dashmovePower : _movePower;

        // 「力を加える」処理は力学的処理なので FixedUpdate で行うこと
        _rb.AddForce(_dir.normalized * _targetSpeed, ForceMode.Impulse);

        if (_input.move == Vector2.zero)
        {
            _targetSpeed = 0.0f;
        }
        _animationspeed = Mathf.Lerp(_animationspeed, _targetSpeed, Time.deltaTime * 10f);

        _anim.SetFloat("Speed", _animationspeed);
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
    void Attack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            _anim.SetBool("Punch", true);
        }
        else
        {
            _anim.SetBool("Punch", false);
        }
    }
    bool IsGrounded()
    {
        // Physics.Linecast() を使って足元から線を張り、そこに何かが衝突していたら true とする
        CapsuleCollider col = GetComponent<CapsuleCollider>();
        Vector3 start = this.transform.position + col.center;   // start: 体の中心
        Vector3 end = start + Vector3.down * m_isGroundedLength;  // end: start から真下の地点
        Debug.DrawLine(start, end); // 動作確認用に Scene ウィンドウ上で線を表示する
        bool isGrounded = Physics.Linecast(start, end); // 引いたラインに何かがぶつかっていたら true とする
        return isGrounded;
    }
}

