using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class HealthCollection : Collections
{
    public float healthNumber = 2000;

    private void OnTriggerEnter2D(Collider2D player)
    {
        if (player.gameObject.tag.Equals("Player"))
        {
            if (player.gameObject.GetPhotonView().IsMine)
            {
                player.gameObject.GetComponent<PlayerController>().AddHealth(healthNumber);
                if (hasAudioSource)
                {
                    AudioSourceController.PlayClip(clipNum);
                }
                gameObject.SetActive(false);
                WeaponGeneration.SetHasWeapon(false);
                photonView.RPC("SetSetHasWeaponF",RpcTarget.OthersBuffered,null);
            }
        }
    }
}
