﻿using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Rendering;

enum DirectionType{
    up = 0,
    right,
    down,
    left
}

public struct line
{
    public MapBlockInfo start;
    public MapBlockInfo end;
}



public class BlockManager : MonoBehaviour
{
    // Start is called before the first frame update

    private int maxCount;
    private int endStep;
    private int bossDefaultEndStep = 5;
    private int generalDefaultEndStep = 4;
    private bool isBossMap = false;
    private int mapMaxBlock = 8;
    private int minMaxBlock = 6;

    private GameObject mapLocation;

    private float blockStepDistance = 1.0f;

    public List<MapBlockInfo> nodeList = new List<MapBlockInfo>();
    public List<MapBlockInfo> keyList = new List<MapBlockInfo>();

    public List<line> lineList = new List<line>();

    public List<Location> canAddNodeLocationList = new List<Location>();

    public float blockInterval;
    public GameObject channelLayer;

    private void Awake()
    {

        ConfigDefault();
        ConfigMap();
        
    }
    private void ConfigMap()
    {
        MapBlockInfo mapInfo = SpwanMapBlockInfo(0, 0, MapBlockType.startType);
        nodeList.Add(mapInfo);

        while (nodeList.Count < endStep)
        {
            AddNext(nodeList[nodeList.Count - 1]);
        }

        if (nodeList.Count == endStep)
        {
            ConfigKeyNodeLink();

            if (isBossMap)
            {
                nodeList[nodeList.Count - 1].blockType = MapBlockType.BossType;
            }
            else
            {
                nodeList[nodeList.Count - 1].blockType = MapBlockType.endType;
            }
        }


        FindLocationsCanAddLocations();
        AddShopAndEvent();

        if (nodeList.Count < maxCount)
        {
            Location randomLocation = canAddNodeLocationList[Random.Range(0, canAddNodeLocationList.Count - 1)];
            MapBlockInfo mapInfoShop = SpwanMapBlockInfo(randomLocation.x, randomLocation.y, MapBlockType.battleType);
            nodeList.Add(mapInfoShop);
            canAddNodeLocationList.Remove(randomLocation);
        }

        ConfigElseNodeLink();
        ConfigMiniMapColor();

        ConfigBlock();
        ConfigChannelComponent();
        ConfigChannel();
    }

    void Start()
    {

        
    }

    public void ConfigBlock()
    {
        nodeList.ForEach(mbi => {
            GameObject prefeb = null;
            string Path = "Block/";
            switch (mbi.blockType)
            {
                case MapBlockType.battleType:
                    prefeb = (GameObject)Resources.Load(Path+"BattleBlockMap");
                    break;
                case MapBlockType.eventType:
                    prefeb = (GameObject)Resources.Load(Path + "EventBlockMap");
                    break;
                case MapBlockType.endType:
                    prefeb = (GameObject)Resources.Load(Path + "EndBlockMap");
                    break;
                case MapBlockType.shopType:
                    break;
                case MapBlockType.startType:
                    prefeb = (GameObject)Resources.Load(Path + "StartBlockMap");
                    break;
                case MapBlockType.BossType:
                    break;

            }
            if(prefeb != null) { 
                BlockController bc = Instantiate(prefeb, Vector2.zero, Quaternion.identity).GetComponent<BlockController>();
                bc.transform.parent = transform;
                Vector2 position = new Vector2(blockInterval * mbi.indexInfo.x- bc.blockWidth/2.0f, blockInterval * mbi.indexInfo.y + bc.blockWidth / 2.0f);
                CalBlockDirection(mbi, bc);
                mbi.bc = bc;
                if(bc is BattleBlockController)
                {
                    BattleBlockController bbc = (BattleBlockController)bc;
                    //bbc.baseEnemyNum = Random.Range(10, 15);
                    //bbc.maxStepSpwanNum = Random.Range(3, 5);
                }
                bc.transform.position = position;
            }
        });
    }

