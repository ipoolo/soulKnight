using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour
{
    public SpriteRenderer sr;
    public float damage;
    public Weapon fireWeapon;
    // Start is called before the first frame update

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        
    }
    public void ConfigRaycast(float direction,float _damage,Weapon _fireWeapon)
    {
        sr.size = new Vector2(direction, 1);
        damage = _damage;
        fireWeapon = _fireWeapon;
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


}
