using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermove2 : MonoBehaviour
{
    [SerializeField] Vector2 move;
    [SerializeField] float _movePower = 6.0f;
    [SerializeField] float _jumpPower = 8.0f;
    [SerializeField] float _turnSpeed = 8.0f;
    [SerializeField] float _gravityPower = 20.0f;
    float h, v;
    float _animationspeed;

    private Vector3 _dir = Vector3.zero;
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

        move = new Vector2(h, v);
        _dir = new Vector3(h, 0, v);
        // カメラのローカル座標系を基準に dir を変換する
        _dir = Camera.main.transform.TransformDirection(_dir);
        // カメラは斜め下に向いているので、Y 軸の値を 0 にして「XZ 平面上のベクトル」にする
        _dir.y = 0;
        _dir = _dir.normalized * _movePower;

        if (Input.GetButton("Jump"))
        {
            _dir.y = _jumpPower;
        }
        Quaternion targetRotation = Quaternion.LookRotation(_dir);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, Time.deltaTime * _turnSpeed);

        _dir.y = _dir.y - (_gravityPower * Time.deltaTime * _movePower);
        _controller.Move(_dir * Time.deltaTime);

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