    //判断mbi的方向
    public void CalBlockDirection(MapBlockInfo mbi, BlockController bc)
    {

        foreach(line line in lineList){ 
            if (line.start == mbi)
            {
                if (line.start.indexInfo.x > line.end.indexInfo.x)
                {
                    bc.directions.Add(BlockDireciton.Left);
                    continue;
                }
                if (line.start.indexInfo.x < line.end.indexInfo.x)
                {
                    bc.directions.Add(BlockDireciton.Right);
                    continue;
                }
                if (line.start.indexInfo.y > line.end.indexInfo.y)
                {
                    bc.directions.Add(BlockDireciton.Bottom);
                    continue;
                }
                if (line.start.indexInfo.y < line.end.indexInfo.y)
                {
                    bc.directions.Add(BlockDireciton.Top);
                    continue;
                }
            }
            if (line.end == mbi)
            {
                if (line.start.indexInfo.x > line.end.indexInfo.x)
                {
                    bc.directions.Add(BlockDireciton.Right);
                    continue;
                }
                if (line.start.indexInfo.x < line.end.indexInfo.x)
                {
                    bc.directions.Add(BlockDireciton.Left);
                    continue;
                }
                if (line.start.indexInfo.y > line.end.indexInfo.y)
                {
                    bc.directions.Add(BlockDireciton.Top);
                    continue;
                }
                if (line.start.indexInfo.y < line.end.indexInfo.y)
                {
                    bc.directions.Add(BlockDireciton.Bottom);
                    continue;
                }
            }
        }
        
    }

    private void ConfigChannel()
    {
        lineList.ForEach(line =>
        {
            //临时
            if (line.start.bc != null && line.end.bc != null) { 
                Vector2 channelStart = new Vector2();
                Vector2 channelEnd = new Vector2();
                GameObject upDownPreFabChannel = (GameObject)Resources.Load("Block/ChannelTileUpDown");
                GameObject leftRightreFabChannel = (GameObject)Resources.Load("Block/ChannelTileLeftRight");

                if (line.start.indexInfo.x > line.end.indexInfo.x)
                {
                    //start 左开口  end 右开口
                    channelStart = new Vector2(line.start.bc.transform.position.x - 0.5f, line.start.bc.transform.position.y - line.start.bc.blockWidth / 2.0f);
                    channelEnd = new Vector2(line.end.bc.transform.position.x + line.end.bc.blockWidth + 0.5f, line.end.bc.transform.position.y - line.end.bc.blockWidth / 2.0f);
                    int channelCount = Mathf.FloorToInt(Mathf.Abs(channelEnd.x - channelStart.x) + 1);
                    for (int i = 0; i < channelCount; i++)
                    {
                        Vector2 position = new Vector2(channelStart.x - i, channelStart.y);
                        Instantiate(leftRightreFabChannel, position, Quaternion.identity).transform.parent = channelLayer.transform;
                    }


                }
                if (line.start.indexInfo.x < line.end.indexInfo.x)
                {
                    //start 右开口  end 左开口
                    channelStart = new Vector2(line.start.bc.transform.position.x + line.start.bc.blockWidth + 0.5f, line.start.bc.transform.position.y - line.start.bc.blockWidth / 2.0f);
                    channelEnd = new Vector2(line.end.bc.transform.position.x - 0.5f, line.end.bc.transform.position.y - line.end.bc.blockWidth / 2.0f);
                    int channelCount = Mathf.FloorToInt(Mathf.Abs(channelEnd.x - channelStart.x) + 1);
                    for (int i = 0; i < channelCount; i++)
                    {
                        Vector2 position = new Vector2(channelStart.x + i, channelStart.y);
                        Instantiate(leftRightreFabChannel, position, Quaternion.identity).transform.parent = channelLayer.transform;
                    }
                }
                if (line.start.indexInfo.y > line.end.indexInfo.y)
                {
                    //start下开口 end上开口
                    channelStart = new Vector2(line.start.bc.transform.position.x + line.start.bc.blockWidth / 2.0f, line.start.bc.transform.position.y - line.start.bc.blockWidth - 0.5f);
                    channelEnd = new Vector2(line.end.bc.transform.position.x + line.end.bc.blockWidth / 2.0f, line.end.bc.transform.position.y + 0.5f);
                    int channelCount = Mathf.FloorToInt(Mathf.Abs(channelEnd.y - channelStart.y) + 1);
                    for (int i = 0; i < channelCount; i++)
                    {
                        Vector2 position = new Vector2(channelStart.x, channelStart.y - i);
                        Instantiate(upDownPreFabChannel, position, Quaternion.identity).transform.parent = channelLayer.transform;
                    }
                }
                if (line.start.indexInfo.y < line.end.indexInfo.y)
                {
                    //start上开口 end下开口
                    channelStart = new Vector2(line.start.bc.transform.position.x + line.start.bc.blockWidth / 2.0f, line.start.bc.transform.position.y + 0.5f);
                    channelEnd = new Vector2(line.end.bc.transform.position.x + line.end.bc.blockWidth / 2.0f, line.end.bc.transform.position.y - line.end.bc.blockWidth - 0.5f);
                    int channelCount = Mathf.FloorToInt(Mathf.Abs(channelEnd.y - channelStart.y) + 1);
                    for (int i = 0; i < channelCount; i++)
                    {
                        Vector2 position = new Vector2(channelStart.x, channelStart.y + i);
                        Instantiate(upDownPreFabChannel, position, Quaternion.identity).transform.parent = channelLayer.transform;
                    }
                }
            }

        });
    } 

