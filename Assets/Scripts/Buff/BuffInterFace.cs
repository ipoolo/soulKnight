using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface BuffInterFace
{
    List<Buff> getBuffList();
}

interface BuffReceiveHittingDamageInterFace
{
    float BuffReceiveHittingDamageInterFaceBody(float _hittingDamage);
    //返回值为处理后的伤害
}
interface BuffReceiveHittedDamageInterFace
{
    float BuffReceiveHittedDamageInterFaceBody(float _hittedDamage);
    //返回值为处理后的伤害
}
