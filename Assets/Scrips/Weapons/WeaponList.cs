using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "ScriptableObject/Weapon/NewWeaponList",fileName = "NewWeaponList")]
public class WeaponList : ScriptableObject
{
    public List<WeaponData> WeaponDatalist;
}
