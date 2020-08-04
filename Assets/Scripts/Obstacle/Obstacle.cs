using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : NPC
{
    private void Awake()
    {
        isRegister = false;
    }
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        canReceiveRepel = false;
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();

    }

    protected override bool ReceiveDamageWithRepelVectorBody(float _damage, Vector3 _repelVector)
    {
        bool isDeath = false;
        isDeath = ReduceHealthBody(_damage);
        return isDeath;

    }

    protected bool ReduceHealthBody(float _reduceValue)
    {
        bool isDeath = false;
        int floorValue = Mathf.FloorToInt(_reduceValue);
        health -= floorValue;
        if(health <= 0)
        {
            isDeath = true;
        }
        return isDeath;

    }

    protected override void DeathBody()
    {
        base.DeathBody();
        Instantiate(Resources.Load("BulletBomb"), transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
