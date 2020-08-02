using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillRage : Skill
{
    public string SkillRageBuffName;
    public override void RunningSkillOnceBody()
    {
        base.RunningSkillOnceBody();
        Instantiate(Resources.Load("Buff/"+SkillRageBuffName) as GameObject, Vector3.zero, Quaternion.identity).GetComponent<Buff>().BuffLoad(skillConfig.castor);

    }

}
