using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorAreaDireciton{
    Top,
    Right,
    Bottom,
    Left
}

public class DoorAreaController : MonoBehaviour
{
    // Start is called before the first frame update
    public DoorAreaDireciton daDircetion;
    public BlockController owner;

    void Start()
    {
        daDircetion = DoorAreaDireciton.Right;
        owner = GetComponentInParent<BlockController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeDoorState(bool isColse)
    {
        gameObject.BroadcastMessage("ReceiveDoorControll", isColse);
    }

    public void ReceivePlayerExit(Vector2 playerVelocityDirection)
    {
        Vector2 inNormal = Vector2.right;
        switch (daDircetion)
        {
            case DoorAreaDireciton.Top:
                inNormal = Vector2.down;
                break;
            case DoorAreaDireciton.Right:
                inNormal = Vector2.left;
                break;
            case DoorAreaDireciton.Bottom:
                inNormal = Vector2.up;
                break;
            case DoorAreaDireciton.Left:
                inNormal = Vector2.right;
                break;
        }

        if(Vector2.Dot(playerVelocityDirection, inNormal) > 0)
        {
            Debug.Log("C");
            //进入
            //传给block判断
            owner.receivePlayerEnter();
        }
    }


}
