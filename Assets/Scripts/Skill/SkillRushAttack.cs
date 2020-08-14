using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SkillRushAttack : Skill
{
    public float rushSpeed;

    private Vector3 targetPosition;
    private Rigidbody2D castorRigidbody2d;


    public float rushMaxTime;
    private float timer;

    // Start is called before the first frame update
    public override void CastSkillOnceBody()
    {
        //bast.CastSkillOnceBody(); 因为不加载动画,所以此处不需要
    }
    public override void RunningSkillOnceBody()
    {
        targetPosition = skillConfig.targetPosition;
        castorRigidbody2d = skillConfig.castorRigidbody2d;
        timer = 0.0f;
        AudioManager.Instance.PlaySoundWithTime("Voices/Arrow", 0.0f);
    }
    private float shadowTimer;
    public override void RunningSkillUpdateBody(float castTimer)
    {
        timer += Time.deltaTime;
        Vector3 targetVector = targetPosition - transform.position;
        Vector3 normalizedVector = targetVector.normalized;
        Vector3 velocityVector = normalizedVector * rushSpeed;
        castorRigidbody2d.velocity = velocityVector;
        shadowTimer += Time.deltaTime;

        skillConfig.castor.GetComponentInChildren<NPC>().SpwanSilderShadow();

        if (shadowTimer > 0.05f)
        {
            shadowTimer = 0;
            skillConfig.castor.GetComponentInChildren<NPC>().SpwanSilderShadow();
        }
        

        //旋转动画效果
        transform.rotation *= Quaternion.FromToRotation(transform.right, velocityVector);
        if (Vector3.Distance(transform.position, targetPosition) < 0.3f)
        {
            skillStateType = SkillStateType.skillTypeFinish;
        }
        if (timer >= rushMaxTime)
        {
            skillStateType = SkillStateType.skillTypeFinish;
        }
    }

}
