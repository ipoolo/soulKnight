﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor.iOS;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Enemy : MonoBehaviour
{

    public Transform BottomLeft;
    public Transform TopRight;
    public float waitTime;
    public float moveSpeed;
    [SerializeField] public float moveSpeedLevelUpScale;
    public float damage;
    [SerializeField] public float damageLevelUpScale;

    public float senseRaidus;

    public GameObject canvasDamage;//绘制伤害
    public GameObject enemyDeathAnim;

    public float maxHealth;
    public float health;
    public float turnRedTime;
    public float patrolTime;
    public float patrolTimer;

    public GameObject psEffect;//这里可以写的基类，提高复用

    private int enemyState = 0; // 没写枚举，直接用数字代替了0 代表巡逻 1代表追击

    private bool isRunning;

    private Rigidbody2D rigid2d;

    private Vector3 targetPosition;
    private GameObject player;
    private Vector3 playerPosition;

    private Color originalColor;
    private SpriteRenderer render;

    private bool isOutControl;
    private float outControlTime = 0.05f;



    // Start is called before the first frame update
    public void Start()
    {
        rigid2d = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        render = GetComponent<SpriteRenderer>();
        originalColor = render.color;
        health = maxHealth;

        //巡逻跟踪时间
        if(patrolTime == 0)
        {
            patrolTime = 2;
        }
        isOutControl = false;
        calculateNewTarget();
    }

    // Update is called once per frame
    public void Update()
    {
        if (!isOutControl) { 
            checkIsFollowToPlayer();

            switch (enemyState)
            {
                case 1:
                    fellowToPlayer();
                    break;
                case 0:
                default:
                    patrol();
                    break;
            }
      }
        
    }

    public virtual bool enemyLevelUpRulesBody(Enemy _enmey, int _level)
    {
        // ture 配置好了，false未配置 调用系统配置
        _enmey.moveSpeed += _level * moveSpeedLevelUpScale;
        _enmey.damage += _level * damageLevelUpScale;
        return true;
    }

    public void checkIsFollowToPlayer()
    {
        playerPosition = player.transform.position;
        if ((playerPosition - transform.position).sqrMagnitude < senseRaidus)
        {
            enemyState = 1;
        }
        else
        {
            enemyState = 0;
        }
    }

    public void fellowToPlayer()
    {
        //将方向拆分给速度
        Vector3 targetVector = playerPosition - transform.position;
        Vector3 normalizedVector = targetVector.normalized;
        Vector3 velocityVector = normalizedVector * moveSpeed;
        rigid2d.velocity = velocityVector;
    }

    private void patrol()
    {
        if (isRunning) {
            patrolTimer += Time.deltaTime;
            //超时或者到达都进入休息，并且寻找新目标(防止目标为不可抵达)
            Debug.DrawLine(transform.position, targetPosition, Color.white);

            Vector3 targetVector = targetPosition - transform.position;
            Vector3 normalizedVector = targetVector.normalized;
            Vector3 velocityVector = normalizedVector * moveSpeed;
            rigid2d.velocity = velocityVector;
            if ((targetPosition - transform.position).sqrMagnitude < 0.2f)
            {
                //等待 然后新目标
                isRunning = false;
                Invoke("calculateNewTarget",waitTime);
                patrolTimer = 0;
                rigid2d.velocity = Vector3.zero;
            }
            if (patrolTimer >= patrolTime)
            {
                isRunning = false;
                Invoke("calculateNewTarget", waitTime);
                patrolTimer = 0;
                rigid2d.velocity = Vector3.zero;
            }

        }


    }

    private void calculateNewTarget()
    {
        targetPosition = new Vector3(Random.Range(BottomLeft.position.x, TopRight.position.x),
            Random.Range(BottomLeft.position.y, TopRight.position.y), 0);
        GameObject target = new GameObject();
        target.transform.position = targetPosition;
        isRunning = true;
    }

    private void OnCollisionEnter2D(Collision2D _collision)
    {
        Collider2D other = _collision.collider;
        Collider2D self = _collision.otherCollider;
        if (other.CompareTag("Player"))
        {
            touchPlayerBody(_collision , other);
        }
        
    }

    public virtual void touchPlayerBody(Collision2D _collision,Collider2D _player)
    {

        PlayerStateController pc = _player.GetComponentInChildren<PlayerStateController>();
        //pc 受伤伤害 TODO
        pc.receiverDamageWithRepelVector(damage, _player.transform.position - transform.position);


    }
    private void OnCollisionExit2D(Collision2D _collision)
    {
        rigid2d.velocity = Vector2.zero;
    }


    public void receiverDamageWithRepelVector(float _damage, Vector3 _repelVector)
    {
        isOutControl = true;
        Debug.Log("_repelVector"+ _repelVector);
        rigid2d.velocity = _repelVector;
        //StartCoroutine("backToUnderControl");
        reduceHealth(_damage);
        renderRed();

    }

    IEnumerator backToUnderControl()
    {
        yield return new WaitForSeconds(outControlTime);
        isOutControl = false;
    }

    private void reduceHealth(float _reduceValue)
    {
        float floorValue = Mathf.FloorToInt(_reduceValue);
        health -= floorValue;
        //掉血粒子效果
        Instantiate(psEffect,transform.position,Quaternion.identity);
        //掉血数值 (暴击值和普通值这里应该通过配置设置不同效果)
        Instantiate(canvasDamage, transform.position, Quaternion.identity).GetComponent<DamageText>().setDamageText(floorValue);

        if (health <= 0)
        {

            //新建死亡效果
            
            Instantiate(Resources.Load("EnemyDeath"), transform.position, Quaternion.identity);
            //新建宝物掉落
            GameObject hp_bar = Instantiate((GameObject)Resources.Load("Coin"));
            hp_bar.transform.position = transform.position;
            
            //销毁自己
            Destroy(gameObject, 0.1f);
        }
        
        Debug.Log("enemyH；"+health);
    }

    private void renderRed()
    {
        render.color = Color.red;
        StartCoroutine("BackToOriginalColor");
    }


    IEnumerator BackToOriginalColor()
    {   
        yield return new WaitForSeconds(turnRedTime);
        render.color = originalColor;
    }

}
