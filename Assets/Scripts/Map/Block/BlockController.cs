﻿using SuperTiled2Unity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum BlockDireciton
{
    Top,
    Right,
    Bottom,
    Left
}

public interface IBattleState
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

    public BlockType blockType;
    [HideInInspector]public int blockWidth;
    private int blockDirectionOffset;
    private Vector2 blockCenterPosation;
    public List<BlockDireciton> directions = new List<BlockDireciton>();
    public BlockManager blockManager;
    protected bool isPlayerFirstEnter;
    public MapBlockInfo mbi;

    private void Awake()
    {
        blockWidth = GetComponentInChildren<SuperMap>().m_Width;
    }
    private void OnEnable()
    {
        isPlayerFirstEnter = true;
    }

    public void Start()
    {
        blockDirectionOffset = Mathf.FloorToInt(blockWidth / 2.0f - 0.5f);
        blockCenterPosation = new Vector2(transform.position.x + blockWidth / 2.0f, transform.position.y - blockWidth / 2.0f);
        ConfigChannleWallAndDoor();
    }

    // Update is called once per frame
    public void Update()
    {
        
    }


    void ConfigChannleWallAndDoor()
    {
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

    public virtual void ReceivePlayerEnter()
    {
        if (blockType != BlockType.startType) { 
            if (isPlayerFirstEnter) {
                
                isPlayerFirstEnter = false;
                blockManager.PlayerEnterBlock(this);
            }
        }
    
    }
}
