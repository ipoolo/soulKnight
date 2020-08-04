using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopBlockController : BlockController
{
    public List<string> itemPrefabNames;
    public float tableOffset = 1;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        ConfigShopTalbesAndItems();
    }

    private void ConfigShopTalbesAndItems()
    {
        if(itemPrefabNames != null)
        {
            GameObject shopTablePrefab;
            Vector2 centerPosition = new Vector2(transform.position.x + blockWidth / 2.0f, transform.position.y - blockWidth / 2.0f);

            for (int i = 0; i< itemPrefabNames.Count; i++) {

                shopTablePrefab = (GameObject)Resources.Load("Item/ShopTable");
                Vector2 spwanLocation = new Vector2(centerPosition.x + (-1 + i) * tableOffset, centerPosition.y);
                ShopTable st = Instantiate(shopTablePrefab, spwanLocation, Quaternion.identity).GetComponent<ShopTable>();
                st.transform.parent = transform;
                st.ConfigGoods(itemPrefabNames[i]);
            }

        }
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }
}
