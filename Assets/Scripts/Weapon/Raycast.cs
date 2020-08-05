using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour
{
    public SpriteRenderer sr;
    public float damage;
    public Weapon fireWeapon;
    public int reboundResidueTimes;
    private string[] maskLayer;
    // Start is called before the first frame update

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        
    }
    public void ConfigRaycast(float direction,float _damage,Weapon _fireWeapon,int _reboundResidueTimes, string[] _maskLayer)
    {
        sr.size = new Vector2(direction, 1);
        damage = _damage;
        fireWeapon = _fireWeapon;
        reboundResidueTimes = _reboundResidueTimes;
        maskLayer = _maskLayer;
    }

    // Update is called once per frame
    void Update()
    {
        //注释代码 代表 随玩家头像转动
        //Vector2 direction = fireWeapon.CalTargetDirection(fireWeapon.firePoint.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), fireWeapon.transform.position);
        //string[] maskLayer = { "Wall", "Obstacle", "ObstacleWall" };
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 100.0f, LayerMask.GetMask(maskLayer));
        //if (hit.collider)
        //{
        //    sr.size = new Vector2(Vector2.Distance(hit.point, fireWeapon.firePoint.transform.position), 1);
        //}
    }
    private void AnimationFinish()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy")|| other.CompareTag("Obstacle"))
        {
            NPC npc = other.GetComponentInChildren<NPC>();
            npc.ReceiveDamageWithRepelVector(damage,transform.right);
        }
    }

    public void ReboundRaycast(RaycastHit2D preHit, float residueDistance, Quaternion preRotation, Vector2 incomingVector)
    {
            //获得反射向量
            Vector2 reflectVector = Vector2.Reflect(incomingVector, preHit.normal);

            RaycastHit2D[] hits = Physics2D.RaycastAll(preHit.point, reflectVector, residueDistance, LayerMask.GetMask(maskLayer));
            if (hits.Length > 0) { 
                RaycastHit2D hit = hits[0];
                if (hits[0].fraction == 0)
                {
                    if (hits.Length >= 2)
                    {
                        hit = hits[1];
                    }
                }

                if (hit.collider)
                {
                    Raycast raycast = Instantiate((GameObject)Resources.Load("Bullet/Raycast/Raycast")).GetComponent<Raycast>();
                    raycast.transform.position = preHit.point;
                    raycast.transform.rotation = preRotation * Quaternion.FromToRotation(incomingVector, reflectVector);
                    float currDistance = hit.fraction * residueDistance;
                    raycast.ConfigRaycast(currDistance, damage, fireWeapon, reboundResidueTimes-1,maskLayer);
                    if (reboundResidueTimes > 0)
                    {
                        raycast.ReboundRaycast(hit, residueDistance * (1 - hit.fraction), raycast.transform.rotation, reflectVector);
                    }
                }
            }

    }


}
