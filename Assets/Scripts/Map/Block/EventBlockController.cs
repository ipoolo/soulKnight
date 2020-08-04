using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventBlocType
{
    RestoreHealthHeadStone,
}

public class EventBlockController : BlockController
{
    public EventBlocType eventBlockType;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        ConfigDefault();
    }

    private void ConfigDefault()
    {
        ConfigEventItem();
    }

    private void ConfigEventItem()
    {
        switch (eventBlockType)
        {
            case EventBlocType.RestoreHealthHeadStone:
                SpwanRestoreHealthHeadStone();
                break;
        }
    }

    private void SpwanRestoreHealthHeadStone()
    {
        Vector2 position = new Vector2(transform.position.x + blockWidth/2.0f , transform.position.y - blockWidth / 2.0f);
        GameObject headStone = Instantiate((GameObject)Resources.Load("Item/RestoreHealth"),position,Quaternion.identity);
        headStone.transform.parent = transform;
    }


    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }
}
