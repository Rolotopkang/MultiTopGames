using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;


[RequireComponent(typeof(AimSystem))]
[AddComponentMenu("Photon Networking/Photon CurrentWeapon View")]
public class IPhoton_Weapon : MonoBehaviourPun,IPunObservable
{
    [Header("CurrentWeapon 自定义传输")] 
    private AimSystem AimSystem;
    private GameObject CurrentWeapon;
    
    private String networkCurrentWeapon;

    private void Awake()
    {
        AimSystem = GetComponent<AimSystem>();
    }

    private void FixedUpdate()
    {
        CurrentWeapon = AimSystem.CurrentWeapon;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(CurrentWeapon.name);
        }
        else
        {
            networkCurrentWeapon = (String)stream.ReceiveNext();
            AimSystem.UpdateCurrentWeapon(networkCurrentWeapon);
        }
    }
}
