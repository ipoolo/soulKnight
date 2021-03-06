﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMEnemyStateTrack : FSMState<Enemy>
{
    private static FSMEnemyStateTrack _singleInstance;
    public static FSMEnemyStateTrack singleInstance
    {
        get
        {
            if (_singleInstance == null)
            {
                _singleInstance = new FSMEnemyStateTrack();
            }
            return _singleInstance;
        }
        set
        {

        }
    }
    public override void Enter(Enemy t)
    {

    }

    public override void Execute(Enemy t)
    {

        t.rigid2d.velocity = (t.playerPosition - t.transform.position).normalized * t.moveSpeed;

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

        if(t.minAvoidDistance != 0.0f)
        {
            t.distance2Player = Vector3.Distance(t.playerPosition, t.transform.position);
            if (t.distance2Player < t.minAvoidDistance)
            {
                //TODO切换到Avoid状态
                t.fsm.ChangeState(FSMEnemyStateAvoid.singleInstance);
            }
        }
    }

    public override void Exit(Enemy t)
    {

    }

    public override bool HandleMessage(Message msg)
    {
        return false;
    }
}
