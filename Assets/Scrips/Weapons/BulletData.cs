using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Bullet/NewBullet",fileName = "NewBullet")]
public class BulletData : ScriptableObject
{
    public float bulletDamage;
    public String bulletName;
    [TextArea] public String description;
    public GameObject BulletPrefab;
}
