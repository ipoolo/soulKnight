using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    public string attackBodyPrefabName;
    public float attackBodyOffset = 2.0f;
    //剑气    

    protected new void Awake()
    {
        base.Awake();
        isCloseInWeapon = true;
    }
    protected new void Start()
    {
        base.Start();
       
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }
    private SwordAttack sa;
    public override void AttackBody()
    {
        //这里用动画用fire
        Debug.Log("Sword.positon"+transform.position);
    }

    protected override void AnimFireCallBackBody()
    {
        base.AnimFireCallBackBody();
        //创建剑气 并且赋值给剑气
        Vector2 position = transform.position;
        Vector2 tmpOffset = transform.parent.right * attackBodyOffset; // 使用weaponPoint的right 因为自己的在执行动画 改了了方向
        position = position + tmpOffset;
        sa = Instantiate((GameObject)Resources.Load("Weapon/AttackBody/" + attackBodyPrefabName), position, transform.parent.rotation).GetComponent<SwordAttack>();
        sa.configSwordAttack(this);
        Invoke("AttackBodyAnimStart", 0);
    }

    public void AttackBodyAnimStart()
    {
        sa.Fire();
    }
}
