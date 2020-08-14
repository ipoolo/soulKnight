using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon
{
    [SerializeField] public string bulletName;



    private int fireTimes;
    public int fireMaxTimes = 3;
    private float fireTimeStep = 0.1f;
    public string muzzleFlashName;
    private new void Awake()
    {
        base.Awake();
    }

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

    protected override void AttackBody()
    {
        base.AttackBody();
        StartCoroutine(Fire());

    }

    IEnumerator Fire()
    {
        while (fireTimes < fireMaxTimes)
        {
            if (!isStopFire)
            {
                animator.SetTrigger("GunFire");


                Vector3 tmp = player.transform.position - firePoint.transform.position;
                impulseSource.GenerateImpulse(tmp.normalized * impulseScale);

                GameObject gb = Instantiate((GameObject)Resources.Load("Weapon/ShellBullet/GunShellBullet"));
                gb.transform.parent = GameObject.FindGameObjectWithTag("Shells").transform;
                gb.transform.position = firePoint.transform.position;

                GameObject muzzleFlash = Instantiate((GameObject)Resources.Load("Weapon/MuzzleFlash/"+ muzzleFlashName),firePoint.transform.position,weaponPoint.transform.rotation);
                muzzleFlash.transform.parent = transform;

                //给玩家反冲
                player.playerStateController.ReceiveDamageWithRepelVector(0,(player.transform.position - firePoint.transform.position).normalized * 0.1f);

                Bullet b = Instantiate((GameObject)Resources.Load("Bullet/"+bulletName), firePoint.transform.position, Quaternion.identity).GetComponent<Bullet>();
                b.targetDirection = CalTargetDirection(firePoint.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.position);
                b.damage = damage * (1 + powerBarValue);
                b.speed *= (1 + powerBarValue);
                b.castor = castor;
                b.fireWeapon = this;
                fireTimes++;
                if (fireTimes == fireMaxTimes)
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
