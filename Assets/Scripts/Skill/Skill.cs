using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

interface CanSkillControl
{
    void SetCanSkillControl(bool _isSkillControl);

    bool getCanSkillControl();

}

interface SkillFinishCallBack
{ 

    void SkillFinishCallBack();
}

public struct SkillConfig
{
    public GameObject castor;
    public Animator animator;
    public object skillCanSkillControlDelegate;
    public object skillFinishDelegate;
    public bool castorIsEnemy;
    public Vector3 targetPosition;
    public Rigidbody2D castorRigidbody2d;
}

public enum SkillStateType
{
    skillTypeNone,
    skillTypeConfig,
    skillTypeCasting,
    skillTypeRunning,
    skillTypeFinish,
}


public class Skill : MonoBehaviour
{
    public SkillConfig skillConfig;
    [SerializeField] public string castingDefaultAnimationPath;
    [SerializeField] public string castingDefaultAnimationLayer;
    [SerializeField] public float animPlayTimeOffest;
    [SerializeField] public float skillCastingTime;
    [SerializeField] public bool isLockCastorControllerAtSkillCasting;
    [SerializeField] public bool isLockCastorControllerAtSkillRunnig;
    [SerializeField] public string skillEffectPrefabsPath;
    private GameObject skillEffectPrefabs;
    private float castTimer;
    private float runningTimer;
    private float finishTimer;
    public SkillStateType skillStateType;
    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (skillStateType)
        {
            case SkillStateType.skillTypeConfig:
                break;
            case SkillStateType.skillTypeCasting:
                CastUpdate();
                break;
            case SkillStateType.skillTypeRunning:
                RunningUpdate();
                break;
            case SkillStateType.skillTypeFinish:
                FinishUpdate();
                break;
        }
    }

    public virtual void ConfigSkill(SkillConfig _skillConfig)
    {
        skillConfig = _skillConfig;
        skillStateType = SkillStateType.skillTypeConfig;
        if (skillEffectPrefabsPath.Length != 0) { 
            skillEffectPrefabs = (GameObject)Resources.Load(skillEffectPrefabsPath);
        }
    }

    public virtual void CastSkill()
    {
        if (isLockCastorControllerAtSkillCasting)
        {
            LockCastorContorl();
        }
        if (skillEffectPrefabs) { 
            GameObject skillEffectGO = Instantiate(skillEffectPrefabs, transform.position, Quaternion.identity);
            skillEffectGO.transform.parent = transform;
        }

        castTimer = 0.0f;
        skillStateType = SkillStateType.skillTypeRunning;
        Debug.Log("CastSkill");

    }

    private void CastUpdate()
    {
        Debug.Log("CastUpdate");
        if (castTimer == 0)
        {
            CastSkillOnceBody();
        }

        castTimer += Time.deltaTime;
        if (castTimer >= skillCastingTime)
        {
            //技能转向执行阶段
            skillStateType = SkillStateType.skillTypeRunning;
        }
        else
        {
            //留给子类之类的做cast动效(粒子效果等)
            CastSkillUpdateBody(castTimer);
        }
    }

    public virtual void CastSkillOnceBody()
    {

        if (skillConfig.animator != null)
        {
            skillConfig.animator.Play(castingDefaultAnimationPath, LayerMask.NameToLayer(castingDefaultAnimationLayer), animPlayTimeOffest);
            //skillConfig.animator.runtimeAnimatorController.animationClips[]
            //这里的吟唱到触发是用的计时器，也可以用AnimationEvent，动画剪辑回调到cast 然后传进来改变SkillStateType的状态。
        }
    }

    public virtual void CastSkillUpdateBody(float castTimer)
    {

    }

    private void RunningUpdate()
    {
        if (runningTimer == 0)
        {
            RunningSkillOnceBody();
        }

        runningTimer += Time.deltaTime;

        //留给子类之类的做cast动效(粒子效果等)
        RunningSkillUpdateBody(runningTimer);
        
    }

    public virtual void RunningSkillOnceBody()
    {
        if (isLockCastorControllerAtSkillRunnig)
        {
            LockCastorContorl();
        }

        if (skillConfig.animator != null && castingDefaultAnimationPath.Length != 0)
        {
            skillConfig.animator.Play(castingDefaultAnimationPath, LayerMask.NameToLayer(castingDefaultAnimationLayer), animPlayTimeOffest);
            //skillConfig.animator.runtimeAnimatorController.animationClips[]
            //这里的吟唱到触发是用的计时器，也可以用AnimationEvent，动画剪辑回调到cast 然后传进来改变SkillStateType的状态。
        }
    }

    public virtual void RunningSkillUpdateBody(float castTimer)
    {
        //技能子类需要自己设置技能结束
        skillStateType = SkillStateType.skillTypeFinish;
    }

    private void FinishUpdate()
    {
        if (finishTimer == 0)
        {
            InvokeFinishDelegate();
            UnLockCastorContorl();
            FinishSkillOnceBody();
        }

        finishTimer += Time.deltaTime;

        //留给子类之类的做cast动效(粒子效果等)
        FinishSkillUpdateBody(runningTimer);
    }
    public virtual void FinishSkillOnceBody()
    {

    }

    public virtual void FinishSkillUpdateBody(float runningTimer)
    {
        Destroy(gameObject);
    }



    public virtual void turnState2SkillFire()
    {
        skillStateType = SkillStateType.skillTypeRunning;
        
    }

    private void InvokeFinishDelegate()
    {
        if (skillConfig.skillFinishDelegate is SkillFinishCallBack)
        {
            ((SkillFinishCallBack)skillConfig.skillCanSkillControlDelegate).SkillFinishCallBack();
                
        }
    }

    
    public void LockCastorContorl()
    {
        if(skillConfig.skillCanSkillControlDelegate is CanSkillControl)
        {
            ((CanSkillControl)skillConfig.skillCanSkillControlDelegate).SetCanSkillControl(true);
        }
    }

    public void UnLockCastorContorl()
    {
        if (skillConfig.skillCanSkillControlDelegate is CanSkillControl)
        {
            ((CanSkillControl)skillConfig.skillCanSkillControlDelegate).SetCanSkillControl(false);
        }
    }

}
