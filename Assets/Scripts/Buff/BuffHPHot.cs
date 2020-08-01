﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffHPHot : BuffDotOrHot
{

    public int stepHealthChange;

    private SpriteRenderer targetRender;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }

    public override void EffectBody()
    {
        base.EffectBody();
        targetNpc.RestoreHealth(stepHealthChange);
    }

}
