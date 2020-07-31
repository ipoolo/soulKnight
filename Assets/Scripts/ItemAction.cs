using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
