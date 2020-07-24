using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Bow : Weapon
{
    [SerializeField] public Bullet bullet;
    [SerializeField] public GameObject firePoint;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void AttackBody()
    {
        base.AttackBody();
        Bullet b = Instantiate(bullet, firePoint.transform.position, Quaternion.identity).GetComponent<Bullet>();
        //Input.mousePosition

        //限制mousePosition 
        b.targetDirection = CalTargetDirection(firePoint.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.position);
        b.damage = damage * (1 + powerBarValue);
        b.speed *= (1 + powerBarValue);
        b.castor = castor;


    }


}
