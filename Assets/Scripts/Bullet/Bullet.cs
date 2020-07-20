using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]public float Speed;
    [SerializeField]public int ejectionNum;
    [SerializeField]public float offsetRadius;
    [SerializeField]public float liftTime;
    [SerializeField]public bool isAutoFollow;
    [SerializeField]public float damage;
    [SerializeField]public float repelPower;
    [SerializeField]public float turnSpeed;

    public Rigidbody2D rigid2D;
    private RectTransform rectTransform;


    public GameObject targetObject;
    public Vector3 targetPosition;
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
        
        rigid2D.velocity = transform.right * Speed;
        rigid2D.gravityScale = 0.0f;

        Destroy(gameObject, liftTime);

    }

    // Update is called once per frame
    public void Update()
    {
        autoFollowTarget();

        rigid2D.velocity = transform.right * Speed;
    }

    public void autoFollowTarget()
    {
        if(isAutoFollow && targetObject != null)
        {
            if (targetPosition == Vector3.zero) { 
                targetPosition = targetObject.transform.position;
            }
            //自动跟踪
            Vector3 direction = targetPosition - transform.position;
            Quaternion changeQuaternion = Quaternion.FromToRotation(transform.right, direction);

            //方向 //速度
            if (changeQuaternion.eulerAngles.z > 1)
            {
                Quaternion targetQuaternion = changeQuaternion * transform.rotation;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetQuaternion, turnSpeed * 100.0f * Time.deltaTime);

            }

        }
    }
    
    private void PointToTarget()
    {
        if (targetPosition == Vector3.zero)
        {
            targetPosition = targetObject.transform.position;
        }
        //自动跟踪
        Vector3 direction = targetPosition - transform.position;
        Quaternion changeQuaternion = Quaternion.FromToRotation(transform.right, direction);


        Quaternion targetQuaternion = changeQuaternion * transform.rotation;
        transform.rotation = targetQuaternion;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider.CompareTag("Enemy"))
        {
            Enemy emeny = collision.collider.GetComponent<Enemy>();
            emeny.receiverDamageWithRepelVector(damage, transform.right.normalized * repelPower);
            
            Instantiate(Resources.Load("EnemyDeath"),transform.position,Quaternion.identity);
            Destroy(gameObject);
            //emeny.receiverDamage(damage, player.transform.position, 1.0f);
        }

    }

}
