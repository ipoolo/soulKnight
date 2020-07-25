using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.iOS;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Enemy : MonoBehaviour, BuffReceiverInterFace, CanSkillControl, SkillFinishCallBack
{

    public Transform BottomLeft;
    public Transform TopRight;

    public Animator animator;
    public float waitTime;
    public float moveSpeed;
    [SerializeField] public float moveSpeedLevelUpScale;
    public float damage;
    [SerializeField] public float damageLevelUpScale;

    public float senseRaidus;

    public GameObject canvasDamage;//绘制伤害
    public GameObject enemyDeathAnim;

    public int maxHealth;
    public int health;
    public float turnRedTime;
    public float patrolTime;
    public float patrolTimer;

    public GameObject psEffect;//这里可以写的基类，提高复用

    private int enemyState = 0; // 没写枚举，直接用数字代替了0 代表巡逻 1代表追击

    private bool isRunning;

    private Rigidbody2D rigid2d;

    private Vector3 targetPosition;
    private GameObject player;
    private Vector3 playerPosition;

    private Color originalColor;
    private SpriteRenderer render;

    private bool isOutControl;
    private float outControlTime = 0.05f;

    public bool canReceiveRepel = true;

    public Vector3 buffPositionOffset = new Vector3(0, 1, 0);

    //buff
    private List<Buff> buffList = new List<Buff>();


    private bool isSkillControl = false;

    private float skillCoolDownTimer = 0;

    public float SkillFireInterval;
    public List<string> skillResPathList = new List<string>();
    private Skill currSkill;
    private SkillConfig currSkillConfig;

    // Start is called before the first frame update
    public void Start()
    {
        ConfigDefalut();
        calculateNewTarget();
    }

    private void ConfigDefalut()
    {
        rigid2d = GetComponent<Rigidbody2D>();
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
    }

    private bool skillTimerStop = false;
    // Update is called once per frame
    public void Update()
    {
        //不是技能控制,不是失去控制时执行
        if (!isOutControl && !isSkillControl) {
            checkIsFollowToPlayer();
            switch (enemyState)
            {
                case 1:
                    fellowToPlayer();
                    break;
                case 0:
                default:
                    patrol();
                    break;
            }
            if (!skillTimerStop) { 
                skillCoolDownTimer += Time.deltaTime;
                if(skillCoolDownTimer >= SkillFireInterval)
                {
                    //随机选择技能列表技能释放 TODO这里可以做概率触发技能配置
                    FireRandomSkill();
                    skillTimerStop = true;

                }
            }
        }
    }

    private void FireRandomSkill()
    {

        int randomIndex = Random.Range(0, skillResPathList.Count - 1);
        GameObject skillPrefab = (GameObject)Resources.Load(skillResPathList[randomIndex]);
        currSkill = ((GameObject)Instantiate(skillPrefab, transform.position, Quaternion.identity)).GetComponent<Skill>();
        currSkill.transform.position = transform.position;
        currSkill.transform.parent = transform;
        currSkillConfig.castor = gameObject;
        currSkillConfig.animator = animator;
        currSkillConfig.skillCanSkillControlDelegate = this;
        currSkillConfig.skillFinishDelegate = this;
        currSkillConfig.castorIsEnemy = true;
        currSkill.ConfigSkill(currSkillConfig);


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
            enemyState = 1;
        }
        else
        {
            enemyState = 0;
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
    private float ExcuteHittingBuffEffect(float _damage)
    {
        float tmp = _damage;
        if (this is BuffReceiverInterFace)
        {
            foreach (Buff buff in buffList)
            {
                if (buff is BuffReceiveHittingDamageInterFace)
                {
                    tmp = ((BuffReceiveHittingDamageInterFace)buff).BuffReceiveHittingDamageInterFaceBody(tmp);
                }
            }
        }

        return tmp;
    }

    //RestoreHealth

    public void RestoreHealth(int _hp)
    {
        int tmp = health + _hp;
        health = tmp > maxHealth ? maxHealth : tmp;
    }


    public void ReceiveDamageWithRepelVector(float _damage, Vector3 _repelVector)
    {
        if (canReceiveRepel)
        {
            isOutControl = true;
            rigid2d.velocity = _repelVector / outControlTime;
            StartCoroutine("BackToUnderControl");
        }

        ReduceHealth(ExcuteHittedBuffEffect(_damage));
        RenderRed();

    }

    private float ExcuteHittedBuffEffect(float _damage)
    {
        float tmp = _damage;
        if (this is BuffReceiverInterFace)
        {
            foreach (Buff buff in buffList)
            {
                if (buff is BuffReceiveHittedDamageInterFace)
                {
                    tmp = ((BuffReceiveHittedDamageInterFace)buff).BuffReceiveHittedDamageInterFaceBody(tmp);
                }
            }
        }

        return tmp;
    }

    IEnumerator BackToUnderControl()
    {
        yield return new WaitForSeconds(outControlTime);
        isOutControl = false;
    }

    private void ReduceHealth(float _reduceValue)
    {
        int floorValue = Mathf.FloorToInt(_reduceValue);
        health -= floorValue;
        //掉血粒子效果
        Instantiate(psEffect, transform.position, Quaternion.identity);
        //掉血数值 (暴击值和普通值这里应该通过配置设置不同效果)
        Instantiate(canvasDamage, transform.position, Quaternion.identity).GetComponent<DamageText>().setDamageText(floorValue);

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

    //buffList
    public void AddBuff(Buff _buff)
    {
        Buff existSameBuff = null;
        //验重
        if (CheckBuffIsExist(_buff.ToString(), out existSameBuff))
        {
            buffList.Remove(existSameBuff);
            existSameBuff.BuffUnLoad();
        }

        buffList.Add(_buff);

        Debug.Log("buffList" + buffList);
    }

    private bool CheckBuffIsExist(string _buffName, out Buff _existSameBuff)
    {
        bool returnValue = false;
        _existSameBuff = null;

        foreach (Buff buff in buffList)
        {
            if (buff.ToString().Equals(_buffName))
            {
                returnValue = true;
                _existSameBuff = buff;
            }
        }
        return returnValue;
    }

    public void RemoveBuff(Buff _buff)
    {
        buffList.Remove(_buff);
    }

    public List<Buff> GetBuffList()
    {
        return buffList;
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
