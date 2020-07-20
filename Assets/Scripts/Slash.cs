using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : Weapon 
{
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

    public new void Attack()
    {
        base.Attack();
        
    }

    private new void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
    }


}
