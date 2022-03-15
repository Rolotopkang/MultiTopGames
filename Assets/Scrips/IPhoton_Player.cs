using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PlayerController))]
[AddComponentMenu("Photon Networking/Photon Player View")]
public class IPhoton_Player : MonoBehaviourPun ,IPunObservable
{
    //属性
    [Header("player 自定义传输")]
    public bool sendHealth = true;
    public bool ThroughPlatForms = true;
    
    //network属性
    private float networkHealth;
    private bool networkisThroughPlatForms;
    
    //组件
    private PlayerController playerController;


    private void Awake()
    {
        this.playerController = GetComponent<PlayerController>();
    }
    
    private void FixedUpdate()
    {
        if (!this.photonView.IsMine)
        {
            if(sendHealth) 
                playerController.SetHealth(networkHealth);
            if (ThroughPlatForms)
                playerController.SetisThroughPlatForms(networkisThroughPlatForms);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            if(sendHealth) 
                stream.SendNext(playerController.getHealth());
            if(ThroughPlatForms)
                stream.SendNext(playerController.GetisThroughPlatForms());
        }
        else
        {
            if(sendHealth) 
                networkHealth = (float) stream.ReceiveNext();
            if (ThroughPlatForms)
                networkisThroughPlatForms = (bool)stream.ReceiveNext();
        }
    }
}
