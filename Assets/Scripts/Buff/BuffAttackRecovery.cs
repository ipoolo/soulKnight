using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BuffAttackRecovery : Buff, BuffReceiveHittingDamageInterFace
{

    public float attackRecoveryScale;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public float BuffReceiveHittingDamageInterFaceBody(float _hittingDamage)
    {
        float damage = _hittingDamage;
        attackRecoveryScale = Mathf.Clamp(attackRecoveryScale, 0, float.MaxValue);
        int recoveryHp = Mathf.FloorToInt(damage * attackRecoveryScale);
        //switch (targetType)
        //{
        //    case PersistentStateTargetType.enemy:
        //        enemy.RestoreHealth(recoveryHp);
        //        break;
        //    case PersistentStateTargetType.player:
        //        playerStateController.RestoreHealth(recoveryHp);
        //        break;
        //}
        targetNpc.RestoreHealth(recoveryHp);

        return damage;

    }
}
