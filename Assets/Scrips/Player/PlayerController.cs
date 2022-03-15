using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine.UI;

public class PlayerController : MonoBehaviourPun
{
    [Header("动态参数")] 
    //actor参数
    //private @ [SerializeField]
    [SerializeField] private bool jumpPresseed;
    [SerializeField] private int JumpTimes;
    [SerializeField] private bool isFrieGround;
    [SerializeField] private bool isGround;
    [SerializeField] private bool isJump;
    [SerializeField] private float health;
    [SerializeField] public bool isShooting = false;
    [SerializeField] private bool isDeath = false;
    [SerializeField] private bool isThroughPlatForms;

    //可调整参数
    //public 
    [Header("可调整参数")] 
    public float speed;
    public float jumpstrenth;
    public int MaxJumpTimes;
    public float maxHealth;

    //定位辅助
    [Header("环境检测")] 
    public float footOffset = 0.4f;
    public float headClearance = 0.5f;
    public float groundDistance = 1.2f;

    public LayerMask fireGround;
    public LayerMask ground;

    //组件
    [Header("组件")]
    public Image RealoadingUI;
    public Text NameText;
    public Image HealthBar;
    public Text PingText;
    private GameObject GameUI;
    private GameObject DeathUI;

    private SpriteRenderer sprd;
    private Rigidbody2D rb2d;
    private Animator anim;
    private BoxCollider2D boxCl2d;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            NameText.text = PhotonNetwork.NickName;
        }
        else
        {
            NameText.text = photonView.Owner.NickName;
        }
        //人物属性初始化
        health = maxHealth;
        isDeath = false;
        //组件获取
        GameUI = GameObject.Find("GameUI");
        DeathUI = GameUI.transform.Find("DeathUI").gameObject;
        sprd = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        boxCl2d = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        //添加到玩家池
        GameManager.AddPlayerList(photonView.Owner,gameObject);
    }

    private void FixedUpdate()
    {
        NetworkMovement();
        //多人检测脚本是否是自己的角色
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
            return;
        PhysicsCheck();
        LocalMovement();
        SwitchAnim();
        DeadCheck();
    }
    //网络移动
    private void NetworkMovement()
    {
        //平台下落
        if (isThroughPlatForms)
        {
            gameObject.layer = 11;
        }
        else
        {
            gameObject.layer = 8;
        }
    }
    //本地移动
    private void LocalMovement()
    {
        float horizontalMove = Input.GetAxisRaw("Horizontal");
        float jumpMove = Input.GetAxis("Jump");
        
        //人物左右移动
        rb2d.velocity = new Vector2(speed * horizontalMove * Time.fixedDeltaTime, rb2d.velocity.y);
        
        if (isGround)
        {
            JumpTimes = MaxJumpTimes;
            isJump = false;
            anim.SetBool("jumping",false);
        }

        if (jumpPresseed && isGround)
        {
            isJump = true;
            // jumping.Play();
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpstrenth * Time.fixedDeltaTime);
            JumpTimes--;
            jumpPresseed = false;
        }
        else if (jumpPresseed && JumpTimes > 0 && isJump)
        {
            // jumping.Play();
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpstrenth * Time.fixedDeltaTime);
            JumpTimes--;
            jumpPresseed = false;
        }
        else if (jumpPresseed && JumpTimes > 0 && !isGround && rb2d.velocity.y < 0)
        {
            // jumping.Play();
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpstrenth * Time.fixedDeltaTime);
            JumpTimes--;
            jumpPresseed = false;
        }
        //改变人物朝向
        if (rb2d.velocity.x > 0 && !isShooting)
        {
            sprd.flipX = false;
        }
        else if(rb2d.velocity.x < 0 && !isShooting)
        {
            sprd.flipX = true;
        }
    }

    void PhysicsCheck()
    {
        RaycastHit2D leftcheck = Raycast(new Vector2(-footOffset, 0f),
            Vector2.down, groundDistance, ground);
        RaycastHit2D rightcheck = Raycast(new Vector2(footOffset, 0f),
            Vector2.down, groundDistance, ground);
        if (leftcheck || rightcheck)
            isGround = true;
        else
            isGround = false;
        
        isFrieGround = Raycast(new Vector2(footOffset, 0f),
            Vector2.down, groundDistance,fireGround, false);
    }


    private void OnTriggerEnter2D(Collider2D otherCollision)
    {
        if (otherCollision.gameObject.tag.Equals("bullets") 
            && !otherCollision.gameObject.GetComponent<Projectile>().owner.Equals(photonView.Owner)
            && !otherCollision.gameObject.GetComponent<Projectile>().owner.GetPhotonTeam().Equals(photonView.Owner.GetPhotonTeam()))
        {
            if (photonView.IsMine)
            {
                float harm_temp = otherCollision.gameObject.GetComponent<Projectile>().harm;
                health = health - harm_temp;
            }
            
            //回收子弹
            otherCollision.transform.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        UIRefresh();
        //多人检测脚本是否是自己的角色
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
            return;
        HealthUpdate();
        Keydown();
    }
    
    private void DeadCheck()
    {
        if (health <= 0 && !isDeath)
        {
            isDeath = true;
            anim.SetTrigger("death");
            rb2d.constraints = RigidbodyConstraints2D.FreezePosition;
            boxCl2d.isTrigger = true;
            DeathUI.SetActive(true);
        }
    }
    private void HealthUpdate()
    {
        if (isFrieGround)
        {
            health--;
        }
    }
    private void UIRefresh()
    {
        HealthBar.fillAmount = health / maxHealth;
        PingText.text =PhotonNetwork.GetPing().ToString();
    }
    
    private void SwitchAnim()
    {
        anim.SetFloat("running", Mathf.Abs(rb2d.velocity.x));

        if (isGround)
        {
            anim.SetBool("falling", false);
        }
        else if (!isGround && rb2d.velocity.y >= 0)
        {
            anim.SetBool("jumping", true);
            anim.SetBool("falling", false);
        }
        else if (!isGround && rb2d.velocity.y < 0)
        {
            anim.SetBool("jumping", false);
            anim.SetBool("falling", true);
        }

        // if (isHurt)
        // {
        //     anim.SetBool("hurt", true);
        //     anim.SetFloat("running", 0);
        //     if (Mathf.Abs(rb2d.velocity.x) < 0.1f)
        //     {
        //         isHurt = false;
        //         anim.SetBool("hurt", false);
        //         anim.SetBool("idle",true);
        //     }
        // }
    }
    
    Boolean Raycast(Vector2 offset, Vector2 rayDiraction, float length, LayerMask layer,bool bothfoot)
    {
        Vector2 pos = transform.position;
        RaycastHit2D hitRight = Physics2D.Raycast(pos + offset, rayDiraction, length, layer);
        RaycastHit2D hitLeft = Physics2D.Raycast(pos - offset, rayDiraction, length, layer);
        Color colorRight = hitRight ? Color.green : Color.red;
        Color colorLeft = hitLeft ? Color.green : Color.red;
        Debug.DrawRay(pos+(offset/2),rayDiraction*length,colorRight);
        Debug.DrawRay(pos-(offset/2),rayDiraction*length,colorLeft);
        if(!bothfoot) return (hitLeft || hitRight);
        else return (hitLeft && hitRight);
    }

    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDiraction, float length, LayerMask layer)
    {
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDiraction, length, layer);
        Color color = hit ? Color.green : Color.red;
        Debug.DrawRay(pos+offset,rayDiraction*length,color);
        return hit;
    }
    
    private void Keydown()
    {
        if (Input.GetButtonDown("Jump") && JumpTimes > 0)
        {
            jumpPresseed = true;
        }

        if (Input.GetKeyDown(KeyCode.S) && !isThroughPlatForms)
        {
            isThroughPlatForms = true;
        }
        else if(Input.GetKeyUp(KeyCode.S) && isThroughPlatForms)
        {
            Invoke(nameof(ReturnLayer),0.4f);
        }
    }


    private void ReturnLayer()
    {
        isThroughPlatForms = false;
    }
    
    public float getHealth()
    {
        return health;
    }

    public void SetHealth(float _health)
    {
        health = _health;
    }

    public void AddHealth(float add)
    {
        if (photonView.IsMine)
        {
            health += add;
            if (health > maxHealth)
            {
                health = maxHealth;
            }
        }
    }

    public void SetisThroughPlatForms(bool _isThroughPlatForms)
    {
        isThroughPlatForms = _isThroughPlatForms;
    }
    
    public bool GetisThroughPlatForms()
    {
        return isThroughPlatForms;
    }
    
    //animation call
    public void delPlayer()
    { 
        //自己网络移除自己
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(this.transform.gameObject);
        }
    }

    private void OnDestroy()
    {
        GameManager.DelPlayerList(photonView.Owner,gameObject);
    }
    
}
