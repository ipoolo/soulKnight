using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWeapon : Item
{
    public Weapon weapon;
    // Start is called before the first frame update
    private new void Awake()
    {
        base.Awake();
        weapon = Instantiate((GameObject)Resources.Load("Weapon/GodSword"), transform.parent.position, Quaternion.identity).GetComponent<Weapon>();
        weapon.transform.parent = transform.parent;
        interactionBodyAction = new System.Action(ChangeWeapon);
        playerEnterAction = new System.Action(ShowWeaponInfo);
        playerExitAction = new System.Action(HideWeaponInfo);
    }

    new void Start()
    {

        //Test 
        base.Start();

    }

    public void ConfigWeapon(Weapon _weapon)
    {
        if (weapon != null) {
            Destroy(weapon.gameObject);
        }
            weapon = _weapon;
            weapon.transform.position = transform.parent.position;
            weapon.transform.parent = transform.parent;
            weapon.transform.rotation = transform.rotation;


    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }

    public void ChangeWeapon()
    {
        if(weapon != null) { 
            weapon.weaponPoint.ChangeWeapon(weapon);
        }
        Destroy(transform.parent.gameObject);

    }

    public void ShowWeaponInfo()
    {

    }

    public void HideWeaponInfo()
    {

    }
}
