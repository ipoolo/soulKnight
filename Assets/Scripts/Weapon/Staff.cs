using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Staff : Weapon
{
    [SerializeField] public float reboundTimes = 0;
    private float reboundTimer = 0;
    public float raycastDistance = 100.0f;

    public override void AttackBody()
    {
        base.AttackBody();
        Fire();
    }

    private string[] maskLayer = { "Wall", "Obstacle", "ObstacleWall" };
    public void Fire()
    {
        reboundTimer = reboundTimes;
        Vector2 direction = CalTargetDirection(firePoint.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.position);
        RaycastHit2D hit = Physics2D.Raycast(firePoint.transform.position, direction, raycastDistance, LayerMask.GetMask(maskLayer));
        if(hit.collider)
        {
            Raycast raycast = Instantiate((GameObject)Resources.Load("Bullet/Raycast/Raycast")).GetComponent<Raycast>();
            raycast.transform.position = firePoint.transform.position;
            raycast.transform.rotation = weaponPoint.transform.rotation;
            raycast.ConfigRaycast(hit.fraction * raycastDistance, damage, this);
            ReboundRaycast(hit, (1-hit.fraction) * raycastDistance, raycast.transform.rotation, direction);
        }
    }

    protected void ReboundRaycast(RaycastHit2D preHit,float residueDistance, quaternion preRotation,Vector2 incomingVector)
    {

        if (reboundTimer > 0)
        {
            Vector2 firePointPosition = firePoint.transform.position;
            //获得反射向量
            Vector2 reflectVector = Vector2.Reflect(incomingVector, preHit.normal);
           
            RaycastHit2D[] hits  = Physics2D.RaycastAll(preHit.point, reflectVector, residueDistance, LayerMask.GetMask(maskLayer));
            RaycastHit2D hit = hits[0];
            if (hits[0].fraction == 0)
            {
                if(hits.Length >= 2) { 
                    hit = hits[1];
                }
            }
            
            if (hit.collider)
            {
                Raycast raycast = Instantiate((GameObject)Resources.Load("Bullet/Raycast/Raycast")).GetComponent<Raycast>();
                raycast.transform.position = preHit.point;
                raycast.transform.rotation = preRotation * Quaternion.FromToRotation(incomingVector, reflectVector);
                float currDistance = hit.fraction * residueDistance;
                raycast.ConfigRaycast(currDistance, damage, this);
                reboundTimer--;
                ReboundRaycast(hit, residueDistance *(1- hit.fraction), raycast.transform.rotation, reflectVector);
            }


        }

    }
}
