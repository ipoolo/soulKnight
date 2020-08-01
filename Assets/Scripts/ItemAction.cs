using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//没有其他复杂行为的action 配置在这里.有的都通过继承item来实现了

public class ItemAction : System.Object
{
    public static Action ItemExitAction()
    {
        return () =>
        {
            Debug.Log("PlayerLeaveThisLevel"); 
        };
    }

}
