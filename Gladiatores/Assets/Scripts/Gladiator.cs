﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Gladiator : MonoBehaviour {

    public enum Weapons
    {
        Punch,
        Sword,
        Bow,
        Shield,

        ALL
    }

    [SerializeField]
    private float walkSpeed = 1F;
    [SerializeField]
    private float jumpPower = 1F;
    [SerializeField]
    private Weapons haveWeapon;

    private Rigidbody2D rb2d;
    private Vector3 velocity;
    private bool isGrounded;
    private bool attackedReady;
    private Transform shoulder;
    private Transform arm;
    private Transform[] weapons;

	// Use this for initialization
	void Start () {
        // 2Dの剛体を取得する
        rb2d = GetComponent<Rigidbody2D>();

        // 肩と腕を子要素から取得する
        shoulder = transform.GetChild(0).GetChild(0);
        arm = shoulder.GetChild(0);

        // 腕の子要素の武器を取得する
        weapons = new Transform[(int)Weapons.ALL];
        for (int i =0; i< (int)Weapons.ALL; i++)
        {
            weapons[i] = arm.GetChild(i);
        }
	}
	
	// Update is called once per frame
	void Update () {
        // 武器は一択になるようにフラグ管理
        for (int i = 0; i < (int)Weapons.ALL; i++)
        {
            weapons[i].gameObject.SetActive(false);
            weapons[(int)haveWeapon].gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    /// <param name="inputValue"></param>
    public void Walk(float inputValue) {
        // 剛体の速度を入力値で変更
        rb2d.velocity = new Vector2(inputValue * walkSpeed, rb2d.velocity.y);
    }

    /// <summary>
    /// 跳躍処理
    /// </summary>
    /// <param name="InputTrigger"></param>
    public void Jump(bool InputTrigger) {
        // 接地中に入力されたら上方向に力を加える
        if (InputTrigger && isGrounded)
        {
            isGrounded = false;
            rb2d.AddForce(transform.up * jumpPower * 200F);
        }
    }

    /// <summary>
    /// 攻撃処理
    /// </summary>
    /// <param name="InputValue"></param>
    public void Attack(float InputValue) {
        var weapon = arm.GetChild((int)haveWeapon);

        // 持っている武器に応じてモーションが異なる
        switch (haveWeapon)
        {
            case Weapons.Punch:
                weapon.GetComponent<Punch>().Attack(InputValue);
                break;
            case Weapons.Sword:
                weapon.GetComponent<Sword>().Attack(InputValue);
                break;
            case Weapons.Bow:
                weapon.GetComponent<Bow>().Attack(InputValue);
                break;
            case Weapons.Shield:
                weapon.GetComponent<Shield>().Attack(InputValue);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 肩の回転
    /// </summary>
    /// <param name="InputAxis"></param>
    public void RotaShoulder(Vector2 InputAxis) {
        // 入力軸の横方向で向きを決定
        if(InputAxis.x <= -0.1F)
        {
            transform.localScale = new Vector3(-1F, transform.localScale.y, transform.localScale.z);
        }
        else if(InputAxis.x >= 0.1f)
        {
            transform.localScale = new Vector3(+1F, transform.localScale.y, transform.localScale.z);
        }

        // 入力軸の縦方向で角度を修正
        shoulder.localEulerAngles = (transform.forward * InputAxis.y * 90F) + transform.forward * 90F;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.transform.tag == "Ground")
        {
            isGrounded = false;
        }
    }
}
