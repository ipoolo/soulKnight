using SuperTiled2Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int coinNum;


    // Start is called before the first frame update
    void Start()
    {

        configDefault();


    }

    private void configDefault()
    {
        //ITEM 全局的加,代码生成的 action由自己设置
        GameObject[] gameOBs = GameObject.FindGameObjectsWithTag("Item");
        foreach (GameObject gb in gameOBs)
        {
            BoxCollider2D[] boxC2ds = gb.GetComponentsInChildren<BoxCollider2D>();
            foreach (BoxCollider2D b2d in boxC2ds)
            {
                if (b2d.name == "Hitbox")
                {
                    //查看自定义数值
                    SuperCustomProperties scp = gb.GetComponent<SuperCustomProperties>();
                    if (scp != null)
                    {
                        CustomProperty cp;
                        if (scp.TryGetCustomProperty("ItemType", out cp))
                        {
                            if (cp.m_Value.Equals("ExitPort"))
                            {
                                Item item = b2d.gameObject.AddComponent<Item>();
                                item.interactionBodyAction = ItemAction.ItemExitAction();

                                break;
                            }
                        }
                    }
                    else
                    {
                        if (b2d.GetComponent<Item>() == null)
                        {
                            Item item = b2d.gameObject.AddComponent<Item>();
                        }
                    }



                }
            }
        }

        GameObject[] peakGbs = GameObject.FindGameObjectsWithTag("Trap");
        foreach (GameObject peakGb in peakGbs)
        {
            SuperCustomProperties scp = peakGb.GetComponent<SuperCustomProperties>();
            if (scp != null)
            {
                CustomProperty cp;
                if (scp.TryGetCustomProperty("TrapName", out cp))
                {
                    string peakName = cp.m_Value;
                    Peak peak = Instantiate((GameObject)Resources.Load("Trap/" + peakName), peakGb.transform.position, Quaternion.identity).GetComponent<Peak>();
                    peak.transform.parent = peakGb.GetComponentInParent<BlockController>().gameObject.transform;
                    AdjustingTileOffset(peak.transform);
                }
            }
        }

        GameObject[] buffTileGbs = GameObject.FindGameObjectsWithTag("BuffTile");
        foreach (GameObject buffTileGb in buffTileGbs)
        {
            SuperCustomProperties scp = buffTileGb.GetComponent<SuperCustomProperties>();
            if (scp != null)
            {
                CustomProperty cp;
                if (scp.TryGetCustomProperty("BuffTileName", out cp))
                {
                    string BuffTileName = cp.m_Value;
                    IncreamSpeedBuffTile increamSpeedBuffTile = Instantiate((GameObject)Resources.Load("BuffTile/" + BuffTileName), buffTileGb.transform.position, Quaternion.identity).GetComponent<IncreamSpeedBuffTile>();
                    increamSpeedBuffTile.transform.parent = buffTileGb.GetComponentInParent<BlockController>().gameObject.transform;
                    AdjustingTileOffset(increamSpeedBuffTile.transform);
                    if (scp.TryGetCustomProperty("BuffSpeedIncrementPresent", out cp))
                    {
                        increamSpeedBuffTile.buffName = cp.m_Value;
                    }
                }

            }
        }
    }

    private void AdjustingTileOffset(Transform t)
    {
        t.position = new Vector3(t.position.x + 0.5f, t.position.y + 0.5f);
    }


    // Update is called once per frame
    void Update()
    {

    }

}
