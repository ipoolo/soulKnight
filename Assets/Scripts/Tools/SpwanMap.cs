using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor.SceneManagement;
using UnityEngine;

public class MapLoction
{
    public int i;
    public int j;
    public MapLoction parent;
    public bool isColsed = false;
    public bool isStartAndFinishLick;

    public MapLoction(int _i, int _j)
    {
        i = _i;
        j = _j;
    }
    public string toString()
    {
        return string.Format("{0},{1}", i, j);
    }
}

public class SpwanMap : MonoBehaviour
{
    List<string> linkStringList = new List<string>();
    MapLoction[][] mapNameArray = new MapLoction[3][];
    List<MapLoction> openpLoctionList = new List<MapLoction>();
    List<MapLoction> closeLoctionList = new List<MapLoction>();

    private bool shouldFindPath = true;

    MapLoction startLoction;
    // Start is called before the first frame update
    void Start()
    {
        mapNameArray[0] = new MapLoction[3] { new MapLoction(0,0), new MapLoction(0, 1), new MapLoction(0, 2) };
        mapNameArray[1] = new MapLoction[3] { new MapLoction(1, 0), new MapLoction(1, 1), new MapLoction(1, 2) };
        mapNameArray[2] = new MapLoction[3] { new MapLoction(2, 0), new MapLoction(2, 1), new MapLoction(2, 2) };

        //随机抽2个 弄死
        randomKill2();
        randomStartPoint();
        configOpenList();

        //计算开始

        Debug.Log("open_count"+ openpLoctionList.Count);
        Debug.Log("close_count"+ closeLoctionList.Count);
        
        while (shouldFindPath)
        {
            MapLoction next = null;
            Debug.Log("b");
            foreach (MapLoction loc in closeLoctionList)
            {
                
                if (getNeighborByStr(loc, out next))
                {
                    break;
                }

            }
            if (next!= null) {
             openpLoctionList.Remove(next);
             closeLoctionList.Add(next);
            }
            if (openpLoctionList.Count <= 0)
            {
                shouldFindPath = false;
            }
        }



        Debug.Log("e");
        foreach (MapLoction loc in closeLoctionList)
        {
            if(loc == startLoction)
            {
                Debug.LogFormat("Start -> {0}", loc.toString());
            }
            else { 
                Debug.LogFormat("{0} -> {1}",loc.parent.toString(), loc.toString());
            }
        }
}
 
    public bool getNeighborByStr(MapLoction location, out MapLoction next)
    {
        Debug.Log("getNeighborByStr");
        bool result = false;
        int i = location.i;
        int j = location.j;

        //上
        if (i - 1 >= 0)
        {
            next = mapNameArray[i - 1][j];
            if (openpLoctionList.Contains(next)) { 
                next.parent = location;

                result = true;
                return result;
            }
        }
        //右
        if (j + 1 <= 2)
        {
            next = mapNameArray[i][j+1];
            if (openpLoctionList.Contains(next))
            {
                next.parent = location;

                result = true;
                return result;
            }
        }
        //下

        if (i + 1 <= 2)
        {
            next = mapNameArray[i + 1][j];
            if (openpLoctionList.Contains(next))
            {
                next.parent = location;

                result = true;
                return result;
            }
        }
        //左

        if (j - 1 >= 0)
        {
            next = mapNameArray[i][j-1];
            if (openpLoctionList.Contains(next))
            {
                next.parent = location;

                result = true;
                return result;
            }
        }
        next = null;
        return result;
    }

    public void randomKill2(){
        bool isCalculate = true;
        MapLoction firstClosedMapLoction = null;
        while (isCalculate) {
            int index  = Random.Range(0, 9999999) % 9;
            if (index != 4 )
            {
                if (!mapNameArray[index/3][index%3].isColsed)
                {
                    mapNameArray[index / 3][index % 3].isColsed = true;
                    isCalculate = false;
                    firstClosedMapLoction = mapNameArray[index / 3][index % 3];
                    Debug.Log("kill" + index);
                }
                
            }
        }

        isCalculate = true;
        while (isCalculate)
        {
            int secondIndex = Random.Range(0, 9999999) % 9;
            if (secondIndex != 4)
            {
                if (!mapNameArray[secondIndex / 3][secondIndex % 3].isColsed)
                {
                    if ((firstClosedMapLoction.i - 1 == secondIndex / 3) && (firstClosedMapLoction.j - 1 == secondIndex / 3)||
                        (firstClosedMapLoction.i - 1 == secondIndex / 3) && (firstClosedMapLoction.j + 1 == secondIndex / 3)||
                        (firstClosedMapLoction.i + 1 == secondIndex / 3) && (firstClosedMapLoction.j - 1 == secondIndex / 3)||
                        (firstClosedMapLoction.i + 1 == secondIndex / 3) && (firstClosedMapLoction.j + 1 == secondIndex / 3)) { 

                    }
                    else
                    {
                        mapNameArray[secondIndex / 3][secondIndex % 3].isColsed = true;
                        isCalculate = false;
                        Debug.Log("kill" + secondIndex);
                    }
                }

            }
        }


    }

    public void randomStartPoint()
    {
        bool isCalculate = true;
        while (isCalculate)
        {
            int index = Random.Range(0, 9999999) % 9;
            if (index != 4 && !(mapNameArray[index / 3][index % 3].isColsed))
            {
                startLoction = mapNameArray[index / 3][index % 3];
                closeLoctionList.Add(startLoction);
                isCalculate = false;
                Debug.Log("index"+index);
            }
        }

    }

    public void configOpenList()
    {
        for(int i =0; i<3;i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (mapNameArray[i][j] == startLoction|| (mapNameArray[i][j].isColsed))
                {
                    //则不加进去
                }
                else
                {
                    openpLoctionList.Add(mapNameArray[i][j]);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}

