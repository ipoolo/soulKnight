using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffDotOrHot : Buff
{

    [SerializeField] public float stepTime;
    [SerializeField] public int effectTotalTimes = 0;
    [HideInInspector]public int effectHappenTimes = 0;

    private float buffTimer;
    private bool isActiveTimer = false;

    public float timerOffset;
    public float destroyWaitTime;

    private void Awake()
    {
        duration = stepTime *  effectTotalTimes + 0.1f - timerOffset;
        //0.01 偏移量
        buffTimer = timerOffset;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public void Update()
    {

        if (isActiveTimer) { 
            CheckEffect();
        }

    }

    private void CheckEffect()
    {
        //Debug.Log("buffTimer" + buffTimer);
        buffTimer += Time.deltaTime;
        if(buffTimer >= stepTime && effectHappenTimes < effectTotalTimes)
        {
            buffTimer = 0;
            Effect();
            effectHappenTimes++;
            if (effectHappenTimes == effectTotalTimes)
            {
                Invoke("BuffUnLoad", destroyWaitTime);
            }
        }

    }
    private void Effect()
    {
        EffectBody();
    }

    public virtual void EffectBody()
    {
        Debug.Log("effect:" + buffTimer);
    }

    public override bool BuffLoadBodyAndIsInvoke()
    {
        base.BuffLoadBodyAndIsInvoke();
        //是否立刻生效看配置的timeoffset
        isActiveTimer = true;
        return false;
        //dot完成后自己调用unload不用 buff的unload调用生命周期
    }


}
