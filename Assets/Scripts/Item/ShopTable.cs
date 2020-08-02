using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;

public class ShopTable : MonoBehaviour
{
    public GameObject sellInfoCanvas;
    public Item goods;
    public Item table;
    public TextMeshProUGUI textMesh;
    private bool isGoodsSold = false;
    // Start is called before the first frame update
    private void Awake()
    {
        sellInfoCanvas.SetActive(false);
    }

    void Start()
    {
        
    }

    public void ConfigGoods(string goodsName)
    {
        goods = Instantiate((Item)Resources.Load("Item/Goods/" + goodsName), transform.position, Quaternion.identity).GetComponentInChildren<Item>();
        goods.transform.parent = transform;
        goods.hintCanShow = false;
        goods.canReceiveTrigger = false;
        textMesh.text = string.Format("{0}", goods.price);
    }

    public void ConfigTable()
    {
        table.playerEnterAction = new System.Action(playerEnterTable);
        table.interactionBodyAction= new System.Action(playerInteractionTable);
        table.playerLeavection = new System.Action(playerLeaveTable);
        table.hintCanShow = false;
    }

    private void playerEnterTable()
    {
        if (!isGoodsSold)
        {
            //show canva
            sellInfoCanvas.SetActive(true);
        }
    }
    private void playerInteractionTable()
    {
        if (!isGoodsSold)
        {
            goods.interactionBodyAction();
            sellInfoCanvas.SetActive(false);
        }

    }

    private void playerLeaveTable()
    {
        if (!isGoodsSold)
        {
            sellInfoCanvas.SetActive(false);
            //hide canva
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
