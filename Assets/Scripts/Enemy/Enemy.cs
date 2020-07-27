using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.iOS;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Enemy : NPC, BuffReceiverInterFace, CanSkillControl, SkillFinishCallBack
{

    public Transform BottomLeft;
    public Transform TopRight;

    public Animator animator;
    public float waitTime;

    [SerializeField] public float moveSpeedLevelUpScale;

    [SerializeField] public float damageLevelUpScale;

    public float senseRaidus;

    public GameObject canvasDamage;//绘制伤害
    public GameObject enemyDeathAnim;


    public float turnRedTime;
    public float patrolTime;
    public float patrolTimer;

    public GameObject psEffect;//这里可以写的基类，提高复用

    enum EnemyStateType
    {
        enemyStatePatrol,
        enemyStateFollowPlayer

    }
    private EnemyStateType enemyStateType = EnemyStateType.enemyStatePatrol; // 没写枚举，直接用数字代替了0 代表巡逻 1代表追击

    private bool isRunning;


    private Vector3 targetPosition;
    private GameObject player;
    private Vector3 playerPosition;

    private Color originalColor;
    private SpriteRenderer render;
    private bool isSkillControl = false;

    private float skillCoolDownTimer = 0;
    public float SkillFireInterval;
    private float survivalTime;


    private Skill currSkill;
    private SkillConfig currSkillConfig = new SkillConfig();
    public List<string> skillRandomResPathList = new List<string>();
    public List<string> skillFireConditionPathList = new List<string>();

    private List<SkillFireConditionController> skillFireConditionControllerList = new List<SkillFireConditionController>();

    // Start is called before the first frame update
    public new void Start()
    {
        base.Start();
        ConfigDefalut();
        calculateNewTarget();
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

    private bool skillTimerStop = true;
    // Update is called once per frame
    public new void Update()
    {
        base.Update();
        survivalTime += Time.deltaTime;

        //不是技能控制,不是失去控制时执行
        if (!isOutControl && !isSkillControl) {
            checkIsFollowToPlayer();
            switch (enemyStateType)
            {
                case EnemyStateType.enemyStatePatrol:
                    patrol();
                    break;
                case EnemyStateType.enemyStateFollowPlayer:
                    fellowToPlayer();
                    break;
                
                default:
                    patrol();
                    break;
            }
            if (!skillTimerStop) {

                //skillConditionPathList 判断
                CheckFireConditionPathList();

                //condition技能可以改变是否锁定 再检测一次
                if (!isSkillControl)
                {

                    skillCoolDownTimer += Time.deltaTime;
                    if (skillCoolDownTimer >= SkillFireInterval)
                    {
                        //随机选择技能列表技能释放 TODO这里可以做概率触发技能配置
                        FireRandomSkill();
                        skillTimerStop = true;

                    }
                }
            }
        }
        //检查运动方向与sprite朝向
        CheckVelocityDirection();

    }

    private void CheckVelocityDirection()
    {
        if(rigid2d.velocity.x >= 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private void FireRandomSkill()
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

    private void CheckFireConditionPathList()
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
        playerPosition = player.transform.position;
        if ((playerPosition - transform.position).sqrMagnitude < senseRaidus)
        {
            if(enemyStateType == EnemyStateType.enemyStatePatrol)
            {
                //状态切换时
                skillTimerStop = false;
                skillCoolDownTimer = 0;

            }
            enemyStateType = EnemyStateType.enemyStateFollowPlayer;
                
            //只有跟踪玩家时才开始计算技能释放
        }
        else
        {
            enemyStateType = EnemyStateType.enemyStatePatrol;
            skillTimerStop = true;
        }
    }

    public void fellowToPlayer()
    {
        //将方向拆分给速度
        Vector3 targetVector = playerPosition - transform.position;
        Vector3 normalizedVector = targetVector.normalized;
        Vector3 velocityVector = normalizedVector * moveSpeed;
        rigid2d.velocity = velocityVector;
    }

    private void patrol()
    {
        if (isRunning) {
            patrolTimer += Time.deltaTime;
            //超时或者到达都进入休息，并且寻找新目标(防止目标为不可抵达)
            Vector3 targetVector = targetPosition - transform.position;
            Vector3 normalizedVector = targetVector.normalized;
            Vector3 velocityVector = normalizedVector * moveSpeed;
            rigid2d.velocity = velocityVector;
            if ((targetPosition - transform.position).sqrMagnitude < 0.2f)
            {
                //等待 然后新目标
                isRunning = false;
                Invoke("calculateNewTarget", waitTime);
                patrolTimer = 0;
                rigid2d.velocity = Vector3.zero;
            }
            if (patrolTimer >= patrolTime)
            {
                isRunning = false;
                Invoke("calculateNewTarget", waitTime);
                patrolTimer = 0;
                rigid2d.velocity = Vector3.zero;
            }
        }
        else
        {
            rigid2d.velocity = Vector3.zero;
        }

    }

    private void calculateNewTarget()
    {
        targetPosition = new Vector3(Random.Range(BottomLeft.position.x, TopRight.position.x),
            Random.Range(BottomLeft.position.y, TopRight.position.y), 0);
        isRunning = true;
    }

    private void OnCollisionEnter2D(Collision2D _collision)
    {
        Collider2D other = _collision.collider;
        Collider2D self = _collision.otherCollider;
        if (other.CompareTag("Player"))
        {
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

    public bool getCanSkillControl()
    {
        throw new System.NotImplementedException();
    }

    public void SkillFinishCallBack()
    {
        //dosomething;
        skillCoolDownTimer = 0;
        skillTimerStop = false;
    }


}
