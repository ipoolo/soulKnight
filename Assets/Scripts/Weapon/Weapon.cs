using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EWeaponType{
    normal,
    storagePower,
    coutinue,
    storagePowerAndCoutinue
}

public class Weapon : MonoBehaviour
{
    [Header("震动摄像机")]
    public float shakeCameraStepTime;
    protected float shakeCameraStepTimer;
    public float impulseScale;
    public Animator animator;

    [SerializeField] public float damage;
    [SerializeField] public float attackInterval;
    [SerializeField] public bool isCloseInWeapon;
    protected CinemachineImpulseSource impulseSource;

    [SerializeField] public float powerMaxSecond;
    
    private float storagePowersustainTime;

    private float attackTimer;
    [HideInInspector]  public bool canAttack;
    protected PlayerController player;
    [HideInInspector]
    public WeaponPoint weaponPoint;
    public EWeaponType weaponType;

    private bool isStoragePower;
    private PowerController powerController;
    protected float powerBarValue;
    protected bool isStopFire = false;
    [SerializeField] public GameObject firePoint;

    public object castor;
    [HideInInspector]public SpriteRenderer sRender;
    public bool IsAutoFireAnimator = true;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (name == "GodSword") { 
            Debug.Log("transform.position" + transform.position);
        }
    }
#endif

    protected void Awake()
    {
        sRender = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        impulseSource = GetComponent<CinemachineImpulseSource>();

        attackTimer = attackInterval;
        canAttack = true;

        weaponPoint = GameObject.FindGameObjectWithTag("WeaponPoint").GetComponent<WeaponPoint>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        powerController = GameObject.FindGameObjectWithTag("UI_PowerBar").GetComponent<PowerController>();
        if (castor == null)
        {
            castor = GameObject.FindGameObjectWithTag("PlayerStateController").GetComponent<PlayerStateController>();
        }
    }


    protected virtual void Start()
    {

    }

    private bool isDoneStoragePowerOnce = false;
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
                if (!isDoneStoragePowerOnce)
                {
                    StoragePowerOnceBody();
                }
                float tempPowerValue = storagePowersustainTime / powerMaxSecond;
                powerBarValue = tempPowerValue > 1 ? 1 : tempPowerValue;
                powerController.updatePowerBarValue(powerBarValue);
                StoragePowerUpdateBody(powerBarValue);
                if(weaponType == EWeaponType.storagePowerAndCoutinue && tempPowerValue >1)
                {
                    isStoragePower = false;
                    isDoneStoragePowerOnce = false;
                    Attack();
                }
            }
        }
        if (isExecuteUpdate)
        {
            ContinueUpdate();
        }
    }
    public void InterruptStoragePower()
    {
        storagePowersustainTime = 0;
        isStoragePower = false;
        isDoneStoragePowerOnce = false;
        powerBarValue = 0;
        powerController.updatePowerBarValue(powerBarValue);
        powerController.hidePowerBar();
        InterruptStoragePowerBody();
    }

    protected virtual void InterruptStoragePowerBody()
    {

    }



    protected virtual void StoragePowerOnceBody()
    {
        Debug.Log("a");
        isDoneStoragePowerOnce = true;
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
            if (animator != null && IsAutoFireAnimator)
            {
                animator.SetTrigger("Fire");
                weaponPoint.pauseFollow();
            }

            if (weaponType == EWeaponType.storagePower)
            {
                if (isStoragePower) {
                    AttackBody();
                }
            }
            else if (weaponType == EWeaponType.coutinue)
            {
                AttackBody();
                if(animator == null)
                {
                    AnimFireCallBack();
                }
            }
            else if (weaponType == EWeaponType.storagePowerAndCoutinue)
            {
                AttackBody();
                if (animator == null)
                {
                    AnimFireCallBack();
                }
            }
            else
            {
                AttackBody();
            }
            //powerBar处理
            isStoragePower = false;
            isDoneStoragePowerOnce = false;
            powerController.hidePowerBar();
        }

    }

    protected virtual void AttackBody()
    {
        attackTimer = 0.0f;
    }

    private void AnimFireCallBack()
    {
        weaponPoint.ContinueFollow();
        AnimFireCallBackBody();
    }

    private bool isExecuteUpdate = false;

    protected virtual void AnimFireCallBackBody()
    {
        if (weaponType == EWeaponType.coutinue || weaponType == EWeaponType.storagePowerAndCoutinue)
        {
            isExecuteUpdate = true;
        }
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

    public void PlayerKeyUpWhenStoragePowerAndCoutinue()
    {
        if (isStoragePower)
        {
            InterruptStoragePower();
        }
        else
        {
            ContinueFinish();
        }
    }

    public void ChangeIsStopFire(bool isStop)
    {
        isStopFire = isStop;
        if (isStopFire)
        {
            weaponPoint.ContinueFollow();
            ContinueFinish();
        }
    }

    private void ContinueUpdate()
    {
        ContinueUpdateBody();
    }

    protected virtual void ContinueUpdateBody()
    //每一帧都会调用
    {

    }

    //
    public void ContinueFinish()
    {
        isExecuteUpdate = false;
        ContinueFinishBody();
    }

    protected virtual void ContinueFinishBody()
    {

    }


}
