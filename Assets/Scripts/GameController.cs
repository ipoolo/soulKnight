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
        //ITEM
        GameObject[] gameOBs = GameObject.FindGameObjectsWithTag("Item");
        foreach(GameObject gb in gameOBs)
        {
            BoxCollider2D[] boxC2ds =  gb.GetComponentsInChildren<BoxCollider2D>();
            foreach (BoxCollider2D b2d in boxC2ds)
            {
                if(b2d.name == "Hitbox")
                {

                    Item item = b2d.gameObject.AddComponent<Item>();

                    //查看自定义数值
                    SuperCustomProperties scp = gb.GetComponent<SuperCustomProperties>();
                    if(scp != null)
                    {
                        CustomProperty cp;
                        if(scp.TryGetCustomProperty("ItemType",out cp))
                        {
                            if (cp.m_Value.Equals("ExitPort"))
                            {
                                item.InteractionBodyAction = ItemAction.ItemExitAction();

                                break;
                            }
                        }
                    }


                       
                }
            }
        }
    }


        // Update is called once per frame
        void Update()
    {
        
    }

}
