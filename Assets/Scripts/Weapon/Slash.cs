using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : Weapon 
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void AttackBody()
    {
        base.AttackBody();

    }

    private new void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
    }


}
