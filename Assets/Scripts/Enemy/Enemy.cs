﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Enemy : NPC ,CanSkillControl, SkillFinishCallBack
{
    
    enum EnemyStateType
    {
        enemyStatePatrol,
        enemyStateFollowPlayer

    }
    public Animator animator;

    [SerializeField] public float moveSpeedLevelUpScale;

    [SerializeField] public float damageLevelUpScale;

    public float touchSenseDistance;
    public float perspectiveSenseDistance;
    public float perspectiveSenseFiledOfView;

    [HideInInspector]public  bool isAvoid = false;
    public float minAvoidDistance;
    public float maxAvoidDistance;
    [HideInInspector] public float distance2Player;

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

    [HideInInspector]public bool isPerspectiveSense;
    [HideInInspector]public bool isTouchSensePalyer;
    public TouchSense touchSense;

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

    public System.Action<Enemy> destoryDelegate;

    private float accumulationTimer;
    private float accumulationMaxTime = 1.5f;
    private float accumulationDamage;

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

        touchSense = GetComponentInChildren<TouchSense>();
        if (touchSense)
        {
            touchSense.setSenceDinstance(touchSenseDistance);
        }

    }
    private void ConfigFSM()
    {
        fsm = new StateMachine<Enemy>();
        fsm.ConfigState(this, FSMEnemyStatePatrol.singleInstance, FSMEnemyStateGolbal.singleInstance);
    }

    // Update is called once per frame
    public new void Update()
    {
        base.Update();
        //伤害累加显示计算器
        CalDamageAccumulationShow();

         if (!isPause) { 
            base.Update();
            
            survivalTime += Time.deltaTime;

            //不是技能控制,不是失去控制时执行
            if (!isOutControl && !isSkillControl) {

                fsm.StateMachineUpdate(this);
                Debug.DrawLine(transform.position, patrolTargetPosition, Color.white);
            
            }
            //检查运动方向与sprite朝向
            CheckVelocityDirection();
        }
        else
        {
            rigid2d.velocity = Vector2.zero;
            //暂停状态 被攻击到不能移动.
        }

    }

    private void CalDamageAccumulationShow()
    {
        accumulationTimer += Time.deltaTime;
        if (accumulationTimer > accumulationMaxTime)
        {
            accumulationDamage = 0;
            accumulationTimer = 0;
        }
    }

    IEnumerator BackToPatrol(Enemy t)
    {
        yield return new WaitForSeconds(patrolWaitTime);
        CalculateNewTarget(t);

    }

    public void CalculateNewTarget(Enemy t)
    {
        bool isLegalPosition = false;
        while (!isLegalPosition) { 
            t.patrolTargetPosition = new Vector3(Random.Range(t.patrolBottomLeft.position.x, t.patrolTopRight.position.x),
            Random.Range(t.patrolBottomLeft.position.y, t.patrolTopRight.position.y), 0);
            float distance = Vector2.Distance(t.patrolTargetPosition, transform.position);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, t.patrolTargetPosition - transform.position, distance, LayerMask.GetMask("Item", "Wall"));
            if (hit.collider == null)
            {
                isLegalPosition = true;
            }
        }
        
        t.isPatrolRunning = true;
    }

    private void CheckVelocityDirection()
    {
        float velocityDirectionX = rigid2d.velocity.x;
        if (isAvoid)
        {
            //躲避时,面向方向为速度相反方向
            velocityDirectionX *= -1;
        }

        if (velocityDirectionX > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (velocityDirectionX < 0)
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
        if (!_player.GetComponent<PlayerController>().isDead) { 
            PlayerStateController pc = _player.GetComponentInChildren<PlayerStateController>();
            //pc 受伤伤害 TODO
            pc.ReceiveDamageWithRepelVector(ExcuteHittingBuffEffect(damage), _player.transform.position - transform.position);
        }


    }

    protected override bool ReceiveDamageWithRepelVectorBody(float _damage, Vector3 _repelVector)
    {
        bool isDeath = false;
        if(_damage > 0) { 
            RenderRed();
            //;
            EffectBlood(_repelVector);
            isDeath = ReduceHealthBody(_damage);
        }
        return isDeath;

    }

    protected void EffectBlood(Vector3 _repelVector)
    {
        GameObject bloodGB = Instantiate((GameObject)Resources.Load("Effect/Blood/BloodLeft"));
        bloodGB.transform.parent = transform;
        bloodGB.transform.position = transform.position ;
        bloodGB.transform.rotation = Quaternion.FromToRotation(Vector2.left, _repelVector);
    }


    protected bool ReduceHealthBody(float _reduceValue)
    {
        bool isDeath = false;
        int floorValue = Mathf.FloorToInt(_reduceValue);
        health -= floorValue;
        //掉血粒子效果
        GameObject  psObject =  Instantiate(psEffect, transform.position, Quaternion.identity);
        //掉血数值 (暴击值和普通值这里应该通过配置设置不同效果)
        DamageText damageText = Instantiate(canvasDamage, transform.position, Quaternion.identity).GetComponent<DamageText>();

        //伤害累加显示
        accumulationDamage += floorValue;
        accumulationTimer = 0;
        damageText.setDamageText(accumulationDamage);

        //为了更容易被检测
        psObject.transform.parent = transform;

        if (health <= 0)
        {
            isDeath = true;

        }

        return isDeath;
    }

    protected override void DeathBody()
    {
        base.DeathBody();
        //新建死亡效果
        Instantiate(Resources.Load("EnemyDeath"), transform.position, Quaternion.identity);
        //新建宝物掉落
        Coin coin = Instantiate((GameObject)Resources.Load("Coin")).GetComponent<Coin>();
        coin.transform.position = transform.position;
        coin.radius = 2.0f;
        AudioManager.Instance.PlaySound3WithTime("Voices/RetroExplosionShort15", 0.0f);
        Instantiate((GameObject)Resources.Load("Effect/BloodArea"), transform.position, Quaternion.identity);
        //销毁自己

        GetComponentInParent<EnemyBase>().baseDestory();
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

    private EnemyHint eHint;
    public void showEnemyHint()
    {
        if(eHint == null) {
            eHint = Instantiate((GameObject)Resources.Load("Hint/EnemyHint"), transform.position, Quaternion.identity).GetComponent<EnemyHint>();
            eHint.transform.parent = transform;
        }
    }

    private void OnDestroy()
    {


        destoryDelegate(this);
    }



}
