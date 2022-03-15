using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Transactions;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UIElements;

public class AimSystem : MonoBehaviourPunCallbacks
{
    public GameObject CurrentWeapon;
    private GameObject LastWeapon;

    public float weaponAngel;

    private WeaponSystem weaponSystem;
    private Vector3 mouseScreenPosition = Vector3.zero;
    private Vector3 aimDirection = Vector3.zero;
    private Camera mainCamera;
    private SpriteRenderer playersprd;
    private SpriteRenderer weaponsprd;


    private Vector3 localScale;
    private Transform RandomWeapons;
    private Transform ExclusiveWeapons;

    private void Awake()
    {
        mainCamera = Camera.main;
        playersprd = GetComponentInParent<SpriteRenderer>();
        weaponsprd = CurrentWeapon.GetComponent<SpriteRenderer>();
        CurrentWeapon = transform.GetChild(0).gameObject;
        RandomWeapons = transform.GetChild(1);
        ExclusiveWeapons = transform.GetChild(2);
    }

    private void Update()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
            return;
        Aim();
    }

    private void Aim()
    {
        Vector3 posScale = new Vector3();
        mouseScreenPosition = Input.mousePosition;
        aimDirection = mouseScreenPosition - mainCamera.WorldToScreenPoint(
            CurrentWeapon.transform.position);
        aimDirection.z = 0;
        aimDirection.Normalize();
        weaponAngel = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        //武器翻转
        if (weaponAngel > 90 || weaponAngel < -90)
        {
            if (CurrentWeapon.transform.localScale.y>0)
            {
                posScale = new Vector3(CurrentWeapon.transform.localScale.x, -CurrentWeapon.transform.localScale.y,
                    CurrentWeapon.transform.localScale.z);
            }
            else
            {
                posScale = new Vector3(CurrentWeapon.transform.localScale.x, CurrentWeapon.transform.localScale.y,
                    CurrentWeapon.transform.localScale.z);
            }
            CurrentWeapon.transform.localScale = posScale;
        }
        else
        {
            if (CurrentWeapon.transform.localScale.y<0)
            {
                posScale = new Vector3(CurrentWeapon.transform.localScale.x, -CurrentWeapon.transform.localScale.y,
                    CurrentWeapon.transform.localScale.z);
            }
            else
            {
                posScale = new Vector3(CurrentWeapon.transform.localScale.x, CurrentWeapon.transform.localScale.y,
                    CurrentWeapon.transform.localScale.z);
            }
            CurrentWeapon.transform.localScale = posScale;
        }
        CurrentWeapon.transform.eulerAngles = new Vector3(0, 0, weaponAngel);
    }

    public void UpdateCurrentWeapon(String weaponName)
    {
        CurrentWeapon.SetActive(false);
        if (FindWeaponInChild(RandomWeapons, ExclusiveWeapons, weaponName).gameObject == null)
        {
            CurrentWeapon = transform.GetChild(0).gameObject;
            Debug.Log("actor上找不到对应武器"+weaponName);
        }
        else
        {
            CurrentWeapon = FindWeaponInChild(RandomWeapons, ExclusiveWeapons, weaponName).gameObject;
        }

        CurrentWeapon.gameObject.SetActive(true);
        weaponsprd = CurrentWeapon.GetComponent<SpriteRenderer>();
    }

    public Transform FindWeaponInChild(Transform father1 ,Transform father2 ,String weaponName)
    {
        Transform forreturn ;
        forreturn = father1.Find(weaponName);
        if (forreturn==null)
        {
            forreturn = father2.Find(weaponName);
        }
        return forreturn;
    }
}
