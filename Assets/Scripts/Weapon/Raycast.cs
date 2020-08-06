using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour
{
    public SpriteRenderer sr;
    public float damage;
    public Weapon fireWeapon;
    public int residueTimes;
    public string[] maskLayer;
    // Start is called before the first frame update

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        
    }
    public void ConfigRaycast(float distance,Weapon _fireWeapon,int _residueTimes , Vector2 _position , Quaternion _rotation,string[] _maskLayer)
    {
        sr.size = new Vector2(distance, sr.size.y);
        residueTimes = _residueTimes;
        damage = _fireWeapon.damage;
        fireWeapon = _fireWeapon;
        transform.position = _position;
        transform.rotation = _rotation;
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

    public void ReboundRaycast(RaycastHit2D preHit, float residueDistance, Vector2 incomingVector)
    {

        //获得反射向量
        Vector2 reflectVector = Vector2.Reflect(incomingVector, preHit.normal);
        Vector2 startPoint = preHit.point - incomingVector.normalized * sr.size.y * 0.5f - reflectVector.normalized * sr.size.y * 0.5f + reflectVector.normalized*0.0001f;

        RaycastHit2D hit = Physics2D.Raycast(startPoint, reflectVector, residueDistance, LayerMask.GetMask(maskLayer));
        Color color = Color.red;

        if(residueTimes%2 == 0) {
            color = Color.yellow;
        }
        else
        {
            color = Color.blue;
        }

        float currDistance;
        if (hit.collider)
        {
            currDistance = hit.fraction * residueDistance;
        }
        else
        {
            currDistance = residueDistance;
        }

        Raycast raycast = Instantiate((GameObject)Resources.Load("Bullet/Raycast/Raycast")).GetComponent<Raycast>();
        Quaternion newRotation = Quaternion.FromToRotation(Vector2.right, reflectVector);
        raycast.ConfigRaycast(currDistance, fireWeapon, residueTimes-1, startPoint, newRotation, maskLayer);
        if (raycast.residueTimes > 0)
        {
            raycast.ReboundRaycast(hit, residueDistance - currDistance, reflectVector);
        }
           
        


    }

}


