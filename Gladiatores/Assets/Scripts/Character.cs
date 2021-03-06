﻿using System.Collections.Generic;
using UnityEngine;

enum LogNum
{
    Attack,
    TakeDamage,

    Max,
}

public class Character : MonoBehaviour
{
    protected Weapon equipmentWeapon_;                  //  !<  装備している武器
    protected Rigidbody2D rigid2d_;                     //  !<  剛体
    protected Vector2 pos_;                             //  !<  座標
    protected Vector2 spd_;                             //  !<  移動速度
    protected Vector2 direction_;                       //  !<  画像の向き
    protected float power_;                             //  !<  攻撃力
    protected int life_;                                //  !<  耐久値
    protected bool isLiving_ = true;                    //  !<  生死判定フラグ
    protected bool isAttacking_ = false;                //  !<  攻撃中フラグ
    protected bool isHitting_ = false;                  //  !<  被ダメージフラグ
    protected bool isJumping_ = false;                  //  !<  ジャンプ中フラグ
    protected object[] logRegistKey_ = new object[(int)WeaponType.Max];

    const float AttackFinishFrame_ = 60;                //  !<  攻撃終了時間
    float currentAttackFrame_ = AttackFinishFrame_;     //  !<  現在の攻撃時間

    float currentInvisibleTime_ = 0f;                   //  !<  被ダメージ時間
    float degree_;                                      //  !<  角度

    Weapon[] weaponGroupType_ = new Weapon[(int)WeaponType.Max];      //  !<  所持している武器の種類の一覧

    public Vector2 Spd
    {
        get { return spd_; }
    }
    public bool IsJumping
    {
        get { return isJumping_; }
        set { isJumping_ = value; }
    }
    public Weapon EquipmentWeapon
    {
        get { return equipmentWeapon_; }
    }
    public float Power
    {
        get { return power_; }
    }
    public bool IsAttacking
    {
        get { return isAttacking_; }
        set { isAttacking_ = value; }
    }
    public bool IsLiving
    {
        get { return isLiving_; }
        set { isLiving_ = value; }
    }
    public int Life
    {
        get { return life_; }
        set { life_ = value; }
    }
    public Rigidbody2D RigitBody2D
    {
        get { return rigid2d_; }
    }
    public Vector2 Direction
    {
        get { return direction_; }
    }
    protected void Start()
    {
        rigid2d_ = GetComponent<Rigidbody2D>();
        direction_ = transform.localScale;
        degree_ = 0f;

        Transform arm = transform.GetChild(0).transform.GetChild(0);

        weaponGroupType_[(int)WeaponType.Punch] = arm.GetChild(0).gameObject.GetComponent<Weapon>();
        weaponGroupType_[(int)WeaponType.Sword] = arm.GetChild(1).gameObject.GetComponent<Weapon>();

        //  TODO    :   未実装
        //weaponGroup_[(int)WeaponType.Shield] = arm.GetChild(2).gameObject;
        //weaponGroup_[(int)WeaponType.Bow] = arm.GetChild(3).gameObject;

        equipmentWeapon_ = weaponGroupType_[(int)WeaponType.Punch].GetComponent<Weapon>();

        for (int lWeaponType = 0; lWeaponType < (int)WeaponType.Max; lWeaponType++)
        {// パンチ以外の武器を停止

            if (!weaponGroupType_[lWeaponType] ||
                lWeaponType == (int)WeaponType.Punch)
                continue;

            weaponGroupType_[lWeaponType].gameObject.SetActive(false);
        }
    }
    protected void Update()
    {
        pos_ = transform.position;
        if (isAttacking_)
        {
            if (--currentAttackFrame_ < 0)
            {// 攻撃から1秒たったら普通の状態
                currentAttackFrame_ = AttackFinishFrame_;
                isAttacking_ = false;
                Logger.RemoveLog(logRegistKey_[(int)LogNum.Attack]);
            }
        }

        if (!isHitting_)
            return;

        currentInvisibleTime_ += Time.deltaTime;
        if (currentInvisibleTime_ > 1f)
        {// 被ダメージ状態から1秒たったら普通の状態
            currentInvisibleTime_ = 0f;
            isHitting_ = false;
            Logger.RemoveLog(logRegistKey_[(int)LogNum.TakeDamage]);
        }
    }

    public void Initialize(int argWeaponTypeIndex, int argLife, Vector2 argEntryPos)
    {
        isLiving_ = true;
        gameObject.SetActive(true);
        life_ = argLife;
        ChangeWeapon(argWeaponTypeIndex);
        transform.position = argEntryPos;
    }

    protected virtual void ChoiceWeapon(WeaponType argWeaponType = WeaponType.Max, GameObject argGameObject = null)
    {
        ChangeWeapon((int)argWeaponType);
        WeaponManager.Instance.RemoveActiveWeapon(argGameObject, 0);
    }

    public virtual void Attack()
    {
        //  TODO    :   武器の当たり判定のON、OFF
        //  TODO    :   animation

        rigid2d_.velocity = new Vector2(0, 0);
        isAttacking_ = true;
        string weaponTypeName = equipmentWeapon_.ThisWeaponType.ToString();
        Logger.Log(logRegistKey_[(int)LogNum.Attack], logRegistKey_[(int)LogNum.Attack] + weaponTypeName);
    }

    public void ChangeWeapon(int argWeaponTypeIndex)
    {
        //  現在の武器をシャットダウン
        equipmentWeapon_.gameObject.SetActive(false);

        //  指定の武器をスタートアップ
        weaponGroupType_[argWeaponTypeIndex].gameObject.SetActive(true);
        equipmentWeapon_ = weaponGroupType_[argWeaponTypeIndex];

    }

    protected void TriggerStay2D(Weapon argWeapon, Collider2D argCollision, int argDamage)
    {
        if (argWeapon && !argCollision.gameObject.transform.parent)
        {// 落ちている武器に触れていれば
            ChoiceWeapon(argWeapon.ThisWeaponType, argCollision.gameObject);
        }
        //  TODO    :   add knock back
        if (isHitting_)
            return;

        //  落ちている武器ではダメージは無し
        if (!argCollision.gameObject.transform.parent)
            return;

        Logger.Log(logRegistKey_[(int)LogNum.TakeDamage], argCollision.tag + " : " + logRegistKey_[(int)LogNum.TakeDamage] + argDamage.ToString() + " Damage!!");

        isHitting_ = true;
        life_ -= argDamage;

        if (life_ <= 0)
        {// 死亡処理
            life_ = 0;
            isLiving_ = false;
        }
    }
}