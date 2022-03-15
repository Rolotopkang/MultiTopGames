using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;



[CreateAssetMenu(menuName = "ScriptableObject/Weapon/NewWeapon",fileName = "NewWeapon")]
public class WeaponData : ScriptableObject
{
    public Sprite WeaponSprite;
    public String WeaponName;
    [TextArea]public String Description;
    public GameObject WeaponPrefab;
    public BulletData UsingBullet;

    public int shootingStrength;
    public float UseSpeed;
    public float Damege;
    public float KnockBack;
    public float SelfKnockBack;
    public float ReloadTime;
    public int MaxBulletNum;
}
