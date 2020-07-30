using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DoorAreaController : MonoBehaviour
{
    // Start is called before the first frame update
    public BlockDireciton daDircetion;
    public BlockController owner;
    public GameObject hitbox;
    public DoorController topDoor;
    public DoorController midDoor;
    public DoorController bottomDoor;
    public GameObject obstrucetBullet;

    void Start()
    {
        owner = GetComponentInParent<BlockController>();
    }


    public void configDoorArea(BlockDireciton direction) {
        Vector2 centerPosition = transform.position;
        switch (direction)
        {
            case BlockDireciton.Top:
                topDoor.transform.position = new Vector2(centerPosition.x - 1, centerPosition.y);
                bottomDoor.transform.position = new Vector2(centerPosition.x + 1, centerPosition.y);
                hitbox.transform.rotation = Quaternion.Euler(0, 0, 90);
                obstrucetBullet.transform.rotation = Quaternion.Euler(0, 0, 90);
                daDircetion = BlockDireciton.Top;
                break;
            case BlockDireciton.Right:
                daDircetion = BlockDireciton.Right;
                break;
            case BlockDireciton.Bottom:
                topDoor.transform.position = new Vector2(centerPosition.x - 1, centerPosition.y);
                bottomDoor.transform.position = new Vector2(centerPosition.x + 1, centerPosition.y);
                hitbox.transform.rotation = Quaternion.Euler(0, 0, 90);
                obstrucetBullet.transform.rotation = Quaternion.Euler(0, 0, 270);
                daDircetion = BlockDireciton.Bottom;
                break;
            case BlockDireciton.Left:
                obstrucetBullet.transform.rotation = Quaternion.Euler(0, 0, 180);
                hitbox.transform.rotation = Quaternion.Euler(0, 0, 270);
                daDircetion = BlockDireciton.Left;
                break;
        }
        
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
            case BlockDireciton.Top:
                inNormal = Vector2.down;
                break;
            case BlockDireciton.Right:
                inNormal = Vector2.left;
                break;
            case BlockDireciton.Bottom:
                inNormal = Vector2.up;
                break;
            case BlockDireciton.Left:
                inNormal = Vector2.right;
                break;
        }

        if(Vector2.Dot(playerVelocityDirection, inNormal) > 0)
        {
            //进入
            //传给block判断
            owner.receivePlayerEnter();
        }
    }


}
