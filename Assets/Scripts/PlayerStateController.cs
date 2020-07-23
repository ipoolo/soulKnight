﻿using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class PlayerStateController : MonoBehaviour
{
    public GameObject statePanel;
    public StatePanelController spController;

    public int health;
    public int maxHealth;
    public int armor;
    public int maxArmor;
    public int mana;
    public int maxMana;

    public int coinNum;
    public float receiveRepelVectorScale;
    public GameObject player;

    public float recoverArmorStepTime;
    public int recoverArmorStepValue;
    public float recoverArmorInterruptTime;
    private float recoverArmorCounter;
    private float recoverArmorInterruptCounter;

    private bool isRecoverArmor;

    private CoinAreaController cac;

    public GameObject psEffect;//粒子效果
    public GameObject canvasDamage;
    public SpriteRenderer render;
    private Color originalColor;
    private bool isLayerShake;
    private bool isReceiveDamage;

    public float canvasDamageOffsetY;

    [SerializeField] public float invincibilityTime;

    // Start is called before the first frame update
    void Start()
    {
        statePanel = GameObject.FindGameObjectWithTag("StatePanel");
        spController = statePanel.GetComponent<StatePanelController>();

        configDefault();
        updateStatePlane();


    }

    private void configDefault()
    {
        //coin 
        coinNum = 0;
        cac = GameObject.FindGameObjectWithTag("CoinAreaController").GetComponent<CoinAreaController>();
        player = GameObject.FindGameObjectWithTag("Player") as GameObject;
        psEffect = Resources.Load("Partcles/PS_BloodEffect") as GameObject;
        canvasDamage = Resources.Load("CanvasDamage") as GameObject;
        isRecoverArmor = true;
        render = gameObject.GetComponentInParent<SpriteRenderer>();
        originalColor = render.color;

        if (invincibilityTime == 0)
        {
            invincibilityTime = 2.0f;
        }
        if(canvasDamageOffsetY == 0)
        {
            canvasDamageOffsetY = 0.5f;
        }
        if(receiveRepelVectorScale == 0.0f)
        {
            receiveRepelVectorScale = 1.0f;
        }

        isReceiveDamage = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRecoverArmor)
        {
            recoverArmorCounter += Time.deltaTime;
            if (recoverArmorCounter >= recoverArmorStepTime)
            {
                //等待时间触发恢复
                recoverArmor(recoverArmorStepValue);
                //reset
                recoverArmorCounter = 0;
            }
        }
        else
        {
            recoverArmorInterruptCounter += Time.deltaTime;
            if (recoverArmorInterruptCounter >= recoverArmorInterruptTime)
            {
                isRecoverArmor = true;
                recoverArmorInterruptCounter = 0;
                //中断结束立刻恢复一次(offset一下恢复计数器达到效果)
                recoverArmorCounter = recoverArmorStepTime;
            }
        }
    }

    private bool receiveDamage(int damage)
    {
        bool receiveSuccess = true;
        int temp = armor - damage;

        //开启中断recoverArmorInterruptCounter
        isRecoverArmor = false;
        recoverArmorInterruptCounter = 0;//重新计时

        if (temp >= 0)
        {
            //防御足够吸收
            armor = temp;
        }
        else
        {
            armor = 0;
            health -= math.abs(temp);
            if (health <= 0)
            {
                //血量不足以吸收伤害
                health = 0;
                receiveSuccess = false;
                //TODO
                Debug.Log("PlayerDeal");
            }
        }
        updateStatePlane();
        return receiveSuccess;
    }

    public bool receiveManaReduce(int manaReduce)
    {
        if (mana == 0)
        {
            return false;
        }

        int temp = mana - manaReduce;
        if (temp >= 0)
        {
            mana = temp;
        }
        else
        {
            mana = 0;
        }

        updateStatePlane();

        return true;
    }

    private void updateStatePlane()
    {
        spController.changeHealth(health, maxHealth);
        spController.changeArmor(armor, maxArmor);
        spController.changeMana(mana, maxMana);
    }

    private void recoverArmor(int i)
    {
        armor += 1;
        armor = armor >= maxArmor ? maxArmor : armor;
        updateStatePlane();
    }

    //coin
    //coinSetting
    public void coinAdd(int addeNum)
    {
        coinNum += addeNum;
        //update coinUI
        updateCoinUI();
    }

    public void updateCoinUI()
    {
        cac.updateCoinnNum(coinNum);
    }

    public bool coinReduce(int reduceNum)
    {
        if (coinNum >= reduceNum)
        {
            coinNum -= reduceNum;
            //update coinUI
            updateCoinUI();
            return true;
        }
        else
        {
            //coin ui_text Shake
            cac.CoinAreaShake();
            return false;
        }

    }

    //receiveDamage
    public void receiverDamageWithRepelVector(float _damage, Vector3 _repelVector)
    {

        if (isReceiveDamage)
        {
            isReceiveDamage = false;
            Vector3 tmp = _repelVector.normalized * 10;
            player.GetComponent<Rigidbody2D>().velocity = tmp;

            //test
            player.GetComponent<PlayerController>().isOutController = true;
            StartCoroutine("playerBackToContoller");
            //test-e
            reduceHealth(_damage);
            renderWhiteAndTurnInvincibilityLayer();
        }
    }

    IEnumerator playerBackToContoller()
    {
        yield return new WaitForSeconds(0.05f);
        player.GetComponent<PlayerController>().isOutController = false;
        
    } 

    private void reduceHealth(float _reduceValue)
    {
        int floorValue = Mathf.FloorToInt(_reduceValue);

        //掉血粒子效果 （可以设置专门的出血位）
        Instantiate(psEffect, transform.position, Quaternion.identity);
        //掉血数值 (暴击值和普通值这里应该通过配置设置不同效果)
        Vector3 tmp = transform.position;
        tmp += new Vector3(0, canvasDamageOffsetY,0);
        Instantiate(canvasDamage, tmp, Quaternion.identity).GetComponent<DamageText>().setDamageText(floorValue);
        //扣血
        receiveDamage(floorValue);

    }

    private void renderWhiteAndTurnInvincibilityLayer()
    {
        render.color = Color.red;
        StartCoroutine("BackToOriginalColorAndUninvincibility");
        player.layer = LayerMask.NameToLayer("InvincibilityLayer");
    }

    IEnumerator BackToOriginalColorAndUninvincibility()
    {
        isLayerShake = true;
        StartCoroutine("layerShake");
        yield return new WaitForSeconds(invincibilityTime);
        isLayerShake = false;
        render.color = originalColor;
        isReceiveDamage = true;
        player.layer = LayerMask.NameToLayer("Player");
    }

    IEnumerator layerShake()
    {
        while (isLayerShake)
        {
            if (render.color != Color.red)
            {
                render.color = Color.red;
            }
            else
            {
                render.color = Color.yellow;
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
}
