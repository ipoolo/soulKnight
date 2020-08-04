using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Animator animator;
    [SerializeField] public float damage;
    [SerializeField] public float attackInterval;
    [SerializeField] public bool isCloseInWeapon;

    [SerializeField] public float powerMaxSecond;
    
    private float storagePowersustainTime;

    private float attackTimer;
    [HideInInspector]  public bool canAttack;
    private GameObject player;
    public WeaponPoint weaponPoint;

    public bool isStoragePowerWeapon;
    public bool isStoragePower;
    private PowerController powerController;
    protected float powerBarValue;
    protected bool isStopFire = false;

    public object castor;
    [HideInInspector]public SpriteRenderer sRender;

    protected void Awake()
    {
        sRender = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        attackTimer = attackInterval;
        canAttack = true;

        weaponPoint = GameObject.FindGameObjectWithTag("WeaponPoint").GetComponent<WeaponPoint>();
        player = GameObject.FindGameObjectWithTag("Player");

        powerController = GameObject.FindGameObjectWithTag("UI_PowerBar").GetComponent<PowerController>();
        if (castor == null)
        {
            castor = GameObject.FindGameObjectWithTag("PlayerStateController").GetComponent<PlayerStateController>();
        }
    }


    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        if (attackTimer >= attackInterval)
        {
            canAttack = true;
        }
        else
        {
            canAttack = false;
        }
        attackTimer += Time.deltaTime;

        if (isStoragePower)
        {
            storagePowersustainTime += Time.deltaTime;
            if (powerMaxSecond != 0)
            {
                float tempPowerValue = storagePowersustainTime / powerMaxSecond;
                powerBarValue = tempPowerValue > 1 ? 1 : tempPowerValue;
                powerController.updatePowerBarValue(powerBarValue);
                StoragePowerUpdateBody(powerBarValue);
            }
        }
    }
    public void InterruptStoragePower()
    {
        storagePowersustainTime = 0;
        isStoragePower = false;
        powerBarValue = 0;
        powerController.updatePowerBarValue(powerBarValue);
        powerController.hidePowerBar();
    }

    protected virtual void StoragePowerUpdateBody(float persent)
    {

    }



    public void ChangeWeaponDirection(Vector2 weaponPointRightDirection)
    {
        if (Vector2.Dot(weaponPointRightDirection, Vector2.right) > 0)
        {
            sRender.flipY = false;
        }
        else
        {
            sRender.flipY = true;
        }

    }

    //这里用动画用动画配置Fire名称,并且回调AnimFireCallBack方法

    public void Attack()
    {
        if (canAttack)
        {
            if (animator != null)
            {
                animator.SetTrigger("Fire");
                weaponPoint.pauseFollow();
            }

            if (isStoragePowerWeapon)
            {
                if (isStoragePower) { 
                    AttackBody();
                }
            }
            else
            {
                AttackBody();
            }
            //powerBar处理
            isStoragePower = false;
            powerController.hidePowerBar();
        }

    }

    public virtual void AttackBody()
    {
        attackTimer = 0.0f;
    }

    private void AnimFireCallBack()
    {
        weaponPoint.continueFollow();
        AnimFireCallBackBody();
    }

    protected virtual void AnimFireCallBackBody()
    {

    }

    //StoragePower
    public void StoragePower()
    {
        if (canAttack)
        {
            storagePowersustainTime = 0.0f;
            isStoragePower = true;
            powerController.showPowerBar();
        }
    }
    public float ExcuteHittingBuffEffect(float _damage)
    {
        float tmp = _damage;
        if (castor is BuffReceiverInterFace)
        {
            List<Buff> buffList = (castor as BuffReceiverInterFace).GetBuffList();
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
    //检查mouse 小于武器出口时翻转向量
    public Vector3 CalTargetDirection(Vector3 _firePointPosition, Vector3 _mousePosition, Vector3 _transformPosition)
    {
        _mousePosition = new Vector3(_mousePosition.x, _mousePosition.y,0);
        Vector3 targetDirection = Vector3.zero;
        Vector3 vectorfireP2MouseP = _mousePosition - _firePointPosition;
        targetDirection = vectorfireP2MouseP;
        Vector3 vectortransformP2fireP = _firePointPosition - _transformPosition;
        if (Vector3.Dot(vectorfireP2MouseP, vectortransformP2fireP) < 0)
        //小于零说明不同向
        {
            targetDirection = Quaternion.Euler(0, 0, 180) * vectorfireP2MouseP;
        }

        return targetDirection;
    }

    public void ChangeIsStopFire(bool isStop)
    {
        isStopFire = isStop;
        if (isStopFire)
        {
            weaponPoint.continueFollow();
        }
    }
}
