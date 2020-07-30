using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface battleState
{
    void BattleStart();
    void BattleEnd();
}

public enum BlockType
{
    battleType,
    eventType,
    shopType,
    startType,
    endType

    
}

public class BlockController : MonoBehaviour
{
    // Start is called before the first frame update

    public BlockType blockType = BlockType.battleType;
    void Start()
    {

        Invoke("test", 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void test()
    {

        ChannelController[] ccs=GetComponentsInChildren<ChannelController>();
        foreach (ChannelController cc in ccs)
        {
            cc.receiveChannelControl(ChannelDirectionType.right,false);
        }

    }

    public void receivePlayerEnter()
    {
        if(blockType == BlockType.battleType) { 
        //这里还要判断block的类型
        gameObject.BroadcastMessage("BattleStart");
        }
    }
}
