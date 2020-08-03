﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.iOS;
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
        rangedWeapon.transform.SetParent(transform);
        //if (weaponObject.GetComponent<Weapon>().isCloseInWeapon) {
        //    currWeapon.transform.position += new Vector3(currWeapon.rectTransform.sizeDelta.x / 2 * currWeapon.rectTransform.localScale.x, 0, 0);
        //}

        weaponObject = Instantiate((GameObject)Resources.Load("Weapon/Sword"), transform.position, Quaternion.identity);
        closeinWeapon = weaponObject.GetComponent<Weapon>();
        closeinWeapon.transform.SetParent(transform);
        //closeinWeapon.transform.position += new Vector3(closeinWeapon.rectTransform.sizeDelta.x / 2 * closeinWeapon.rectTransform.localScale.x, 0, 0);

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

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.GetComponent<Enemy>()) { 
            UpdateWeapon2BackupLocation(closeinWeapon);
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Enemy>())
        {
            UpdateWeapon2BackupLocation(rangedWeapon);
        }
    }
    private void UpdateWeapon2BackupLocation(Weapon nextWeapon)
    {
         ChangeWeaponWhenBackUp(currWeapon, nextWeapon, backUpRotaionRanged);
    }

    private void ChangeWeaponWhenBackUp(Weapon curr,Weapon next,float backUpRotaion)
    {
        float rotaion = backUpRotaion;
        if(curr == closeinWeapon)
        {
            rotaion -= 90.0f;
        }
        curr.transform.rotation = Quaternion.Euler(0, 0, rotaion);
        curr.sRender.sortingLayerName = "WeaponHide";
        curr.transform.parent = backUpGb.transform;
        next.sRender.sortingLayerName = "Weapon";
        next.transform.parent = transform;
        next.transform.rotation = transform.rotation;
        currWeapon = next;
    }

}
