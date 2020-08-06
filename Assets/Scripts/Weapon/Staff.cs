using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Staff : Weapon
{
    [SerializeField] public int reboundTimes = 0;
    public float raycastDistance = 100.0f;

    public override void AttackBody()
    {
        base.AttackBody();
        Fire();
    }

    private string[] maskLayer = { "Wall", "Obstacle", "ObstacleWall" };
    public void Fire()
    {
        Vector2 direction = CalTargetDirection(firePoint.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.position);
        RaycastHit2D hit = Physics2D.Raycast(firePoint.transform.position, direction, raycastDistance, LayerMask.GetMask(maskLayer));
        if(hit.collider)
        {
            Raycast raycast = Instantiate((GameObject)Resources.Load("Bullet/Raycast/Raycast")).GetComponent<Raycast>();
            raycast.ConfigRaycast(hit.fraction * raycastDistance, this, reboundTimes, firePoint.transform.position,weaponPoint.transform.rotation, maskLayer);
            if (raycast.residueTimes > 0) {
                raycast.ReboundRaycast(hit, (1-hit.fraction) * raycastDistance, direction);
            }
        }
    }

    
}
