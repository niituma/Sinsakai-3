using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float _movePower = 3;
    [SerializeField] float _jumpPower = 3;
    [SerializeField] float _gravityPower = 0.3f;
    float h, v;
    float _animationspeed;
    Rigidbody _rb = default;
    Animator _anim = default;
    /// <summary>入力された方向の XZ 平面でのベクトル</summary>
    Vector3 _dir;
    public Vector2 move;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        move = new Vector2(h, v);
        _dir = Vector3.forward * v + Vector3.right * h;
        // カメラのローカル座標系を基準に dir を変換する
        _dir = Camera.main.transform.TransformDirection(_dir);
        // カメラは斜め下に向いているので、Y 軸の値を 0 にして「XZ 平面上のベクトル」にする
        _dir.y = 0;
        // キャラクターを「現在の（XZ 平面上の）進行方向」に向ける
        Vector3 forward = _rb.velocity;
        forward.y = 0;

        if (forward != Vector3.zero)
        {
            this.transform.forward = forward;
        }

        Vector3 velo = new Vector3(_rb.velocity.x, _rb.velocity.y, _rb.velocity.x);
        velo.y = _rb.velocity.y;
        _rb.velocity = velo;

        Jump();


    }
    private void LateUpdate()
    {
        float targetSpeed = _movePower;

        if (move == Vector2.zero)
        {
            targetSpeed = 0.0f;
        }
        _animationspeed = Mathf.Lerp(_animationspeed, targetSpeed, Time.deltaTime * 10f);

        _anim.SetFloat("Speed", _animationspeed);

    }

    void FixedUpdate()
    {
        // 「力を加える」処理は力学的処理なので FixedUpdate で行うこと
        _rb.AddForce(_dir.normalized * _movePower, ForceMode.Impulse);
    }

    void Jump()
    {
        Vector3 velosity = _rb.velocity;
        if (Input.GetButtonDown("Jump"))
            _rb.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
        else if (!Input.GetButtonDown("Jump") && velosity.y > 0)
            velosity.y *= _gravityPower;

        _rb.velocity = velosity;
    }
}
