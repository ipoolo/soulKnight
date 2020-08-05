using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class WeaponPoint : MonoBehaviour
{
    public Weapon currWeapon;
    public Weapon rangedWeapon;
    public Weapon closeinWeapon;
    public GameObject backUpGb;
    private bool isFollow;
    public float backUpRotaionRanged = 225.0f;
    // Start is called before the first frame update
    void Start()
    {
        isFollow = true;
        GameObject weaponObject = Instantiate((GameObject)Resources.Load("Weapon/Bow"), transform.position, Quaternion.identity);
        rangedWeapon = weaponObject.GetComponent<Weapon>();
        weaponObject = Instantiate((GameObject)Resources.Load("Weapon/Sword"), transform.position, Quaternion.identity);
        closeinWeapon = weaponObject.GetComponent<Weapon>();

        //currWeapon = rangedWeapon;
        //UpdateWeapon2BackupLocation(closeinWeapon);

        currWeapon = closeinWeapon;
        UpdateWeapon2BackupLocation(rangedWeapon);
    }

    // Update is called once per frame
    void Update()
    {
        followMouse();
    }

    private void followMouse()
    {
        if (isFollow) {
            Vector2 directionV = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

            float rotaion = Mathf.Atan2(directionV.y, directionV.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0,0,rotaion);

            if(currWeapon == closeinWeapon) { 
                currWeapon.ChangeWeaponDirection(transform.right);
            }
        }
    }

    public void pauseFollow()
    {
        isFollow = false;
    }

    public  void continueFollow()
    {
        isFollow = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Enemy>()|| other.GetComponent<Obstacle>()) {
            UpdateWeapon2BackupLocation(closeinWeapon);
        }

    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Enemy>() || other.GetComponent<Obstacle>())
        {
            UpdateWeapon2BackupLocation(rangedWeapon);
        }
    }
    private void UpdateWeapon2BackupLocation(Weapon nextWeapon)
    {
         ChangeWeaponWhenBackUp(currWeapon, nextWeapon);
    }

    private Weapon ChangeWeaponWhenBackUp(Weapon curr,Weapon next)
    {
        float rotaion = backUpRotaionRanged;
        if(curr.isCloseInWeapon)
        {
            rotaion -= 90.0f;
        }
        curr.InterruptStoragePower();
        if (curr.animator) { 
            curr.animator.enabled = false;
        }
        curr.transform.rotation = backUpGb.transform.rotation * Quaternion.Euler(0, 0, rotaion);
        curr.transform.position = transform.position;
        curr.sRender.sortingLayerName = "WeaponHide";
        curr.sRender.flipY = false;
        curr.sRender.flipX = false;
        curr.ChangeIsStopFire(true);
        curr.transform.parent = backUpGb.transform;
        if(next != null) { 
            next.sRender.sortingLayerName = "Weapon";

            if (next.animator)
            {
                next.animator.enabled = true;
            }
            next.transform.parent = transform;
            next.transform.position = transform.position;
            next.transform.rotation = transform.rotation;
            next.ChangeIsStopFire(false);
            currWeapon = next;
        }


        return curr;
    }

    private Weapon WeaponBackUp(Weapon backUpWeapon)
    {
        return ChangeWeaponWhenBackUp(backUpWeapon, null);
    }

    public void ChangeWeapon(Weapon nextWeapon)
    {
        Weapon pre;
        if (currWeapon.isCloseInWeapon == nextWeapon.isCloseInWeapon)
        {
             ChangeWeaponWhenBackUp(currWeapon, nextWeapon);
        }
        else
        {
            WeaponBackUp(nextWeapon);
        }

        if (nextWeapon.isCloseInWeapon)
        {
            pre = closeinWeapon;
            closeinWeapon = nextWeapon;
        }
        else
        {
            pre = rangedWeapon;
            rangedWeapon = nextWeapon;
        }
        
        
        ItemWeapon itemWeapon = Instantiate((GameObject)Resources.Load("Item/WeaponItem"), transform.parent.position + transform.right, Quaternion.identity).GetComponentInChildren<ItemWeapon>();
        itemWeapon.ConfigWeapon(pre);
    }
}
