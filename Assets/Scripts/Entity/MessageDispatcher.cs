using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Message
{
    public string sender;
    public string receiver;
    public int msg;//状态枚举
    public double sendMessageTime;
    public object[] otherMsgInfo;
}

public class MessageDispatcher : Singleton<MessageDispatcher>
{
    private static List<Message> msgDelayList = new List<Message>();
    private static MessageDispatcher _messageDispatcher;

    public void Update()
    {
        checkMsgShouldSend();
    }


    public void DispatchMassage(double _delayTime, Message _msg)
    {
        Entity e = EntityManager.ManagerInstance().EntityById(_msg.receiver);
        if (_delayTime == 0)
        {
            _msg.sendMessageTime = Time.time;
            SendMessage(e, _msg);
        }
        else if(_delayTime > 0)
        {
            _msg.sendMessageTime = Time.time + _delayTime;
            msgDelayList.Add(_msg);
        }
    }

    private void checkMsgShouldSend()
    {
        List<Message> removeList = new List<Message>();
        if (msgDelayList != null && msgDelayList.Count > 0)
        {
            foreach(Message msg in msgDelayList)
            {
                if(msg.sendMessageTime <= Time.time)
                {
                    Entity e = EntityManager.ManagerInstance().EntityById(msg.receiver);
                    SendMessage(e,msg);
                    removeList.Add(msg);
                }
            }

            foreach(Message msg in removeList)
            {
                msgDelayList.Remove(msg);
            }
        }
    }


    public void SendMessage(Entity _e, Message _msg)
    {
        bool sendResult = _e.ReceiveMsg(_msg);
    }

    
}
