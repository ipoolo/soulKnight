using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffHPHot : BuffDotOrHot
{

    public int stepHealthChange;
    public string effectPathStrInRes;
    public float effectColorTime;
    public Color effectColor;

    private Color originColor;
    private SpriteRenderer targetRender;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }

    public override void EffectBody()
    {
        base.EffectBody();
        //业务逻辑
        //switch (targetType)
        //{
        //    case PersistentStateTargetType.enemy:
        //        enemy.RestoreHealth(stepHealthChange);
        //        break;
        //    case PersistentStateTargetType.player:
        //        playerStateController.RestoreHealth(stepHealthChange);
        //        //TestDot
        //        //playerStateController.receiverDamage(1.0f);
        //        break;
        //}
        targetNpc.RestoreHealth(stepHealthChange);
        //效果转移到recoverHP处统一处理
        //加一点粒子效果
        //if (effectPathStrInRes.Length != 0)
        //{
        //    GameObject effectPSOb = Instantiate(Resources.Load(effectPathStrInRes) as GameObject, targetGb.transform.position, Quaternion.identity);
        //    effectPSOb.transform.parent = targetGb.transform;
        //    targetRender = targetGb.GetComponentInChildren<SpriteRenderer>();
        //    originColor = targetRender.color;
        //    targetRender.color = effectColor;
        //}
        //StartCoroutine("renderBackOriginColor");

    }

    //IEnumerator renderBackOriginColor()
    //{
    //    yield return new WaitForSeconds(effectColorTime);
    //    targetRender.color = originColor;
    //}


}
