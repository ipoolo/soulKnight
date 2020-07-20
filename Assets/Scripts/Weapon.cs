using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private Animator animator;
    public RectTransform rectTransform;
    [SerializeField] public float damage;
    [SerializeField] public float attackInterval;
    private float attackTimer;
    private bool canAttack;
    private GameObject player;
    private WeaponPoint weaponPoint;
    


    public void Start()
    {
        animator = GetComponent<Animator>();
        rectTransform = GetComponent<RectTransform>();

        attackTimer = attackInterval;
        canAttack = true;

        weaponPoint = GameObject.FindGameObjectWithTag("WeaponPoint").GetComponent<WeaponPoint>();
        player = GameObject.FindGameObjectWithTag("Player");
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
    }

    public void attack()
    {
        if (canAttack) {
            if (animator != null)
            {
                animator.Play("fire",0,.0f);
                animator.SetBool("Fire", true);
                attackTimer = 0.0f;
                weaponPoint.pauseFollow();
            }
        }
        Debug.Log("a");
        
    }

    public void finishAnim()
    {
        weaponPoint.continueFollow();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {

        //武器触发伤害
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
