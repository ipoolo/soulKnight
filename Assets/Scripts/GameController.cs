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
        //TEST
        GameObject[] gameOBs = GameObject.FindGameObjectsWithTag("Item");
        foreach(GameObject gb in gameOBs)
        {
            BoxCollider2D[] boxC2ds =  gb.GetComponentsInChildren<BoxCollider2D>();
            foreach (BoxCollider2D b2d in boxC2ds)
            {
                if(b2d.name == "hitbox")
                {
                    b2d.gameObject.AddComponent<Item>();
                }
            }
        }
    }


        // Update is called once per frame
        void Update()
    {
        
    }

}
