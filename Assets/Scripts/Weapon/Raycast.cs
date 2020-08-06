using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour
{
    [HideInInspector] public SpriteRenderer sr;
    [HideInInspector] public float damage;
    [HideInInspector] public Weapon fireWeapon;
    [HideInInspector]public int residueTimes;
    [HideInInspector] public string[] maskLayer;
    [HideInInspector] public Raycast nextRaycast;
    [HideInInspector] public Animator animator;
    public float raycastHoldTime = 0.2f;
    public float damageIntervalTime;
    private NpcTimer npcTimer;
    // Start is called before the first frame update

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        npcTimer = this.gameObject.AddComponent<NpcTimer>().ConfingIntervalTime(damageIntervalTime,damageIntervalTime - 0.01f);
        //立刻触发一次伤害（因为离开碰撞体并不remove掉对象，这样对象短时间反复进入不会触发伤害ps:移除了 enter部分伤害代码）
    }

    void Start()
    {

    }
    public void ConfigRaycast(float distance,Weapon _fireWeapon,int _residueTimes , Vector2 _position , Quaternion _rotation,string[] _maskLayer,float _raycastHoldTime)
    {
        sr.size = new Vector2(distance, sr.size.y);
        residueTimes = _residueTimes;
        damage = _fireWeapon.damage;
        fireWeapon = _fireWeapon;
        transform.position = _position;
        transform.rotation = _rotation;
        maskLayer = _maskLayer;
        raycastHoldTime = _raycastHoldTime;
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

    private void AnimationFireFinish(string str)
    {
        Debug.Log("AnimationFireFinish" + str);
        switch (fireWeapon.weaponType)
        {
            case EWeaponType.normal:
                animator.SetTrigger("Hold");
                StartCoroutine(AnimationTurn2Finish());
                break;
            case EWeaponType.storagePower:
                animator.SetTrigger("Hold");
                StartCoroutine(AnimationTurn2Finish());
                break;
            default:
                animator.SetTrigger("Hold");
                break;
        }
    }

    IEnumerator AnimationTurn2Finish()
    {
        yield return new WaitForSeconds(raycastHoldTime);
        animator.SetTrigger("Exit");
    }

    private void AnimationHoldFinish(string str)
    {
        Debug.Log("AnimationHoldFinish" + str);
        switch (fireWeapon.weaponType)
        {
            case EWeaponType.normal:

                break;
            default:
                break;
        }
    }

    public void RaycastChinaAnimation2Exit()
    {
        if (nextRaycast) { 
            nextRaycast.RaycastChinaAnimation2Exit();
        }
        animator.SetTrigger("Exit");
    }

    protected void AnimationExitFinish()
    {
        Destroy(this.gameObject);
    }

    public void ReboundRaycast(RaycastHit2D preHit, float residueDistance, Vector2 incomingVector)
    {

        //获得反射向量
        Vector2 reflectVector = Vector2.Reflect(incomingVector, preHit.normal);
        Vector2 startPoint = preHit.point - incomingVector.normalized * sr.size.y * 0.5f - reflectVector.normalized * sr.size.y * 0.5f + reflectVector.normalized*0.0001f;

        RaycastHit2D hit = Physics2D.Raycast(startPoint, reflectVector, residueDistance, LayerMask.GetMask(maskLayer));


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
        raycast.ConfigRaycast(currDistance, fireWeapon, residueTimes-1, startPoint, newRotation, maskLayer, raycastHoldTime);
        nextRaycast = raycast;
        float nextRaycastDistance = residueDistance - currDistance;
        if (nextRaycast.residueTimes > 0 && nextRaycastDistance >0)
        {
            nextRaycast.ReboundRaycast(hit, nextRaycastDistance, reflectVector);
        }
    }

    public void updateRaycastAndSubRaycast(float currDistance,Vector2 startPoint, Quaternion currRotation, RaycastHit2D hit,float residueDistance)
    {

        Debug.Log("currDistance");
        Debug.Log("residueTimes"+ residueTimes);
        ConfigRaycast(currDistance, fireWeapon, residueTimes, startPoint, currRotation, maskLayer, raycastHoldTime);
        float residueNextDistance = residueDistance - currDistance;
        if (residueTimes > 0 && residueNextDistance > 0) { 
            //计算下条的数据信息
            Vector2 incomingVector = hit.point - startPoint;
            Vector2 reflectVector = Vector2.Reflect(incomingVector, hit.normal);
            Vector2 NextstartPoint = hit.point - incomingVector.normalized * sr.size.y * 0.5f - reflectVector.normalized * sr.size.y * 0.5f + reflectVector.normalized * 0.0001f;

                
            float nextDistance;

            RaycastHit2D nextHit = Physics2D.Raycast(NextstartPoint, reflectVector, residueNextDistance, LayerMask.GetMask(maskLayer));
            if (nextHit.collider)
            {
                nextDistance = nextHit.fraction * residueNextDistance;
            }
            else
            {
                nextDistance = residueNextDistance;
            }
            Quaternion nextRotation = Quaternion.FromToRotation(Vector2.right, reflectVector);
            if(nextRaycast != null ) {
                Debug.Log("updateRaycastAndSubRaycast ");
                nextRaycast.updateRaycastAndSubRaycast(nextDistance, NextstartPoint, nextRotation, nextHit, residueNextDistance);
            }
            else if(nextRaycast == null )
            {
                Debug.Log("updateRaycastAndSubRaycast _N");
                //生成
                ReboundRaycast(nextHit, residueNextDistance, incomingVector);
                nextRaycast.updateRaycastAndSubRaycast(nextDistance, NextstartPoint, nextRotation, nextHit, residueNextDistance);
            }
        }
        else
        {
            //没有距离/没有反弹数量了 但是还有链路，则销毁链路
            if(nextRaycast != null)
            {
                nextRaycast.DestroyRaycastChain();
            }
        }
        


    }


    //链销毁
    protected void DestroyRaycastChain()
    {
        if (nextRaycast)
        {
            nextRaycast.DestroyRaycastChain();
        }
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Obstacle"))
        {
            NPC npc = other.GetComponentInChildren<NPC>();
            npcTimer.addNpc(npc);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Obstacle"))
        {
            NPC npc = other.GetComponentInChildren<NPC>();

            if (npcTimer.CheckTimerForEffect(npc))
            {
                DamageEffectBody(npc);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //if (other.CompareTag("Enemy") || other.CompareTag("Obstacle"))
        //{
        //    NPC npc = other.GetComponentInChildren<NPC>();

        //    npcTimer.removeNpc(npc);
        //}
    }

    protected virtual void DamageEffectBody(NPC npc)
    {
        npc.ReceiveDamageWithRepelVector(damage, transform.right);
    }


}


