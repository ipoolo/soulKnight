using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.iOS;
using UnityEngine;

public class WeaponPoint : MonoBehaviour
{
    public Weapon currWeapon;
    private bool isFollow;
    
    // Start is called before the first frame update
    void Start()
    {
        isFollow = true;
        GameObject weaponObject = Instantiate((GameObject)Resources.Load("Weapon/Slash"), transform.position, Quaternion.identity);
        currWeapon = weaponObject.GetComponent<Weapon>();
        currWeapon.transform.SetParent(transform);
        currWeapon.transform.position += new Vector3(currWeapon.rectTransform.sizeDelta.x / 2 * currWeapon.rectTransform.localScale.x, 0, 0);

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
        }
    }

    public void pauseFollow()
    {
        isFollow = false;
    }

    public void continueFollow()
    {
        isFollow = true;
    }
}
