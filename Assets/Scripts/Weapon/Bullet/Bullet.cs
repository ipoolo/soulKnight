﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    [SerializeField]public float speed;
    [SerializeField]public float offsetRadius;
    [SerializeField]public float liftTime;
    [SerializeField]public bool isAutoFollow;
    [HideInInspector]public float damage;
    [SerializeField]public float repelPower;
    [SerializeField]public float turnSpeed;
    [SerializeField]public float reboundTimes;
    public string collisionDestoryEffectName = "BulletBomb";
    [HideInInspector]public Weapon fireWeapon;
    public string voiceName;
    public float voiceTimeOffset;
    public object castor;

    [HideInInspector] private Rigidbody2D rigid2D;


    [HideInInspector] public GameObject targetObject;
    [HideInInspector] public Vector3 targetDirection;
    private BoxCollider2D collision2d;
    //自动巡航用

    //加减速待完成

    private void Awake()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        collision2d = GetComponent<BoxCollider2D>();
    }
    // Start is called before the first frame update
    public void Start()
    {

        PointToTarget();
        collision2d.
        //随机一个角度偏移
        transform.rotation *= Quaternion.Euler(0, 0, Random.Range(-offsetRadius, offsetRadius));
        //将子弹移出炮孔

        transform.position += new Vector3(collision2d.size.x * transform.lossyScale.x / 2, 0, 0);

        updateSpeed();
        rigid2D.gravityScale = 0.0f;

        Destroy(gameObject, liftTime);

    }

    // Update is called once per frame
    public void Update()
    {
        autoFollowTarget();
    }

    public void configFireWeapon(Weapon _fireWeapon)
    {
        fireWeapon = _fireWeapon;
    }

    public void autoFollowTarget()
    {
        if(isAutoFollow && targetObject != null)
        {
            if (targetDirection == Vector3.zero) {
                targetDirection = targetObject.transform.position - transform.position;
            }
            //自动跟踪
            Vector3 direction = targetDirection;
            Quaternion changeQuaternion = Quaternion.FromToRotation(transform.right, direction);

            //方向 //速度
            if (changeQuaternion.eulerAngles.z > 1)
            {
                Quaternion targetQuaternion = changeQuaternion * transform.rotation;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetQuaternion, turnSpeed * 100.0f * Time.deltaTime);

            }

        }
        updateSpeed();
    }
    
    private void PointToTarget()
    {
        if (targetDirection == Vector3.zero)
        {
            targetDirection = targetObject.transform.position - transform.position;
        }
        //跟踪
        Vector3 direction = targetDirection;
        Quaternion changeQuaternion = Quaternion.FromToRotation(transform.right, direction);


        Quaternion targetQuaternion = changeQuaternion * transform.rotation;
        transform.rotation = targetQuaternion;
    }

    private void OnCollisionEnter2D(Collision2D _collision)
    {

        Collider2D other = _collision.collider;
        Collider2D selfCollider = _collision.otherCollider;

        if (other.CompareTag("Enemy")|| other.CompareTag("Obstacle") )
        {
            AudioManager.Instance.PlaySoundBulletWithTime("Voices/" + voiceName, voiceTimeOffset);
            NPC npc = other.GetComponent<NPC>();
            npc.ReceiveDamageWithRepelVector(fireWeapon.ExcuteHittingBuffEffect(damage), transform.right.normalized * repelPower);
            Instantiate(Resources.Load(collisionDestoryEffectName),transform.position,Quaternion.identity);
            Destroy(gameObject);
        }

        string[] wallLayerNames = { "Wall" ,"Item" };
        if (selfCollider.IsTouchingLayers(LayerMask.GetMask(wallLayerNames)))
        {
            if (reboundTimes > 0) {
                ReboundBody(_collision);
                reboundTimes--;
            }
            else
            {
                KnockWallBody(_collision);
            }
            //emeny.receiverDamage(damage, player.transform.position, 1.0f);
        }

        string[] obstructBulletlayerNames = { "ObstacleWall" };
        //碰到虚拟墙
        if (selfCollider.IsTouchingLayers(LayerMask.GetMask(obstructBulletlayerNames)))
        {

            KnockWallBody(_collision);
        }

    }



    public void ReboundBody(Collision2D _collision)
    {
        Vector3 reflectPosition;
        Vector3 reflectVector;
        ContactPoint2D contactPoin2D= _collision.contacts[0];
        //获得反射向量
        reflectVector = Vector2.Reflect(transform.right, contactPoin2D.normal);
        //获得旋转角度
        Quaternion rotation = Quaternion.FromToRotation(transform.right, reflectVector);
        //计算反射后position

        float scaleX = Mathf.Abs(contactPoin2D.point.x - transform.position.x);
        float scaleY = Mathf.Abs(contactPoin2D.point.y - transform.position.y);
        //旋转
        transform.rotation *= rotation;
        //移动postion
        reflectPosition = new Vector3(contactPoin2D.point.x + reflectVector.normalized.x * scaleX,
            contactPoin2D.point.y + reflectVector.normalized.y * scaleY, 0);
        transform.position = reflectPosition;
        updateSpeed();


    }
    public void updateSpeed()
    {
        updateSpeed(speed);
    }

    public void updateSpeed(float _speed)
    {
        speed = _speed;
        rigid2D.velocity = transform.right * speed;
    }

    protected virtual void KnockWallBody(Collision2D _collision)
    {
            ContactPoint2D cp = _collision.contacts[0];
            Vector2 ImpactRight = cp.normal * -1;
            GameObject bullet = Instantiate((GameObject)Resources.Load(collisionDestoryEffectName),transform.position,Quaternion.FromToRotation(Vector2.right, ImpactRight));
            AudioManager.Instance.PlaySoundBulletWithTime("Voices/" + voiceName, voiceTimeOffset);

        Destroy(gameObject);
    }

}
