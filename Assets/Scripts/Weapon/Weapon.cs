﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private Animator animator;
    public RectTransform rectTransform;
    [SerializeField] public float damage;
    [SerializeField] public float attackInterval;
    [SerializeField] public bool isCloseInWeapon;

    [SerializeField] public float powerMaxSecond;
    
    private float storagePowersustainTime;

    private float attackTimer;
    [HideInInspector]  public bool canAttack;
    private GameObject player;
    private WeaponPoint weaponPoint;

    public bool isStoragePowerWeapon;
    public bool isStoragePower;
    private PowerController powerController;
    public float powerBarValue;




    public virtual void Start()
    {
        animator = GetComponent<Animator>();
        rectTransform = GetComponent<RectTransform>();

        attackTimer = attackInterval;
        canAttack = true;

        weaponPoint = GameObject.FindGameObjectWithTag("WeaponPoint").GetComponent<WeaponPoint>();
        player = GameObject.FindGameObjectWithTag("Player");

        powerController = GameObject.FindGameObjectWithTag("UI_PowerBar").GetComponent<PowerController>();
    }

    public virtual void Update()
    {
        if (attackTimer >= attackInterval)
        {
            canAttack = true;
        }
        else
        {
            canAttack = false;
        }
        attackTimer += Time.deltaTime;

        if (isStoragePower)
        {
            storagePowersustainTime += Time.deltaTime;
            if (powerMaxSecond != 0)
            {
                float tempPowerValue = storagePowersustainTime / powerMaxSecond;
                powerBarValue = tempPowerValue > 1 ? 1 : tempPowerValue;
                powerController.updatePowerBarValue(powerBarValue);
            }
        }

    }

    public void Attack()
    {
        if (canAttack)
        {
            if (animator != null)
            {
                animator.Play("fire", 0, .0f);
                animator.SetBool("Fire", true);
                weaponPoint.pauseFollow();
            }

            if (isStoragePowerWeapon)
            {
                if (isStoragePower) { 
                    AttackBody();
                }
            }
            else
            {
                AttackBody();
            }
            //powerBar处理
            isStoragePower = false;
            powerController.hidePowerBar();
        }
        Debug.Log("a");

    }

    public virtual void AttackBody()
    {
        attackTimer = 0.0f;
    }

    public void finishAnim()
    {
        weaponPoint.continueFollow();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (isCloseInWeapon)
        {
            //近战武器触发伤害
            if (other.CompareTag("Enemy"))
            {
                //{
                Enemy emeny = other.GetComponent<Enemy>();
                Transform temp = GameObject.FindGameObjectWithTag("WeaponPoint").GetComponent<Transform>();
                float zRotaion = temp.rotation.eulerAngles.z;
                Vector3 tempV = Quaternion.AngleAxis(zRotaion, Vector3.forward) * Vector3.right;
                Debug.Log(zRotaion + "zRotaion||||" + tempV + "tempV");
                emeny.receiverDamageWithRepelVector(damage, tempV);
                //emeny.receiverDamage(damage, player.transform.position, 1.0f);
            }
        }
    }

    //StoragePower
    public void StoragePower()
    {
        if (canAttack)
        {
            storagePowersustainTime = 0.0f;
            isStoragePower = true;
            powerController.showPowerBar();
        }
    }

    //检查mouse 小于武器出口时翻转向量
    public Vector3 CalTargetDirection(Vector3 _firePointPosition, Vector3 _mousePosition, Vector3 _transformPosition)
    {
        _mousePosition = new Vector3(_mousePosition.x, _mousePosition.y,0);
        Vector3 targetDirection = Vector3.zero;
        Vector3 vectorfireP2MouseP = _mousePosition - _firePointPosition;
        targetDirection = vectorfireP2MouseP;
        Vector3 vectortransformP2fireP = _firePointPosition - _transformPosition;
        Debug.Log("Vector3.Dot(vectorfireP2MouseP, vectortransformP2fireP)!!"+ Vector3.Dot(vectorfireP2MouseP, vectortransformP2fireP));
        if (Vector3.Dot(vectorfireP2MouseP, vectortransformP2fireP) < 0)
        //小于零说明不同向
        {
            targetDirection = Quaternion.Euler(0, 0, 180) * vectorfireP2MouseP;
        }

        Debug.Log("targetDirection" + targetDirection);
        return targetDirection;
    }
}