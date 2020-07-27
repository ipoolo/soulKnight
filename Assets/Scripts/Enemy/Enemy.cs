using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.iOS;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Enemy : NPC, BuffReceiverInterFace, CanSkillControl, SkillFinishCallBack
{



    public Animator animator;


    [SerializeField] public float moveSpeedLevelUpScale;

    [SerializeField] public float damageLevelUpScale;

    public float senseRaidus;

    public GameObject canvasDamage;//绘制伤害
    public GameObject enemyDeathAnim;


    public float turnRedTime;
    public float patrolTime;
    [HideInInspector]public float patrolTimer;//计时器
    public float patrolWaitTime;
    public Transform patrolBottomLeft;
    public Transform patrolTopRight;

    public GameObject psEffect;//这里可以写的基类，提高复用

    public StateMachine<Enemy> fsm;

    enum EnemyStateType
    {
        enemyStatePatrol,
        enemyStateFollowPlayer

    }

    public GameObject player;
    public Vector3 playerPosition;

    private Color originalColor;
    private SpriteRenderer render;
    private bool isSkillControl = false;

    public float skillCoolDownTimer = 0;
    public float SkillFireInterval;
    private float survivalTime;

    public Vector3 patrolTargetPosition;
    public bool isPatrolRunning;


    private Skill currSkill;
    private SkillConfig currSkillConfig = new SkillConfig();
    public List<string> skillRandomResPathList = new List<string>();
    public List<string> skillFireConditionPathList = new List<string>();
    private List<SkillFireConditionController> skillFireConditionControllerList = new List<SkillFireConditionController>();

    public bool isSkillTimerStop;

    // Start is called before the first frame update
    public new void Start()
    {
        base.Start();
        ConfigDefalut();
        ConfigFSM();
    }

    private void ConfigDefalut()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        render = GetComponent<SpriteRenderer>();
        originalColor = render.color;
        health = maxHealth;
        //巡逻跟踪时间
        if (patrolTime == 0)
        {
            patrolTime = 2;
        }
        isOutControl = false;
        animator = GetComponent<Animator>();

        //初始化 skillFireConditionist
        foreach (string path in skillFireConditionPathList)
        {
            SkillFireConditionController tmp =Instantiate((GameObject)Resources.Load(path),transform.position,Quaternion.identity).GetComponent<SkillFireConditionController>();
            tmp.transform.parent = transform;
            skillFireConditionControllerList.Add(tmp);
        }
        survivalTime = 0;

    }
    private void ConfigFSM()
    {
        fsm = new StateMachine<Enemy>();
        fsm.ConfigState(this, FSMEnemyStatePatrol.singleInstance, FSMEnemyStateGolbal.singleInstance);
        //golbalState还没写
        

    }


    // Update is called once per frame
    public new void Update()
    {
        base.Update();
        survivalTime += Time.deltaTime;

        //不是技能控制,不是失去控制时执行
        if (!isOutControl && !isSkillControl) {

            Debug.Log("Update");
            fsm.StateMachineUpdate(this);
            Debug.DrawLine(transform.position, patrolTargetPosition, Color.white);
            
        }
        //检查运动方向与sprite朝向
        CheckVelocityDirection();

    }

    IEnumerator BackToPatrol(Enemy t)
    {
        yield return new WaitForSeconds(patrolWaitTime);
        CalculateNewTarget(t);

    }

    public void CalculateNewTarget(Enemy t)
    {
        t.patrolTargetPosition = new Vector3(Random.Range(t.patrolBottomLeft.position.x, t.patrolTopRight.position.x),
        Random.Range(t.patrolBottomLeft.position.y, t.patrolTopRight.position.y), 0);
        t.isPatrolRunning = true;
    }



    private void CheckVelocityDirection()
    {
        if(rigid2d.velocity.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (rigid2d.velocity.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    public void FireRandomSkill()
    {

        int randomIndex = Random.Range(0, skillRandomResPathList.Count - 1);
        if(skillRandomResPathList.Count != 0) { 
            GameObject skillPrefab = (GameObject)Resources.Load(skillRandomResPathList[randomIndex]);
            currSkill = ((GameObject)Instantiate(skillPrefab, transform.position, Quaternion.identity)).GetComponent<Skill>();
            currSkill.transform.position = transform.position;
            currSkill.transform.parent = transform;
            currSkillConfig.castor = gameObject;
            currSkillConfig.animator = animator;
            currSkillConfig.skillCanSkillControlDelegate = this;
            currSkillConfig.skillFinishDelegate = this;
            currSkillConfig.castorIsEnemy = true;
            currSkillConfig.targetPosition = playerPosition;
            currSkillConfig.castorRigidbody2d = rigid2d;
            currSkill.ConfigSkill(currSkillConfig);
            currSkill.CastSkill();
        }

    }

    public void CheckFireConditionPathList()
    {
        List<SkillFireConditionController> removeList = new List<SkillFireConditionController>();
        foreach(SkillFireConditionController fc in skillFireConditionControllerList)
        {
            currSkillConfig.castor = gameObject;
            currSkillConfig.animator = animator;
            currSkillConfig.skillCanSkillControlDelegate = this;
            currSkillConfig.skillFinishDelegate = this;
            currSkillConfig.castorIsEnemy = true;
            currSkillConfig.targetPosition = playerPosition;
            currSkillConfig.castorRigidbody2d = rigid2d;

            SkillFireConditionConfigDetails _details = new SkillFireConditionConfigDetails();
            _details.castorHealth = health;
            _details.castorHealthPresentValue = health / maxHealth;
            _details.runTimer = survivalTime;
            if(fc.ShouldFireSkill(_details, 0))
            {
                GameObject skillPrefab = (GameObject)Resources.Load(fc.skillResPath);
                currSkill = ((GameObject)Instantiate(skillPrefab, transform.position, Quaternion.identity)).GetComponent<Skill>();
                currSkill.transform.position = transform.position;
                currSkill.transform.parent = transform;
                currSkill.ConfigSkill(currSkillConfig);
                currSkill.CastSkill();
                removeList.Add(fc);
            }
        }
        foreach(SkillFireConditionController fc in removeList)
        {
            skillFireConditionControllerList.Remove(fc);
        }

    }

    public virtual bool enemyLevelUpRulesBody(Enemy _enmey, int _level)
    {
        // ture 配置好了，false未配置 调用系统配置
        _enmey.moveSpeed += _level * moveSpeedLevelUpScale;
        _enmey.damage += _level * damageLevelUpScale;
        return true;
    }

    public void checkIsFollowToPlayer()
    {
        //playerPosition = player.transform.position;
        //if ((playerPosition - transform.position).sqrMagnitude < senseRaidus)
        //{
        //    if(enemyStateType == EnemyStateType.enemyStatePatrol)
        //    {
        //        //状态切换时
        //        skillTimerStop = false;
        //        skillCoolDownTimer = 0;

        //    }
        //    enemyStateType = EnemyStateType.enemyStateFollowPlayer;
                
        //    //只有跟踪玩家时才开始计算技能释放
        //}
        //else
        //{
        //    enemyStateType = EnemyStateType.enemyStatePatrol;
        //    skillTimerStop = true;
        //}
    }


    private void patrol()
    {
        

    }



    private void OnCollisionEnter2D(Collision2D _collision)
    {
        Collider2D other = _collision.collider;
        Collider2D self = _collision.otherCollider;
        if (other.CompareTag("Player"))
        {
            //tesst
            Message mgs = new Message();
            mgs.receiver = this.entityIdString;
            mgs.msg = 1;
            GameObject.FindGameObjectWithTag("GameController").GetComponent<MessageDispatcher>().DispatchMassage(0, mgs);
            
            //test -end
            TouchPlayerBody(_collision, other);
        }

    }

    public virtual void TouchPlayerBody(Collision2D _collision, Collider2D _player)
    {

        PlayerStateController pc = _player.GetComponentInChildren<PlayerStateController>();
        //pc 受伤伤害 TODO
        pc.ReceiveDamageWithRepelVector(ExcuteHittingBuffEffect(damage), _player.transform.position - transform.position);


    }


    public override void ReceiveDamageWithRepelVectorBody(float _damage, Vector3 _repelVector)
    {
        RenderRed();
    }




    public override void ReduceHealthBody(float _reduceValue)
    {
        int floorValue = Mathf.FloorToInt(_reduceValue);
        health -= floorValue;
        //掉血粒子效果
        GameObject  psObject =  Instantiate(psEffect, transform.position, Quaternion.identity);
        //掉血数值 (暴击值和普通值这里应该通过配置设置不同效果)
        DamageText damageText = Instantiate(canvasDamage, transform.position, Quaternion.identity).GetComponent<DamageText>();
        damageText.setDamageText(floorValue);

        //为了更容易被检测
        psObject.transform.parent = transform;

        if (health <= 0)
        {
            //新建死亡效果

            Instantiate(Resources.Load("EnemyDeath"), transform.position, Quaternion.identity);
            //新建宝物掉落
            GameObject hp_bar = Instantiate((GameObject)Resources.Load("Coin"));
            hp_bar.transform.position = transform.position;

            //销毁自己
            GetComponentInParent<EnemyBase>().baseDestory();
        }

    }

    private void RenderRed()
    {
        render.color = Color.red;
        StartCoroutine("BackToOriginalColor");
    }


    IEnumerator BackToOriginalColor()
    {
        yield return new WaitForSeconds(turnRedTime);
        render.color = originalColor;
    }

    public void SkillFire(){
        //animationEeventCall
        currSkill.turnState2SkillFire();
    }

    public void SetCanSkillControl(bool _canSkillContro)
    {
        isSkillControl = _canSkillContro;
    }

    public bool GetCanSkillControl()
    {
        throw new System.NotImplementedException();
    }

    public void SkillFinishCallBack()
    {
        //dosomething;
        skillCoolDownTimer = 0;
        isSkillTimerStop = false;
    }


    public override bool ReceiveMsg(Message msg)
    {
        return fsm.receiveMessage(msg);
    }

    public void test() { 
    GameObject skillPrefab = (GameObject)Resources.Load("Skill/SkillRage");
    currSkill = ((GameObject) Instantiate(skillPrefab, transform.position, Quaternion.identity)).GetComponent<Skill>();
    currSkill.transform.position = transform.position;
    currSkill.transform.parent = transform;
    currSkill.ConfigSkill(currSkillConfig);
    currSkill.CastSkill();

    }
}
