using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float moveSpeed;


    private Rigidbody2D rid2d;
  
    private Animator animator;
    private WeaponPoint weaponPoint;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        rid2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        weaponPoint = GetComponentInChildren<WeaponPoint>();
    }

    // Update is called once per frame
    void Update()
    {
        checkAttack();
        checkToward();
    }

    private void FixedUpdate()
    {
        playerMove();
        
    }

    void playerMove()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        rid2d.velocity = new Vector2( moveX * moveSpeed , moveY * moveSpeed);

        if(rid2d.velocity.sqrMagnitude > 0.2f)
        {
            animator.SetBool("Run", true);
        }
        else
        {
            animator.SetBool("Run", false);
        }
    }

    void checkAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //attack
            if (!weaponPoint.currWeapon.isStoragePowerWeapon) { 
                weaponPoint.currWeapon.Attack();
            }
            else
            {
                //StoragePower
                weaponPoint.currWeapon.StoragePower();
            }

        }
        else if(Input.GetMouseButtonUp(0))
        {
            if (weaponPoint.currWeapon.isStoragePowerWeapon && weaponPoint.currWeapon.isStoragePower)
            {
                weaponPoint.currWeapon.Attack();
            }
        }

    }

    void checkToward()
    {
        
        if(Camera.main.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0,0,0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180.0f, 0);
        }
    }
}
