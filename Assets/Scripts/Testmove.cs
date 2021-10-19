using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testmove : MonoBehaviour
{
    public float speed = 6.0F;          //歩行速度
    public float jumpSpeed = 8.0F;      //ジャンプ力
    public float gravity = 20.0F;       //重力の大きさ
    public float rotateSpeed = 3.0F;    //回転速度
    public float camRotSpeed = 5.0f;    //視点の上下スピード

    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    private float h, v;
    private float mX, mY;
    private float lookUpAngle;
    private float flyingTime = 0f;
    private bool isFlying;

    // Use this for initialization
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }//Start()

    // Update is called once per frame
    void Update()
    {

        h = Input.GetAxis("Horizontal");    //左右の矢印キーの値を取得(-1.0~1.0)
        v = Input.GetAxis("Vertical");      //上下の矢印キーの値を取得(-1.0~1.0)
        mX = Input.GetAxis("Mouse X");      //マウスの左右移動量(-1.0~1.0)
        mY = Input.GetAxis("Mouse Y");      //マウスの上下移動量(-1.0~1.0)

        //カメラのみ上下に回転させる，180-140=60より上下60度まで見ることができる
        lookUpAngle = Camera.main.transform.eulerAngles.x - 180 + camRotSpeed * mY;
        if (Mathf.Abs(lookUpAngle) > 140)
            Camera.main.transform.Rotate(new Vector3(camRotSpeed * mY, 0, 0));

        if (controller.isGrounded || isFlying)
        {
            moveDirection = new Vector3(h, 0, v);
            moveDirection = transform.TransformDirection(moveDirection);
            gameObject.transform.Rotate(new Vector3(0, rotateSpeed * mX, 0));
        }

        if (controller.isGrounded)
        {
            isFlying = false;
            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jumpSpeed;
                flyingTime = 0f;
            }
        }
        else
        {
            flyingTime += Time.deltaTime;
            if (Input.GetButtonDown("Jump"))
            {
                if (flyingTime < 0.35f)
                    isFlying = !isFlying;
                else
                    flyingTime = 0f;
            }
        }

        if (isFlying)
            moveDirection.y = Input.GetButton("Jump") ? 0.8f * jumpSpeed : 0f;
        else
            moveDirection.y -= gravity * Time.deltaTime;    //重力の効果
        controller.Move(moveDirection * Time.deltaTime);    //キャラクターを移動させる

    }//Update()
}
