using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHealthVial : Item
{
    public bool isUsed = false;
    public NPC user;
    public int restoreHealthValue;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        this.interactionBodyAction = new System.Action(UseItem);
        user = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<NPC>();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }

    private void UseItem()
    {
        if (!isUsed)
        {
            isUsed = true;
            user.RestoreHealth(restoreHealthValue);
            Destroy(this.GetComponentInParent<SpriteRenderer>().gameObject);
        }
    }


}