    private void ConfigChannelComponent()
    {
        GameObject[] gameOBs = GameObject.FindGameObjectsWithTag("ChannelWall");
        foreach (GameObject gb in gameOBs)
        {
            gb.AddComponent<ChannelWallController>().configChannel();

        }
    }

    public void ConfigKeyNodeLink()
    {
        keyList = new List<MapBlockInfo>();
            nodeList.ForEach(i => keyList.Add(i));
            keyList.ForEach(mbi => {
                if (mbi.parent) { 
                    line line = new line();
                    line.start = mbi.parent;
                    line.end = mbi;
                    lineAddWithMapBlockInfo(mbi, mbi.parent);
                }
            });
    }

    public void ConfigElseNodeLink()
    {
        List<MapBlockInfo> elseList = nodeList.GetRange(endStep, nodeList.Count - endStep);
        //取出后加入的节点
        //判断这些节点与哪些节点连接相连的说明2个位置点的距离等于1
        elseList.ForEach(elseMbi =>
        {
            nodeList.ForEach(nodeMbi => { 
                if(nodeMbi.blockType==MapBlockType.eventType|| nodeMbi.blockType == MapBlockType.battleType|| nodeMbi.blockType == MapBlockType.shopType)
                {
                   float distance =Mathf.Abs(Vector2.Distance(new Vector2(elseMbi.indexInfo.x, elseMbi.indexInfo.y), new Vector2(nodeMbi.indexInfo.x, nodeMbi.indexInfo.y)));
                    if (distance == blockStepDistance)
                    {
                        lineAddWithMapBlockInfo(elseMbi, nodeMbi);
                    }
                }
            });
        });
    }

    public void lineAddWithMapBlockInfo(MapBlockInfo start,MapBlockInfo end)
    {
        bool isExist = false;
        //验重
        lineList.ForEach(line =>
        {
              if(line.start == start && line.end == end)
            {
                isExist = true;
            }
            else if (line.start == start && line.end == end)
            {
                isExist = true;
            }
        });
        if (!isExist)
        {
            line newLine = new line();
            newLine.start = start;
            newLine.end = end;
            lineList.Add(newLine);
        }
    }

    public void ConfigMiniMapColor()
    {
        foreach (MapBlockInfo mbi in nodeList)
        {

            switch (mbi.blockType)
            {
                case MapBlockType.startType:
                    mbi.GetComponent<SpriteRenderer>().color = Color.green;
                    break;
                case MapBlockType.endType:
                    mbi.GetComponent<SpriteRenderer>().color = Color.blue;
                    break;
                case MapBlockType.battleType:
                    mbi.GetComponent<SpriteRenderer>().color = new Color(248 / 255.0f, 159.0f/255.0f, 51.0f / 255.0f, 1.0f);
                    break;
                case MapBlockType.shopType:
                    mbi.GetComponent<SpriteRenderer>().color = Color.yellow;
                    break;
                case MapBlockType.BossType:
                    mbi.GetComponent<SpriteRenderer>().color = Color.red;
                    break;
                case MapBlockType.eventType:
                    mbi.GetComponent<SpriteRenderer>().color = new Color(255 / 255.0f, 0.0f, 237.0f / 255.0f, 1.0f);
                    break;
            }
        }

        lineList.ForEach(line => {
            ChannelLine cline = ((GameObject)Instantiate(Resources.Load("Block/Line"), transform.position, Quaternion.identity)).GetComponent<ChannelLine>();
            cline.config(new Vector2(line.start.indexInfo.x, line.start.indexInfo.y), new Vector2(line.end.indexInfo.x, line.end.indexInfo.y));
            cline.transform.parent = mapLocation.transform;
        });
    }
    public void ConfigDefault()
    {
        if(channelLayer == null)
        {
            channelLayer = new GameObject();
            channelLayer.transform.position = transform.position;
            channelLayer.transform.parent = transform;

        } 

        mapLocation = GameObject.FindGameObjectWithTag("MapLocation");

        if (isBossMap)
        {
            endStep = bossDefaultEndStep;
        }
        else
        {
            endStep = generalDefaultEndStep;
        }
        maxCount = Mathf.Clamp(minMaxBlock, mapMaxBlock, Random.Range(0, 2) + endStep + 2);//2是商店加事件块
        //地图只能生成6到8个节点
    }

