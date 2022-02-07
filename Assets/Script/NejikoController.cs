using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NejikoController : MonoBehaviour
{
    CharacterController controller;
    Animator animator;

    Vector3 moveDirection = Vector3.zero;

    public float gravity;
    public float speedZ;
    public float speedJump;

    private void Start()
    {
        //必要なコンポーネントを自動取得
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        //接地判定
        if (controller.isGrounded)
        {
            //前進ベロシティの設定
            if (Input.GetAxis("Vertical") > 0.0f)
            {
                moveDirection.z = Input.GetAxis("Vertical") * speedZ;
            }
            else
            {
                moveDirection.z = 0;
            }

            //方向の回転
            transform.Rotate(0, Input.GetAxis("Horizontal") * 3, 0);

            //ジャンプ処理
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = speedJump;
                animator.SetTrigger("jump");
            }
        }

        //重力分の力を毎フレーム追加
        moveDirection.y -= gravity * Time.deltaTime;

        //移動実行
        Vector3 globalDirection = transform.TransformDirection(moveDirection);
        controller.Move(globalDirection * Time.deltaTime);

        //移動後設置してたらY方向の速度はリセット
        if (controller.isGrounded) moveDirection.y = 0;

        //速度が０以上なら走っているフラグをtrueにする
        //IdleとRunアニメーションの制御
        animator.SetBool("run", moveDirection.z > 0.0f);
    }
}
