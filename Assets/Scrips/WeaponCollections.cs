using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class WeaponCollections : Collections
{
    public WeaponData weaponData;
    

    private void OnTriggerEnter2D(Collider2D player)
    {
        if (player.gameObject.tag.Equals("Player"))
        {
            if (player.gameObject.GetPhotonView().IsMine)
            {
                player.gameObject.GetComponentInChildren<PlayerWeaponList>().addWeapon(weaponData);
                player.gameObject.GetComponentInChildren<AimSystem>().UpdateCurrentWeapon(weaponData.WeaponName);
                if (hasAudioSource) { AudioSourceController.PlayClip(clipNum);}
                gameObject.SetActive(false);
                WeaponGeneration.SetHasWeapon(false);
                photonView.RPC("SetSetHasWeaponF",RpcTarget.OthersBuffered,null);
            }
        }
    }
    
}
