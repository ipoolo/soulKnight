using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMEnemyStateAvoid : FSMState<Enemy>
{
    private static FSMEnemyStateAvoid _singleInstance;
    public static FSMEnemyStateAvoid singleInstance
    {
        get
        {
            if (_singleInstance == null)
            {
                _singleInstance = new FSMEnemyStateAvoid();
            }
            return _singleInstance;
        }
        set
        {

        }
    }
    public override void Enter(Enemy t)
    {
        CalAvoidTargetPosition(t);
        t.isAvoid = true;
    }

    private void CalAvoidTargetPosition(Enemy t)
    {
        float randomRadius = Random.Range(-45.0f, 45.0f);
        Vector3 tmp = t.transform.position - t.playerPosition;
        tmp =  Quaternion.Euler(0.0f,0.0f,randomRadius) * tmp;
        t.rigid2d.velocity = tmp.normalized * t.moveSpeed ;
    }

    public override void Execute(Enemy t)
    {


        if (!t.isSkillTimerStop)
        {
            //允许技能释放
            //检查条件触发的技能
            t.CheckFireConditionPathList();
            //condition技能可以改变是否锁定 再检测一次
            if (!t.isSkillTimerStop)
            {
                t.skillCoolDownTimer += Time.deltaTime;
                if (t.skillCoolDownTimer >= t.SkillFireInterval)
                {
                    //随机选择技能列表技能释放 TODO这里可以做概率触发技能配置
                    t.FireRandomSkill();
                    t.isSkillTimerStop = true;

                }
            }

        }

        if (t.maxAvoidDistance != 0.0f)
        {
            t.distance2Player = Vector3.Distance(t.playerPosition, t.transform.position);
            if (t.distance2Player > t.maxAvoidDistance)
            {
                //TODO切换到Track状态
                t.fsm.ChangeState(FSMEnemyStateTrack.singleInstance);
            }
        }
    }

    public override void Exit(Enemy t)
    {
        t.isAvoid = false;
    }

    public override bool HandleMessage(Message msg)
    {
        return false;
    }
}
