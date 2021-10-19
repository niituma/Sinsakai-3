using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermove2 : MonoBehaviour
{
    [SerializeField] Vector2 move;
    [SerializeField] float _turnSpeed = 8.0f;
    public float _movePower = 6.0f;
    public float _jumpPower = 8.0f;
    public float _gravity = 20.0f;
    float _animationspeed;
    float h, v;
    bool isfly = default;

    private Vector3 _movedir = Vector3.zero;
    Animator _anim = default;
    CharacterController _controller;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        if (_controller.isGrounded)
        {
            move = new Vector2(h, v);
            _movedir = new Vector3(h, 0.0f, v);
            _movedir = Camera.main.transform.TransformDirection(_movedir);
            // カメラは斜め下に向いているので、Y 軸の値を 0 にして「XZ 平面上のベクトル」にする
            _movedir.y = 0;
            _movedir = _movedir * _movePower;
            if (Input.GetButton("Jump"))
            {
                _movedir.y = _jumpPower;
            }
            if (move != Vector2.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(_movedir);
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, Time.deltaTime * _turnSpeed);
            }
        }

        _movedir.y -= _gravity * Time.deltaTime;
        _controller.Move(_movedir * Time.deltaTime);
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
}
