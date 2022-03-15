using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Projectile : MonoBehaviourPun
{
    //属性
    public Player owner;
    public float initialSpeed;
    //组件
    private CapsuleCollider2D coll;
    private Rigidbody2D rb2d;
    private TrailRenderer trail;

    public float MaxLifeTimer = 7;
    public float harm;
    public float lifeTimer;
    
    protected void Awake()
    {
        coll = GetComponent<CapsuleCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();
        trail = GetComponentInChildren<TrailRenderer>();
        lifeTimer = MaxLifeTimer;
    }

    private void OnEnable()
    {
        lifeTimer = MaxLifeTimer;
    }

    protected void Update()
    {
        doRecycle();
    }

    protected void doRecycle()
    {
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    protected void OnTriggerEnter2D(Collider2D otherCollision)
    {
        if (otherCollision.gameObject.tag.Equals("StaticObject"))
        {
            //play子弹碰撞动画
            //摧毁自己
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 删除轨迹
    /// </summary>
    private void OnDisable()
    {
        trail.Clear();
    }
}
