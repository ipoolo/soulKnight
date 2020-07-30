using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public enum ChannelDirectionType{
    Top,
    right,
    left,
    Bottom
}


public class ChannelController : MonoBehaviour
{
    public ChannelDirectionType channelDirection;
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
            channelDirection = ChannelDirectionType.Top;
        }
        if (gameObject.name.Contains("Right"))
        {
            channelDirection = ChannelDirectionType.right;
        }
        if (gameObject.name.Contains("Bottom"))
        {
            channelDirection = ChannelDirectionType.Bottom;
        }
        if (gameObject.name.Contains("Left"))
        {
            channelDirection = ChannelDirectionType.left;
        }
    }

    public void receiveChannelControl(ChannelDirectionType direction ,bool isOpen)
    {
        if (direction == channelDirection)
        {
            gameObject.SetActive(isOpen);
        }

    }
}
