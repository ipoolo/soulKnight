using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FSMEnemyStatePatrol : FSMState<Enemy>
{
    private static FSMEnemyStatePatrol _singleInstance;
    public static FSMEnemyStatePatrol singleInstance {
        get
        {
            if (_singleInstance == null)
            {
                _singleInstance = new FSMEnemyStatePatrol();
            }
            return _singleInstance;
        }
        set
        {

        }
    }
    //state作为工具类使用 不带参数,从而使得其可以单例,纯工具

    //TODO这样的单例可能有线程安全问题,后期关注下


    public override void Enter(Enemy t)
    {
        t.CalculateNewTarget(t);
        t.patrolTimer = 0;
    }

    public override void Execute(Enemy t)
    {
        if (t.isPatrolRunning)
        {
            t.patrolTimer += Time.deltaTime;
            //超时或者到达都进入休息，并且寻找新目标(防止目标为不可抵达)
            Vector3 targetVector = t.patrolTargetPosition - t.transform.position;
            Vector3 normalizedVector = targetVector.normalized;
            Vector3 velocityVector = normalizedVector * t.moveSpeed;
            t.rigid2d.velocity = velocityVector;

            if (((t.patrolTargetPosition - t.transform.position).magnitude < 0.2f )||( t.patrolTimer >= t.patrolTime))
            {
                //等待 然后新目标
                t.isPatrolRunning = false;
                t.patrolTimer = 0;
                t.rigid2d.velocity = Vector3.zero;
                t.StartCoroutine("BackToPatrol",t);
            }
        }
        else
        {
            //巡逻休息时
            t.rigid2d.velocity = Vector3.zero;
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
