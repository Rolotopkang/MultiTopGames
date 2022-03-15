using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Random = System.Random;

public class WeaponGeneration : MonoBehaviourPun
{
    public float resetTime;

    [Header("状态")] 
    [SerializeField]
    private bool HasWeapon;
    [SerializeField]
    private int currentWeaponNum = 1;

    private float timer=0;
    private WeaponData currentWeaponData;
    [SerializeField]
    private int weaponListMaxLength;
    [SerializeField]
    private Transform[] weaponObjects;


    private IPhoton_WeaponStation PhotonWeaponStation;
    private void Awake()
    {
        weaponListMaxLength = GetComponentsInChildren<Transform>(true).Length;
        weaponObjects = GetComponentsInChildren<Transform>(true);
        PhotonWeaponStation = GetComponent<IPhoton_WeaponStation>();
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            ResetTimer();
        }
        SwitchSprite();
    }
    
    private void SwitchSprite()
    {
        if (!HasWeapon)
        {
            for(int i=1;i<weaponObjects.Length;i++)
            {
                if (i == 1)
                {
                    weaponObjects[i].gameObject.SetActive(true);
                }
                else
                {
                    weaponObjects[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            for(int i=1;i<weaponObjects.Length;i++)
            {
                if (i == currentWeaponNum)
                {
                    weaponObjects[i].gameObject.SetActive(true);
                }
                else
                {
                    weaponObjects[i].gameObject.SetActive(false);
                }
            }
        }
    }
    
    private void ResetTimer()
    {
        if (!HasWeapon)
        {
            timer += Time.deltaTime;
            if (timer>= resetTime)
            {
                currentWeaponNum = RandomWeaponNumber();
                timer = 0;
                HasWeapon = true;
                photonView.RPC("SetSetHasWeaponT",RpcTarget.OthersBuffered,null);
            }
        }
    }
    
    private int RandomWeaponNumber()
    {
        Random r = new Random();
        return r.Next(2,weaponListMaxLength);
    }

    [PunRPC]
    public void SetSetHasWeaponT()
    {
        HasWeapon = true;
    }
    
    [PunRPC]
    public void SetSetHasWeaponF()
    {
        HasWeapon = false;
    }
    
    public void SetHasWeapon(bool set)
    {
        HasWeapon = set;
    }

    public void SetCurrentWeaponNum(int set)
    {
        currentWeaponNum = set;
    }
    
    public bool GetHasWeapon()
    {
        return HasWeapon;
    }

    public int GetCurrentWeaponNum()
    {
        return currentWeaponNum;
    }
}
