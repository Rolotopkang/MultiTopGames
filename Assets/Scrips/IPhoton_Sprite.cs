using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[AddComponentMenu("Photon Networking/Photon Sprite View")]
public class IPhoton_Sprite : MonoBehaviourPun,IPunObservable
{
    [Header("Sprite 自定义传输")] 

    public bool flipx = true;
    public bool flipy = false;

    
    
    private bool networkFlipx;
    private bool networkFlipy;
    
    private SpriteRenderer sprd;

    private void Awake()
    {
        this.sprd = GetComponent<SpriteRenderer>();
    }
    
    private void FixedUpdate()
    {
        if (!this.photonView.IsMine)
        {
            if (flipx)this.sprd.flipX = networkFlipx;
            if (flipy)this.sprd.flipY = networkFlipy;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            if (flipx)
            {
                stream.SendNext(this.sprd.flipX);
            }

            if(flipy)
                stream.SendNext(this.sprd.flipY);
        }
        else
        {
            if (flipx)
            {
                networkFlipx = (bool) stream.ReceiveNext();
            }
            
            if (flipy)
            {
                networkFlipy = (bool) stream.ReceiveNext();
            }
        }
    }
}
