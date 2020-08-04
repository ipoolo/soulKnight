using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.U2D;

public class ShopTable : MonoBehaviour
{
    public GameObject sellInfoCanvas;
    public Item goods;
    public Item table;
    public TextMeshProUGUI textMesh;
    private bool isGoodsSold = false;
    public PlayerController player;
    public GameObject coinImage;
    public GameObject goodsLocation;
    // Start is called before the first frame update
    private void Awake()
    {
        sellInfoCanvas.SetActive(false);
    }

    void Start()
    {
        ConfigTable();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

    }

    public void ConfigGoods(string goodsName)
    {
        ConfigGoods((GameObject)Resources.Load("Item/" + goodsName));
    }

    public void ConfigGoods(GameObject goodsPrefab)
    {
        goods = Instantiate(goodsPrefab, goodsLocation.transform.position, Quaternion.identity).GetComponentInChildren<Item>();
        goods.transform.parent.parent = transform;
        goods.hintCanShow = false;
        goods.canReceiveTrigger = false;

    }

    public void ConfigTable()
    {
        table.playerEnterAction = new System.Action(playerEnterTable);
        table.interactionBodyAction= new System.Action(playerInteractionTable);
        table.playerExitAction = new System.Action(playerLeaveTable);
        table.hintCanShow = false;
    }

    private void playerEnterTable()
    {
        if (!isGoodsSold)
        {
            //show canva
            sellInfoCanvas.SetActive(true);
            textMesh.text = string.Format("{0}", goods.price);
            coinImage.SetActive(true);
        }
    }
    private void playerInteractionTable()
    {
        if (!isGoodsSold)
        {
            if (player.playerStateController.coinReduce(10))
            {
                isGoodsSold = true;
                if (goods.interactionBodyAction != null)
                {
                    goods.interactionBodyAction();
                }

                sellInfoCanvas.SetActive(false);
            }
            else
            {
                coinImage.SetActive(false);
                textMesh.text = string.Format("穷鬼,走走走");
            }
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
