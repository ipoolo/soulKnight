using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffDefenseReduce : Buff,BuffReceiveHittedDamageInterFace
{
    public float DefenseReduceScale;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float BuffReceiveHittedDamageInterFaceBody(float _hittedDamage)
    {
        float damage = _hittedDamage;
        DefenseReduceScale = Mathf.Clamp(DefenseReduceScale, 0, 1);
        int afterDefenseReduceDamage = Mathf.FloorToInt(damage * DefenseReduceScale);
        return afterDefenseReduceDamage;
    }
}
