using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class WeaponController : MonoBehaviourPun
{
    [Header("按键状态")] 
    [SerializeField] private bool mouse0; 
    
    [Header("武器属性")] 
    public WeaponData weaponData;
    [SerializeField] private float maxReloadTime;
    [SerializeField] private int maxBulletNum;
    
    private SpriteRenderer OwnerRenderer;
    private BulletData bulletData;
    private AimSystem AimSystem;

    [Header("重新装填属性")] 
    [SerializeField]private int bulletNum;
    [SerializeField]private float reloadTime;
    [SerializeField] public bool isReloading=false;
    private PlayerController _playerController;
    private Image playerRealoadingUI;
    [SerializeField] private float useTimer = 0;
    
    [Header("外部插件")] 
    public AudioSource shootAudio;
    
    
    private GameObject gunOwner;

    private Transform firePOS;
    private void Awake()
    {
        useTimer = weaponData.UseSpeed;
        firePOS = gameObject.transform.GetChild(0);
        AimSystem = transform.parent.parent.GetComponent<AimSystem>();
        gunOwner = gameObject.transform.parent.parent.parent.gameObject;
        OwnerRenderer = gunOwner.GetComponent<SpriteRenderer>();
        _playerController = gunOwner.GetComponent<PlayerController>();
        playerRealoadingUI = _playerController.RealoadingUI;
        bulletData = weaponData.UsingBullet;
        maxBulletNum = weaponData.MaxBulletNum;
        maxReloadTime = weaponData.ReloadTime;
        bulletNum = maxBulletNum;
    }

    private void Update()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
            return;
        KeyDown();
    }
    
    private void KeyDown()
    {
        if (Input.GetMouseButton(0)&& !isReloading)
        {
            ChangeRenderer();
            if (bulletNum > 0)
            { 
                ShootTimer();
            }
            else
            {
                ReloadGun();
            }
        }else { _playerController.isShooting = false; ReTimer(); }
        
        
        
        if (Input.GetKeyDown(KeyCode.R) && bulletNum!=maxBulletNum && !isReloading)
        {
            ReloadGun();
        }
        if (isReloading)
        {
            playerRealoadingUI.gameObject.SetActive(true); 
            ReloadTimer(); 
            if (reloadTime >= maxReloadTime)
            {
                 isReloading = false;
                 playerRealoadingUI.gameObject.SetActive(false);
                 bulletNum = maxBulletNum;
                 reloadTime = 0;
            }
        }else playerRealoadingUI.gameObject.SetActive(false);
    }

    private void ChangeRenderer()
    {
        //修改类型
        //TODO
        gunOwner.GetComponent<PlayerController>().isShooting = true;
        if (AimSystem.weaponAngel <= 90 && AimSystem.weaponAngel >= -90)
        {
            OwnerRenderer.flipX = false;
        }
        else
        {
            OwnerRenderer.flipX = true;
        }
    }
    private void ReTimer()
    {
        if(useTimer<weaponData.UseSpeed)
            useTimer += Time.deltaTime;
    }
    
    
    private void ShootTimer()
    {
        if(!(useTimer>=weaponData.UseSpeed)) useTimer += Time.deltaTime;
        //射击
        if (useTimer >= weaponData.UseSpeed)
        { 
            Shoot(); 
            photonView.RPC("Shoot",RpcTarget.Others,null); 
            bulletNum--; useTimer = 0;
        }
    }
    
    private void ReloadGun()
    {
        isReloading = true;
        playerRealoadingUI.gameObject.SetActive(true);
    }

    private void ReloadTimer()
    {
        reloadTime += Time.deltaTime;
        playerRealoadingUI.fillAmount = reloadTime / maxReloadTime;
    }
    
    [PunRPC]
    private void Shoot()
    {
        GameObject bullet_temp = PoolManager.Release(bulletData.BulletPrefab,
            firePOS.position, transform.rotation,photonView.Owner,weaponData.shootingStrength).gameObject;
        bullet_temp.GetComponent<Projectile>().harm = weaponData.Damege * weaponData.UsingBullet.bulletDamage;
        bullet_temp.GetComponent<Rigidbody2D>().velocity = firePOS.right*weaponData.shootingStrength;
        shootAudio.Play();
    }


    public void SetReloadingTime(float set)
    {
        reloadTime = set;
    }
}
