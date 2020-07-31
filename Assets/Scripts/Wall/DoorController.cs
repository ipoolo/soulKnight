using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        ConfigDefault();
    }

    public void ConfigDefault()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReceiveDoorControll(bool isColse)
    {
        if (isColse)
        {
            animator.SetTrigger("Close");
        }else
        {
            animator.SetTrigger("Open");
        }
    }

}
