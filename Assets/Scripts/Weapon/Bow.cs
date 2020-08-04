using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Bow : Weapon
    //改成基类
{
    [SerializeField] public Bullet bullet;
    [SerializeField] public GameObject firePoint;
    private int fireTimes;
    public int fireMaxTimes = 5;
    private float fireTimeStep = 0.1f;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void AttackBody()
    {
        base.AttackBody();
        StartCoroutine(Fire());

    }

    IEnumerator Fire()
    {
        while(fireTimes < fireMaxTimes) {
            if (!isStopFire) { 
                Bullet b = Instantiate(bullet, firePoint.transform.position, Quaternion.identity).GetComponent<Bullet>();
                b.targetDirection = CalTargetDirection(firePoint.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.position);
                b.damage = damage * (1 + powerBarValue);
                b.speed *= (1 + powerBarValue);
                b.castor = castor;
                b.fireWeapon = this;
                fireTimes++;
                if(fireTimes == fireMaxTimes)
                {
                    fireTimes = 0;
                    break;
                }
                yield return new WaitForSeconds(fireTimeStep);
            }
            else
            {
                break;
            }
        }
    }


}
