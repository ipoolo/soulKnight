using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;

public enum BlockDireciton
{
    Top,
    Right,
    Bottom,
    Left
}

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
    public int blockWidth = 15;
    private int blockDirectionOffset;
    public void Start()
    {
        Invoke("test", 2);
        blockDirectionOffset = Mathf.FloorToInt(blockWidth / 2.0f - 0.5f);
        ConfigChannel();
    }

    // Update is called once per frame
    public void Update()
    {
        
    }

    //TEST findAllChannel
    private void ConfigChannel()
    {
        GameObject[] gameOBs = GameObject.FindGameObjectsWithTag("Channel");
        foreach (GameObject gb in gameOBs)
        {
            gb.AddComponent<ChannelWallController>().configChannel();

        }
    }


    void test()
    {
        List<BlockDireciton> directions = new BlockDireciton[2]{ BlockDireciton.Bottom, BlockDireciton.Right }.ToList<BlockDireciton>(); ;

        //通知方向墙壁 根据链接设置判定开关哪些通道的墙壁
        ChannelWallController[] ccs=GetComponentsInChildren<ChannelWallController>();
        foreach (ChannelWallController cc in ccs)
        {
            foreach (BlockDireciton direction in directions) 
            {
                if(cc.receiveChannelControl(direction, false))
                {
                    Vector2 posation = Vector2.zero;
                    Vector2 blockPosation = transform.position;
                    //生成门
                    switch (direction)
                    {
                        case BlockDireciton.Top:
                            posation = new Vector2(blockPosation.x,blockPosation.y + blockDirectionOffset);
                            break;
                        case BlockDireciton.Right:
                            posation = new Vector2(blockPosation.x + blockDirectionOffset, blockPosation.y);
                            break;
                        case BlockDireciton.Bottom:
                            posation = new Vector2(blockPosation.x, blockPosation.y - blockDirectionOffset);
                            break;
                        case BlockDireciton.Left:
                            posation = new Vector2(blockPosation.x - blockDirectionOffset , blockPosation.y);
                            break;
                    }
                    DoorAreaController dac = ((GameObject)Instantiate(Resources.Load("Wall/DoorArea/DoorArea"), posation, Quaternion.identity)).GetComponent<DoorAreaController>() ;
                    dac.configDoorArea(direction);
                    dac.transform.parent = transform;
                    break;
                }
            }
        };
    }

    public virtual void receivePlayerEnter()
    {
    }
}
