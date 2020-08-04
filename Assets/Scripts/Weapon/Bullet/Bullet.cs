using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    [SerializeField]public float speed;
    [SerializeField]public int ejectionNum;
    [SerializeField]public float offsetRadius;
    [SerializeField]public float liftTime;
    [SerializeField]public bool isAutoFollow;
    [SerializeField]public float damage;
    [SerializeField]public float repelPower;
    [SerializeField]public float turnSpeed;
    [SerializeField]public float reboundTimes;
    public object castor;

    public Rigidbody2D rigid2D;
    private RectTransform rectTransform;


    public GameObject targetObject;
    public Vector3 targetDirection;
    //自动巡航用

    //加减速待完成

    // Start is called before the first frame update
    public void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        PointToTarget();

        //随机一个角度偏移
        transform.rotation *= Quaternion.Euler(0, 0, Random.Range(-offsetRadius, offsetRadius));
        //将子弹移出炮孔
        transform.position += new Vector3(rectTransform.rect.width * rectTransform.sizeDelta.x / 2, 0, 0);

        updateSpeed();
        rigid2D.gravityScale = 0.0f;

        Destroy(gameObject, liftTime);

    }

    // Update is called once per frame
    public void Update()
    {
        autoFollowTarget();
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
        if (other.CompareTag("Enemy"))
        {
            Enemy emeny = other.GetComponent<Enemy>();
            emeny.ReceiveDamageWithRepelVector(ExcuteHittingBuffEffect(damage), transform.right.normalized * repelPower);

            
            Instantiate(Resources.Load("EnemyDeath"),transform.position,Quaternion.identity);
            Destroy(gameObject);
            //emeny.receiverDamage(damage, player.transform.position, 1.0f);
        }

        string[] layerNames = { "Wall" ,"Item", "ObstructBullet" };
        if (selfCollider.IsTouchingLayers(LayerMask.GetMask(layerNames)))
        {
           
            if (reboundTimes > 0) {
                ReboundBody(_collision);
                reboundTimes--;
            }
            else
            {
                knockWallBody(_collision);
            }
            //emeny.receiverDamage(damage, player.transform.position, 1.0f);
        }

        string[] obstructBulletlayerNames = { "ObstructBullet" };
        //碰到虚拟墙
        if (selfCollider.IsTouchingLayers(LayerMask.GetMask(obstructBulletlayerNames)))
        {

            knockWallBody(_collision);
            //emeny.receiverDamage(damage, player.transform.position, 1.0f);
        }

    }


    private float ExcuteHittingBuffEffect(float _damage)
    {
        float tmp = _damage;
        if (castor is BuffReceiverInterFace)
        {
            List<Buff>  buffList= (castor as BuffReceiverInterFace).GetBuffList();
            foreach(Buff buff in buffList)
            {
                if (buff is BuffReceiveHittingDamageInterFace)
                {
                    tmp = ((BuffReceiveHittingDamageInterFace)buff).BuffReceiveHittingDamageInterFaceBody(tmp);
                }
            }
        }
        
        return tmp;
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

    public void knockWallBody(Collision2D _collision)
    {
            Instantiate(Resources.Load("EnemyDeath"),transform.position,Quaternion.identity);
            Destroy(gameObject);
    }

}
