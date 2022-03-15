using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class PlayerWeaponList : MonoBehaviour
{
    [Header("武器库设置")] 
    public int maxWeaponCount;
    public String weaponListName;
    public String EXweaponListName;
    
    
    private Queue<WeaponData> weaponDatasQueue = new Queue<WeaponData>();
    private WeaponList weaponList;
    private WeaponList exclusiveWeaponList;
    

    private void Awake()
    {
        weaponList = Resources.Load<WeaponList>(weaponListName);
        exclusiveWeaponList = Resources.Load<WeaponList>(EXweaponListName);

        //以防最大武器数小于初始携带武器数量
        if (maxWeaponCount<weaponList.WeaponDatalist.Count)
        {
            maxWeaponCount = weaponList.WeaponDatalist.Count;
        }
        //初始武器数量入队列
        foreach (WeaponData weapon in weaponList.WeaponDatalist)
        {
            weaponDatasQueue.Enqueue(weapon);
        }
        
    }

    public void addWeapon(WeaponData weaponData)
    {
        //重复检测
        if(weaponDatasQueue.Contains(weaponData))
            return;
        //最大上限替换武器
        if (weaponDatasQueue.Count == maxWeaponCount)
        {
            weaponDatasQueue.Dequeue();
            weaponDatasQueue.Enqueue(weaponData);
        }
        else 
            weaponDatasQueue.Enqueue(weaponData);
    }

    public WeaponData addExclusiveWeapon()
    {
        Random random =new Random();
        int addNum = random.Next(0, exclusiveWeaponList.WeaponDatalist.Count - 1);
        addWeapon(exclusiveWeaponList.WeaponDatalist[addNum]);
        return exclusiveWeaponList.WeaponDatalist[addNum];
    }
    public Queue<WeaponData> getWeaponDataQueue()
    {
        return weaponDatasQueue;
    }
}
