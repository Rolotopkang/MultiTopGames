using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

public class TrackProjectile : MonoBehaviourPun
{
    [SerializeField] protected GameObject target;
    private List<GameObject> inRoomplayerList;
    
    private Projectile projectile;
    private Rigidbody2D rb2d;

    private bool isHomingCoroutine;
    private float minLength;
    private Coroutine homing;
    private float targetStraightDirection;
    private float enhanceHoming;
        

    [SerializeField] private float bulletspeed;
    [Range(0,10f)] public float homingStrenth;
    [SerializeField] private float homingcoroutineRange = 5;
    private void Awake()
    {
        projectile = GetComponent<Projectile>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        bulletspeed = GetComponent<Projectile>().initialSpeed;
        isHomingCoroutine = false;
        inRoomplayerList = GameManager.inRoomPlayerDictionary.Values.ToList();
    }

    private void Update()
    {
        target = findTarget(inRoomplayerList);
        if (target != null && !isHomingCoroutine && minLength<=homingcoroutineRange)
        {
            isHomingCoroutine = true;
            homing=StartCoroutine(HomingCoroutine(target));
        }else if (target != null && isHomingCoroutine && minLength > homingcoroutineRange)
        {
            StopCoroutine(homing);
        }
    }

    private GameObject findTarget(List<GameObject> inRoomplayerList)
    {
        GameObject targetPlayer = null;
        minLength = Single.MaxValue;
        float temp;
        foreach (var player in inRoomplayerList)
        {
            if (player == null) { continue; }
            if (!player.GetComponent<PhotonView>().Owner.GetPhotonTeam().Equals(projectile.owner.GetPhotonTeam()))
            {
                temp = (player.transform.position - transform.position).magnitude;
                if (temp<minLength)
                {
                    minLength = temp;
                    targetPlayer = player;
                }
            }
        }
        return targetPlayer;
    }

    IEnumerator HomingCoroutine(GameObject target)
    {
        while (gameObject.activeSelf)
        {
            if (target.activeSelf && target!=null)
            {
                Vector3 targetDirection = target.transform.position - transform.position;
                targetStraightDirection = targetDirection.magnitude;
                if (targetStraightDirection < 1) { targetStraightDirection = 1;}
                enhanceHoming = homingcoroutineRange / targetStraightDirection;
                targetDirection = targetDirection.normalized;
                Vector2 dir = Vector2.Lerp(rb2d.velocity.normalized,targetDirection,homingStrenth*enhanceHoming/100);
                rb2d.velocity = dir * bulletspeed;
                float angle = Mathf.Atan2(rb2d.velocity.y,rb2d.velocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
            yield return null;
        }
    }
}
