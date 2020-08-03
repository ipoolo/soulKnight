using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestoreHealthHeadStone : HeadStone
{
    public string buffName;
    new void Start()
    {
        base.Start();
    }
    new void Update()
    {
        base.Update();
    }
    public override void EffectBody()
    {
        Instantiate(Resources.Load("Buff/"+ buffName) as GameObject, Vector3.zero, Quaternion.identity).GetComponent<Buff>().BuffLoad(playerController.gameObject);
    }
}
