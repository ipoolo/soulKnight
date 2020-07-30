﻿using SuperTiled2Unity;
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
    private int blockWidth;
    private int blockDirectionOffset;
    private Vector2 blockCenterPosation;

    public void Start()
    {
        Invoke("test", 2);
        blockWidth = GetComponentInChildren<SuperMap>().m_Width;
        blockDirectionOffset = Mathf.FloorToInt(blockWidth / 2.0f - 0.5f);
        blockCenterPosation = new Vector2(transform.position.x + blockWidth / 2.0f, transform.position.y - blockWidth / 2.0f);
        Debug.Log("blockWidth"+ blockWidth);
        ConfigChannel();
    }

    // Update is called once per frame
    public void Update()
    {
        
    }

    //TEST findAllChannel
    private void ConfigChannel()
    {
        GameObject[] gameOBs = GameObject.FindGameObjectsWithTag("ChannelWall");
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

                    //生成门
                    switch (direction)
                    {
                        case BlockDireciton.Top:
                            posation = new Vector2(blockCenterPosation.x,blockCenterPosation.y + blockDirectionOffset);
                            break;
                        case BlockDireciton.Right:
                            posation = new Vector2(blockCenterPosation.x + blockDirectionOffset, blockCenterPosation.y);
                            break;
                        case BlockDireciton.Bottom:
                            posation = new Vector2(blockCenterPosation.x, blockCenterPosation.y - blockDirectionOffset);
                            break;
                        case BlockDireciton.Left:
                            posation = new Vector2(blockCenterPosation.x - blockDirectionOffset , blockCenterPosation.y);
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