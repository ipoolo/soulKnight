using System.Collections.Generic;
using UnityEngine;

public class Staff : Weapon
{
    [SerializeField] public int reboundTimes = 0;
    public float raycastDistance = 100.0f;
    private Raycast currRaycast;
    public float raycastHoldTime;
    private List<PS_RayAbsorb> currPsList = new List<PS_RayAbsorb>();
    

    protected override void AttackBody()
    {
        base.AttackBody();
        RemovePs();
        Fire();
    }

    private float pastPersent;
    protected override void StoragePowerOnceBody()
    {
        base.StoragePowerOnceBody();
        pastPersent = 0.0f;
        currPsList = new List<PS_RayAbsorb>();
    }

    protected override void StoragePowerUpdateBody(float persent)
    {
        if(persent - pastPersent >= 0.195f)
        {
            pastPersent = persent;
            SpwanPS();
            MiniShakeCamera();
        }
        base.StoragePowerUpdateBody(persent);
    }

    protected void SpwanPS()
    {
        base.StoragePowerOnceBody();
        GameObject gb = Instantiate((GameObject)Resources.Load("Partcles/PS_RayAbsorb"));
        gb.transform.parent = player.transform;
        gb.transform.position = player.transform.position;
        PS_RayAbsorb ps = gb.GetComponent<PS_RayAbsorb>();
        var main = ps.GetComponent<ParticleSystem>().main;
        main.stopAction = ParticleSystemStopAction.Callback;
        currPsList.Add(ps);
    }

    protected void RemovePs()
    {
        currPsList.ForEach(ps =>
        {
            var main = ps.GetComponent<ParticleSystem>().main;
            main.loop = false;
            Destroy(ps.gameObject, main.duration);
        });

    }

    protected override void InterruptStoragePowerBody()
    {
        base.InterruptStoragePowerBody();
        RemovePs();
    }

    private string[] maskLayer = { "Wall", "Obstacle", "ObstacleWall" };
    public void Fire()
    {
        ShakeCamera();
        shakeCameraStepTimer = 0.0f;

        Vector2 direction = CalTargetDirection(firePoint.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.position);
        RaycastHit2D hit = Physics2D.Raycast(firePoint.transform.position, direction, raycastDistance, LayerMask.GetMask(maskLayer));
        float currRaycastDistance = 0;
        if (hit.collider)
        {
            currRaycastDistance = hit.fraction * raycastDistance;
        }
        else
        {
            currRaycastDistance = raycastDistance;
        }
        Raycast raycast = Instantiate((GameObject)Resources.Load("Bullet/Raycast/Raycast")).GetComponent<Raycast>();
        raycast.ConfigRaycast(currRaycastDistance, this, reboundTimes, firePoint.transform.position,weaponPoint.transform.rotation, maskLayer, raycastHoldTime);
        if (raycast.residueTimes > 0 && hit.collider) {
            raycast.ReboundRaycast(hit, (1-hit.fraction) * raycastDistance, direction);
        }

        if (weaponType == EWeaponType.coutinue || weaponType == EWeaponType.storagePowerAndCoutinue)
        {
            raycast.transform.parent = weaponPoint.transform;
        }
        currRaycast = raycast;
    }

    //震动屏幕其实可以抽象到更高层

    protected override void ContinueUpdateBody()
    {

        shakeCameraStepTimer += Time.deltaTime;
        if(shakeCameraStepTimer > shakeCameraStepTime)
        {
            ShakeCamera();
            shakeCameraStepTimer = 0.0f;
        }


        base.ContinueUpdateBody();
        Vector2 direction = CalTargetDirection(firePoint.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.position);
        RaycastHit2D hit = Physics2D.Raycast(firePoint.transform.position, direction, this.raycastDistance, LayerMask.GetMask(maskLayer));
        float firstRaycastDistance = 0;
        if (hit.collider)
        {
            firstRaycastDistance = hit.fraction * raycastDistance;
        }
        else
        {
            firstRaycastDistance = raycastDistance;
        }
        currRaycast.updateRaycastAndSubRaycast(firstRaycastDistance, firePoint.transform.position, weaponPoint.transform.rotation, hit, raycastDistance);
    }

    protected override void ContinueFinishBody()
    {
        base.ContinueFinishBody();
        if (currRaycast) { 
            currRaycast.RaycastChinaAnimation2Exit();
        }
    }

    protected void ShakeCamera()
    {
       
        Vector3 tmp = player.transform.position - firePoint.transform.position;
        tmp += new Vector3(Random.Range(-1, 1), Random.Range(-1, 1));
        impulseSource.GenerateImpulse(tmp.normalized * impulseScale);
    }

    protected void MiniShakeCamera()
    {

        Vector3 tmp = player.transform.position - firePoint.transform.position;
        tmp += new Vector3(Random.Range(-1, 1), Random.Range(-1, 1));
        impulseSource.GenerateImpulse(tmp.normalized * impulseScale * 0.3f);
    }



}
