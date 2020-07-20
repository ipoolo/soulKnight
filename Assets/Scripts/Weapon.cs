using System.Collections;
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
    private bool canAttack;
    private GameObject player;
    private WeaponPoint weaponPoint;

    public bool isStoragePowerWeapon;
    private bool isStoragePower;
    private PowerController powerController;

    


    public void Start()
    {
        animator = GetComponent<Animator>();
        rectTransform = GetComponent<RectTransform>();

        attackTimer = attackInterval;
        canAttack = true;

        weaponPoint = GameObject.FindGameObjectWithTag("WeaponPoint").GetComponent<WeaponPoint>();
        player = GameObject.FindGameObjectWithTag("Player");

        powerController = GameObject.FindGameObjectWithTag("UI_PowerBar").GetComponent<PowerController>();
    }

    public void Update()
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

        if (isStoragePower) {
            storagePowersustainTime += Time.deltaTime;
            if (powerMaxSecond != 0) { 
                float tempPowerValue = storagePowersustainTime / powerMaxSecond;
                float powerBarValue = tempPowerValue > powerMaxSecond ? powerMaxSecond : tempPowerValue;
                powerController.updatePowerBarValue(powerBarValue);
            }
        }
                    
    }

    public void Attack()
    {
        if (canAttack) {
            if (animator != null)
            {
                animator.Play("fire",0,.0f);
                animator.SetBool("Fire", true);
                attackTimer = 0.0f;
                weaponPoint.pauseFollow();
            }

            //powerBar处理
            isStoragePower = false;
            powerController.hidePowerBar();
        }
        Debug.Log("a");
        
    }

    public void finishAnim()
    {
        weaponPoint.continueFollow();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (isCloseInWeapon) { 
        //近战武器触发伤害
            if (other.CompareTag("Enemy")) { 
                //{
                Enemy emeny = other.GetComponent<Enemy>();
                Transform temp = GameObject.FindGameObjectWithTag("WeaponPoint").GetComponent<Transform>();
                float zRotaion = temp.rotation.eulerAngles.z;
                Vector3 tempV =  Quaternion.AngleAxis(zRotaion, Vector3.forward) * Vector3.right;
                Debug.Log(zRotaion + "zRotaion||||"+tempV + "tempV");
                emeny.receiverDamageWithRepelVector(damage, tempV);
                //emeny.receiverDamage(damage, player.transform.position, 1.0f);
            }
        }
    }

    //StoragePower
    public void StoragePower()
    {
        Debug.Log("wp_StoragePower");
        storagePowersustainTime = 0.0f;
        isStoragePower = true;
        powerController.showPowerBar();
    }
}
