using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    // Start is called before the first frame update
    [HideInInspector] public string entityIdString;
    protected bool isRegister = true;

    private void OnEnable()
    {
        if (isRegister)
        {
            entityIdString = EntityManager.ManagerInstance().RegisterEntity(this);
        }
    }

    private void OnDestroy()
    {
        EntityManager.ManagerInstance().removeByEntity(this);
    }

    public virtual bool ReceiveMsg(Message msg)
    {
        throw new System.NotImplementedException();
    }


}
