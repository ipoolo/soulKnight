using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSpeedIncrementPresent : Buff
{
    public float incrementPresent;
    private float incrementValue;
    public override bool BuffLoadBodyAndIsInvokeBody()
    {
        bool isInvoke = true;

        incrementValue = targetNpc.moveSpeed * incrementPresent;
        targetNpc.moveSpeed += incrementValue;
        return isInvoke;
    }

    public override void BuffUnLoadBody()
    {
        targetNpc.moveSpeed -= incrementValue;
    }
}
