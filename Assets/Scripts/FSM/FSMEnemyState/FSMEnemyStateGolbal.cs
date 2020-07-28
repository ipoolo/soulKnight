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
        t.isPerspectiveSense = false;

        if ((t.playerPosition - t.transform.position).magnitude < t.perspectiveSenseDistance)
        {
            //小于视觉距离
            Vector3 postion2Target = t.playerPosition - t.transform.position;
            float angle = Vector3.Angle(t.transform.right,postion2Target);
            RaycastHit2D hit = Physics2D.Raycast(t.transform.position, postion2Target.normalized, t.perspectiveSenseDistance, LayerMask.GetMask("Player", "Wall", "InvincibilityLayer"));

            if (hit.collider != null && hit.collider.CompareTag("Player")) { 
                if (Mathf.Abs(angle) < t.perspectiveSenseFiledOfView)
                {
                    t.isPerspectiveSense = true;
                }
            }
        }


        //DEBUG LINE
        Debug.DrawLine(t.transform.position, t.transform.position + t.transform.right.normalized * t.perspectiveSenseDistance, Color.red);
        Vector3 postion2Target2 = t.transform.right;
        postion2Target2 =  Quaternion.Euler(0, 0, t.perspectiveSenseFiledOfView) * postion2Target2;
        Debug.DrawLine(t.transform.position, new Vector3(t.transform.position.x+ postion2Target2.normalized.x *t.perspectiveSenseDistance, t.transform.position.y + postion2Target2.normalized.y * t.perspectiveSenseDistance,0), Color.green); ;
        Debug.DrawLine(t.transform.position, new Vector3(t.transform.position.x + postion2Target2.normalized.x * t.perspectiveSenseDistance, t.transform.position.y - postion2Target2.normalized.y * t.perspectiveSenseDistance, 0), Color.yellow); ;

        if (t.isTouchSensePalyer || t.isPerspectiveSense)
        {
            //状态跳转到跟踪
            t.fsm.ChangeState(FSMEnemyStateTrack.singleInstance);
        }
        else
        {
            t.isTouchSensePalyer = false;
            t.fsm.ChangeState(FSMEnemyStatePatrol.singleInstance);
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
