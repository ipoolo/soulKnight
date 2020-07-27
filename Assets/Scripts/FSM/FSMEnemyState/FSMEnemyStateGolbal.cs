using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMEnemyStateGolbal : FSMState<Enemy>
{
    private static FSMEnemyStateGolbal _singleInstance;
    public static FSMEnemyStateGolbal singleInstance
    {
        get
        {
            if (_singleInstance == null)
            {
                _singleInstance = new FSMEnemyStateGolbal();
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
        t.playerPosition = t.player.transform.position;
        if ((t.playerPosition - t.transform.position).sqrMagnitude < t.senseRaidus)
        {
            //状态跳转到跟踪
            t.fsm.ChangeState(FSMEnemyStateTrack.singleInstance);


            //if (enemyStateType == EnemyStateType.enemyStatePatrol)
            //{
            //    //状态切换时
            //    skillTimerStop = false;
            //    skillCoolDownTimer = 0;

            //}
            //enemyStateType = EnemyStateType.enemyStateFollowPlayer;

            //只有跟踪玩家时才开始计算技能释放
        }
        else
        {
            t.fsm.ChangeState(FSMEnemyStatePatrol.singleInstance);
            //状态跳转到巡逻
            //enemyStateType = EnemyStateType.enemyStatePatrol;
            //skillTimerStop = true;
        }
    }

    public override void Exit(Enemy t)
    {
        throw new System.NotImplementedException();
    }

    public override bool HandleMessage(Message msg)
    {
        return false;
    }


}