    public void AddShopAndEvent()
    {
        Location randomLocation = canAddNodeLocationList[Random.Range(0, canAddNodeLocationList.Count - 1)];
        MapBlockInfo mapInfoShop = SpwanMapBlockInfo(randomLocation.x, randomLocation.y, MapBlockType.shopType);
        nodeList.Add(mapInfoShop);
        canAddNodeLocationList.Remove(randomLocation);
        randomLocation = canAddNodeLocationList[Random.Range(0, canAddNodeLocationList.Count - 1)];
        MapBlockInfo mapInfoEvent = SpwanMapBlockInfo(randomLocation.x, randomLocation.y, MapBlockType.eventType);
        nodeList.Add(mapInfoEvent);
        canAddNodeLocationList.Remove(randomLocation);
    }

    public void AddNext(MapBlockInfo _mapBlockInfo)
    {

        int tempX = 0;
        int tempY = 0;
        bool IsCalculate = true;

        while (IsCalculate) {
            tempX = (int)_mapBlockInfo.indexInfo.x;
            tempY = (int)_mapBlockInfo.indexInfo.y;
            int DirectionIndex = Random.Range(0, 4);
            switch (DirectionIndex)
            {
                case (int)DirectionType.up:
                    tempY = tempY + 1;
                    break;
                case (int)DirectionType.right:
                    tempX = tempX + 1;
                    break;
                case (int)DirectionType.down:
                    tempY = tempY - 1;
                    break;
                case (int)DirectionType.left:
                    tempX = tempX - 1;
                    break;
            }
            if(CheckNewNodeValid(tempX, tempY)&&!CheckNewNodeExist(tempX, tempY))
            {
                IsCalculate = false;
                MapBlockInfo mapInfo = SpwanMapBlockInfo(tempX,tempY,MapBlockType.battleType);
                mapInfo.parent = _mapBlockInfo;
                nodeList.Add(mapInfo);

            }
        }

        //colorful 
    }

    public MapBlockInfo SpwanMapBlockInfo(int x, int y , MapBlockType type)
    {

        GameObject gb = (GameObject)Instantiate(Resources.Load("Block/MapBlock"), transform.position, Quaternion.identity);
        MapBlockInfo mapInfo = gb.GetComponent<MapBlockInfo>();
        mapInfo.indexInfo.x = x;
        mapInfo.indexInfo.y = y;
        mapInfo.blockType = type;
        mapInfo.transform.position = new Vector2(mapInfo.indexInfo.x,mapInfo.indexInfo.y);
        mapInfo.transform.parent = mapLocation.transform;
        return mapInfo;
    }

    public bool CheckNewNodeValid(int x, int y)
    {
        bool result = true;
        if( x > 2  || x < -2 || y > 2 || y < -2)
        {
            result = false;
        }
        return result;
    }

    public bool CheckNewNodeExist(int x, int y)
    {
        bool isExist = false;
        foreach (MapBlockInfo mbi in nodeList)
        {
            if(mbi.indexInfo.x == x && mbi.indexInfo.y == y)
            {
                isExist = true;
                break;
            }
        }
        return isExist;
    }

    public bool CheckLocationExistInCanAddNodeLocationList(int x, int y)
    {
        bool isExist = false;
        foreach (Location l in canAddNodeLocationList)
        {
            if (l.x == x && l.y == y)
            {
                isExist = true;
                break;
            }
        }
        return isExist;
    }

    public void FindLocationsCanAddLocations()
    {
        List<MapBlockInfo> battleList = nodeList.GetRange(1, nodeList.Count - 2);
        //拿出战斗节点 也可以遍历 查看type获得

        //判断每个battleNode 上下左右是否 空闲,空闲则加入到canAddNodeLocationList
        foreach(MapBlockInfo battleNode in battleList)
        {
            int tempX = (int)battleNode.indexInfo.x;
            int tempY = (int)battleNode.indexInfo.y;

            //up 
            AddLocation(tempX , tempY+1);
            //right
            AddLocation(tempX+1, tempY);
            //down
            AddLocation(tempX , tempY-1);
            //left
            AddLocation(tempX-1, tempY);

        }

    }

    public void AddLocation(int x, int y)
    {
        if (CheckNewNodeValid(x, y) && !CheckNewNodeExist(x, y) && !CheckLocationExistInCanAddNodeLocationList(x,y))
        {
            Location loc = new Location();
            loc.x = x;
            loc.y = y;
            canAddNodeLocationList.Add(loc);
        }
           
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}