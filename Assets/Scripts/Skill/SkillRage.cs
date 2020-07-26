using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillRage : Skill
{
    public string SkillRageBuffResPath;
    public override void RunningSkillOnceBody()
    {
        base.RunningSkillOnceBody();
        PersistentStateTargetType targetType;
        if (skillConfig.castorIsEnemy)
        {
            targetType = PersistentStateTargetType.enemy;
        }
        else
        {
            targetType = PersistentStateTargetType.player;
        }
        Buff buff= Instantiate(Resources.Load(SkillRageBuffResPath) as GameObject, Vector3.zero, Quaternion.identity).GetComponent<Buff>().BuffLoad(skillConfig.castor, targetType);

    }

}
