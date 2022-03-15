using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(WeaponGeneration))]
[AddComponentMenu("Photon Networking/Photon WeaponGeneration View")]
public class IPhoton_WeaponStation : MonoBehaviourPun, IPunObservable
{
    [Header("CurrentWeapon 自定义传输")]
    private int currentWeaponNum;

    [SerializeField]
    private int NetworkCurrentWeaponNum; 
    
    
    private WeaponGeneration WeaponGeneration;

    private void Awake()
    {
        WeaponGeneration = GetComponent<WeaponGeneration>();
    }

    private void FixedUpdate()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            currentWeaponNum = WeaponGeneration.GetCurrentWeaponNum();
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                stream.SendNext(currentWeaponNum);
            }
        }
        else
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                NetworkCurrentWeaponNum = (int) stream.ReceiveNext();
                WeaponGeneration.SetCurrentWeaponNum(NetworkCurrentWeaponNum);   
            }
        }
    }
}
