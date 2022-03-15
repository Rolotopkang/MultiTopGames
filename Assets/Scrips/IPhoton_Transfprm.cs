using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class IPhoton_Transfprm : MonoBehaviourPun, IPunObservable
{
    Vector3 currPos = Vector3.zero;
    Vector3 currScale = Vector3.zero;
    Quaternion currRot = Quaternion.identity;

    private void Awake()
    {
        //设置传送数据类型 (同步属性)
        photonView.Synchronization= ViewSynchronization.UnreliableOnChange;
        //设置player位置的初始值
        currPos = transform.position;
        currScale = transform.localScale;
        currRot = transform.rotation;
    }
    
    private void Update()
    {
        if (!photonView.IsMine)
        {
            transform.position = Vector3.Lerp(transform.position, currPos, Time.deltaTime * 3.0f);
            transform.localScale = Vector3.Lerp(transform.localScale, currScale, Time.deltaTime * 3.0f);
            transform.rotation = Quaternion.Slerp(transform.rotation,currRot, Time.deltaTime * 3.0f);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //传送本地玩家的位置（和旋转等）信息
        if(stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.localScale);
            stream.SendNext(transform.rotation);
        }
        else//接收远程玩家的位置信息
        {
            currPos = (Vector3)stream.ReceiveNext();
            currScale = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
