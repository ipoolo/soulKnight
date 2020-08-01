using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadStone : Item
{
    private Animator headStoneAnimator;
    private bool isUsed;

    public void Start()
    {

        base.Start();
        DefaultConfig();
    }

    private void DefaultConfig()
    {
        headStoneAnimator = GetComponentInParent<Animator>();
        this.interactionBodyAction = new System.Action(UseStone);
        offset = new Vector2(-0.5f, 2.1f);
        isUsed = false;
    }

    new void Update()
    {
        base.Update();
    }

    public void UseStone()
    {

        if (!isUsed)
        {
            isUsed = true;
            headStoneAnimator.SetTrigger("Use");
            EffectBody();
            HideFoucsArror();
            hintCanShow = false;
        }
    }

    public virtual void EffectBody()
    {

    }
}
