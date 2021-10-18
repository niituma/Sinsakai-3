using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float walkSpeed;    // 歩く速さ
    public float runSpeed;     // 走る速さ

    private Vector3 moveDirection = Vector3.zero; // 移動する量
    private Vector3 direction;                    // 移動する方向
    private float x;                              // horizontal
    private float z;                              // vertical
    private float gravity = 98f;                  // 下方向への移動量
    private CharacterController controller;       // character controller



    // Use this for initialization
    void Start()
    {
        // character controller 取得
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        // GetAxisを定義
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        if (x != 0 || z != 0)
        {
            direction = new Vector3(x, 0, z);
        } // GetAxisから方向を取得


        if (Input.GetMouseButton(1))
        {  // 右クリックしながら移動するとダッシュ
            moveDirection = direction * runSpeed;
        }
        else
        {
            moveDirection = direction * walkSpeed;
        }


        // 地面についていなかったら下方向に落ちる
        if (controller.isGrounded)
        {
            moveDirection.y = 0f;
        }
        else
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // 移動する
        controller.Move(moveDirection * Time.deltaTime);

        // ここが問題のよう。移動中は回転するのだが、移動し終わると元の向きに戻ってしまう。
        Quaternion q = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 1200.0f * Time.deltaTime);


    }
}
