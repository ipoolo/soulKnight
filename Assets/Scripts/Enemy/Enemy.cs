﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Enemy : MonoBehaviour
{
    public Transform BottomLeft;
    public Transform TopRight;
    public float waitTime;
    public float moveSpeed;
    public float senseRaidus;

    public GameObject canvasDamage;
    public GameObject enemyDeathAnim;

    public float maxHealth;
    public float health;
    public float turnRedTime;

    public GameObject psEffect;//这里可以写的基类，提高复用

    private int enemyState = 0; // 没写枚举，直接用数字代替了0 代表巡逻 1代表追击

    private bool isRunning;

    private Rigidbody2D rigid2d;

    private Vector3 targetPosition;
    private GameObject player;
    private Vector3 playerPosition;

    private Color originalColor;
    private SpriteRenderer render;


    // Start is called before the first frame update
    public void Start()
    {
        rigid2d = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        render = GetComponent<SpriteRenderer>();
        originalColor = render.color;
        health = maxHealth;

        calculateNewTarget();
    }

    // Update is called once per frame
    public void Update()
    {
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
        transform.position = Vector3.MoveTowards(transform.position, playerPosition, Time.deltaTime * moveSpeed);
    }

    private void patrol()
    {
        if (isRunning) { 
            if((targetPosition - transform.position).sqrMagnitude < 0.2f)
            {
                //等待 然后新目标
                isRunning = false;
                Invoke("calculateNewTarget",waitTime);
            }
            transform.position = Vector3.MoveTowards(transform.position, targetPosition,Time.deltaTime * moveSpeed);
        }


    }

    private void calculateNewTarget()
    {
        targetPosition = new Vector3(UnityEngine.Random.Range(BottomLeft.position.x, TopRight.position.x),
            UnityEngine.Random.Range(BottomLeft.position.y, TopRight.position.y), 0);
        isRunning = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        rigid2d.velocity = Vector2.zero;
    }

    public void receiverDamage(float _damage, Vector3 _hitPosition , float _repelDistance)
    {

        //击退
        Vector3 temp = transform.position - _hitPosition;
        temp = temp.normalized * _repelDistance;
        transform.position += temp;

        //扣血
        reduceHealth(_damage);

        //变红
        renderRed();

    }

    public void receiverDamageWithRepelVector(float _damage, Vector3 _repelVector)
    {
        transform.position += _repelVector;


        reduceHealth(_damage);

        renderRed();

    }

    private void reduceHealth(float _reduceValue)
    {
        health -= _reduceValue;
        //掉血粒子效果
        Instantiate(psEffect,transform.position,Quaternion.identity);
        //掉血数值 (暴击值和普通值这里应该通过配置设置不同效果)
        Instantiate(canvasDamage, transform.position, Quaternion.identity).GetComponent<DamageText>().setDamageText(_reduceValue);

        if (health <= 0)
        {

            //新建死亡效果
            Instantiate(enemyDeathAnim, transform.position, Quaternion.identity);
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
