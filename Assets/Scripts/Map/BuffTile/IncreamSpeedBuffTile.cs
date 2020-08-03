using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreamSpeedBuffTile : EffectTile
{
    public string buffName;
    // Start is called before the first frame update
    new void Start()
    {

    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }
    private void LanuchBuff(NPC npc)
    {
        Buff buff = Instantiate((GameObject)Resources.Load("Buff/" + buffName)).GetComponentInChildren<Buff>();
        buff.BuffLoad(npc);
    }

    public override void EffectBody(NPC npc)
    {
        base.EffectBody(npc);
        LanuchBuff(npc);
    }

}


