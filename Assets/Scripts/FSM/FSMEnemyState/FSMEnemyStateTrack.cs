using System.Collections;
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
    public override void enter(Enemy t)
    {
        t.skillCoolDownTimer = 0;
        t.isSkillTimerStop = false;
    }

    public override void execute(Enemy t)
    {
        //follow
        //将方向拆分给速度
        //Vector3 targetVector = t.playerPosition - t.transform.position;
        //Vector3 normalizedVector = targetVector.normalized;
        //Vector3 velocityVector = normalizedVector * t.moveSpeed;

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
    }

    public override void exit(Enemy t)
    {

    }
}
