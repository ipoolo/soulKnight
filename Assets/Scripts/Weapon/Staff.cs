using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Staff : Weapon
{
    [SerializeField] public int reboundTimes;
    public float raycastDistance = 100.0f;

    public override void AttackBody()
    {
        base.AttackBody();
        Fire();
    }

    public string[] maskLayer = { "Wall", "Obstacle", "ObstacleWall" };
    public void Fire()
    {
        Vector2 direction = CalTargetDirection(firePoint.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.position);
        RaycastHit2D hit = Physics2D.Raycast(firePoint.transform.position, direction, raycastDistance, LayerMask.GetMask(maskLayer));
        if(hit.collider)
        {
            Raycast raycast = Instantiate((GameObject)Resources.Load("Bullet/Raycast/Raycast")).GetComponent<Raycast>();
            raycast.transform.position = firePoint.transform.position;
            raycast.transform.rotation = weaponPoint.transform.rotation;
            raycast.ConfigRaycast(hit.fraction * raycastDistance, damage, this,reboundTimes, maskLayer);
            if (reboundTimes>0) { 
                raycast.ReboundRaycast(hit, (1-hit.fraction) * raycastDistance, raycast.transform.rotation, direction);
            }
        }
    }

    
}
