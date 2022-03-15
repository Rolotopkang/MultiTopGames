using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(AimSystem))]
public class WeaponSystem : MonoBehaviourPun
{
    private WeaponData currentWeaponData;
    
    private Queue<WeaponData> weaponDatasQueue = new Queue<WeaponData>();
    private AimSystem aimSystem;
    private WeaponData changedWeapon;
    private void Start()
    {
        //获取列表
        weaponDatasQueue = GetComponent<PlayerWeaponList>().getWeaponDataQueue();
        aimSystem = GetComponent<AimSystem>();

        //检查网络
        //TODO
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
            return;
        
        //如果列表有枪械，赋予第一把枪
        if (weaponDatasQueue.ElementAt(0) != null)
        {
            currentWeaponData = weaponDatasQueue.ElementAt(0);
            NextWeapon();
        }
    }

    private void Update()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
            return;
        KeyDown();
    }

    private void KeyDown()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            NextWeapon();
        }
    }
    private void NextWeapon()
    { 
        changedWeapon = weaponDatasQueue.Dequeue();
        weaponDatasQueue.Enqueue(changedWeapon);
        aimSystem.UpdateCurrentWeapon(changedWeapon.WeaponName);
        currentWeaponData = changedWeapon;
    }

    public Queue<WeaponData> GetWeaponDataQueue()
    {
        return weaponDatasQueue;
    }
}
