using SuperTiled2Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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

        GameObject[] peakPositions = GameObject.FindGameObjectsWithTag("Trap");
        foreach (GameObject peakPosition in peakPositions)
        {
            SuperCustomProperties scp = peakPosition.GetComponent<SuperCustomProperties>();
            if (scp != null)
            {
                CustomProperty cp;
                if (scp.TryGetCustomProperty("TrapName", out cp))
                {
                    string peakName = cp.m_Value;
                    Trap trap = Instantiate((GameObject)Resources.Load("Trap/" + peakName), peakPosition.transform.position, Quaternion.identity).GetComponent<Trap>();
                    trap.transform.parent = peakPosition.GetComponentInParent<BlockController>().gameObject.transform;
                    AdjustingTileOffset(trap.transform);
                }
                Destroy(peakPosition);
            }
        }

        GameObject[] buffTilePositions = GameObject.FindGameObjectsWithTag("BuffTile");
        foreach (GameObject buffTilePosition in buffTilePositions)
        {
            SuperCustomProperties scp = buffTilePosition.GetComponent<SuperCustomProperties>();
            if (scp != null)
            {
                CustomProperty cp;
                if (scp.TryGetCustomProperty("BuffTileName", out cp))
                {
                    string BuffTileName = cp.m_Value;
                    IncreamSpeedBuffTile increamSpeedBuffTile = Instantiate((GameObject)Resources.Load("BuffTile/" + BuffTileName), buffTilePosition.transform.position, Quaternion.identity).GetComponent<IncreamSpeedBuffTile>();
                    increamSpeedBuffTile.transform.parent = buffTilePosition.GetComponentInParent<BlockController>().gameObject.transform;
                    AdjustingTileOffset(increamSpeedBuffTile.transform);
                    if (scp.TryGetCustomProperty("BuffSpeedIncrementPresent", out cp))
                    {
                        increamSpeedBuffTile.buffName = cp.m_Value;
                    }
                    Destroy(buffTilePosition);
                }

            }
        }

        GameObject[] obstaclePositions = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject obstaclePosition in obstaclePositions)
        {
            SuperCustomProperties scp = obstaclePosition.GetComponent<SuperCustomProperties>();
            if (scp != null)
            {
                CustomProperty cp;
                if (scp.TryGetCustomProperty("ObstacleName", out cp))
                {
                    string ObstacleName = cp.m_Value;
                    Obstacle obstacle = Instantiate((GameObject)Resources.Load("Obstacle/" + ObstacleName), obstaclePosition.transform.position, Quaternion.identity).GetComponent<Obstacle>();
                    obstacle.transform.parent = obstaclePosition.GetComponentInParent<BlockController>().gameObject.transform;
                    AdjustingTileOffset(obstacle.transform);
                    Destroy(obstaclePosition);
                }
            }
            PhysicsMaterial2D pMaterial2D = (PhysicsMaterial2D)Resources.Load("ZeroFrictionPhysicsMaterial2D");
            Collider2D[] collider2Ds = GameObject.FindObjectsOfType<Collider2D>();
            foreach (Collider2D collider2D in collider2Ds)
            {
                if(collider2D.gameObject.layer == LayerMask.NameToLayer("Wall"))
                {
                    collider2D.sharedMaterial = pMaterial2D;
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
