using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ChannelWallController : MonoBehaviour
{
    public BlockDireciton channelDirection;
    // Start is called before the first frame update
    void Start()
    {
        configChannel();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void configChannel()
    {

        if(gameObject.name.Contains("Top")){
            channelDirection = BlockDireciton.Top;
        }
        if (gameObject.name.Contains("Right"))
        {
            channelDirection = BlockDireciton.Right;
        }
        if (gameObject.name.Contains("Bottom"))
        {
            channelDirection = BlockDireciton.Bottom;
        }
        if (gameObject.name.Contains("Left"))
        {
            channelDirection = BlockDireciton.Left;
        }
    }

    public bool receiveChannelControl(BlockDireciton direction ,bool isClose)
    {
        bool result = false;
        if (direction == channelDirection)
        {
            gameObject.SetActive(isClose);
            result = true;
        }
        
        return result;

    }
}
