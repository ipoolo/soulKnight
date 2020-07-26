using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffAttackIncrementPresent : Buff
{
    public float incrementPresent;
    private float incrementValue;
    public override bool BuffLoadBodyAndIsInvokeBody()
    {
        bool isInvoke = true;
        switch (targetType)
        {
            case PersistentStateTargetType.enemy:
                incrementValue = enemy.moveSpeed * incrementPresent;
                enemy.moveSpeed += incrementValue;
                break;
            case PersistentStateTargetType.player:
                incrementValue = playerStateController.moveSpeed * incrementPresent;
                playerStateController.moveSpeed += incrementValue;
                break;
            case PersistentStateTargetType.noTarget:
                break;
        }
        return isInvoke;
    }

    public override void BuffUnLoadBody()
    {
        switch (targetType)
        {
            case PersistentStateTargetType.enemy:
            enemy.moveSpeed -= incrementValue;
            break;
            case PersistentStateTargetType.player:
            playerStateController.moveSpeed -= incrementValue;
            break;
            case PersistentStateTargetType.noTarget:
            break;
        }
    }
}
