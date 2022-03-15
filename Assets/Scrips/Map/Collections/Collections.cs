using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Collections : MonoBehaviourPun
{
    [Header("音效")]
    public bool hasAudioSource;
    public int clipNum;
    
    protected AudioSourceController AudioSourceController;
    protected WeaponGeneration WeaponGeneration;
    protected IPhoton_WeaponStation PhotonWeaponStation;
    
    protected void Awake()
    {
        WeaponGeneration = GetComponentInParent<WeaponGeneration>();
        AudioSourceController = GetComponentInParent<AudioSourceController>();
        PhotonWeaponStation = GetComponentInParent<IPhoton_WeaponStation>();
    }
    
    public void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }
}
