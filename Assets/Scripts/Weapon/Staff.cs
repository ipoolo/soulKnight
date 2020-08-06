using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Staff : Weapon
{
    [SerializeField] public int reboundTimes = 0;
    public float raycastDistance = 100.0f;
    private Raycast currRaycast;
    public float raycastHoldTime;

    protected override void AttackBody()
    {
        base.AttackBody();
        Fire();
    }

    private string[] maskLayer = { "Wall", "Obstacle", "ObstacleWall" };
    public void Fire()
    {
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

        if (weaponType == EWeaponType.coutinue || weaponType == EWeaponType.storagePoweAndCoutinue)
        {
            raycast.transform.parent = weaponPoint.transform;
        }
        currRaycast = raycast;
    }

    protected override void ContinueUpdateBody()
    {

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



}
