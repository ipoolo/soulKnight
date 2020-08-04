using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Location
{
    public int x;
    public int y;
}
public enum MapBlockType
{
    startType,
    endType,
    BossType,
    battleType,
    shopType,
    eventType
}

public class MapBlockInfo : MonoBehaviour
{
    public Location indexInfo = new Location();
    public MapBlockType blockType;
    public MapBlockInfo parent = null;
    public BlockController bc;
    public SpriteRenderer sRender;

    private void Awake()
    {
        sRender = GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {

        string fileName = "";
        switch (blockType)
        {
            case MapBlockType.battleType:
                fileName = "BattleRoom";
                break;
            case MapBlockType.endType:
                fileName = "EndRoom";
                break;
            case MapBlockType.eventType:
                fileName = "EventRoom";
                break;
            case MapBlockType.startType:
                fileName = "StartRoom";
                break;
            case MapBlockType.shopType:
                fileName = "ShopRoom";
                break;
            case MapBlockType.BossType:
                fileName = "BossRoom";
                break;
        }
        sRender.sprite = Sprite.Create((Texture2D)Resources.Load(string.Format("MiniMap/{0}", fileName), typeof(Texture2D)), sRender.sprite.rect, new Vector2(0.5f,0.5f),16);
    }

    public void ChangeColor(Color _color)
    {
        sRender.color = _color;
    }

    public Color CurrColor()
    {
        return sRender.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
