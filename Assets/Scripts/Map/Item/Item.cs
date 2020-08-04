using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour,InteractionInterface
{
    public PlayerController playerController;
    public Vector3 offset;
    public bool hintCanShow = true;
    public bool canReceiveTrigger;

    private Hint hint;
    public string hintPrefabName = "Hint";

    //delegate
    public Action interactionBodyAction;
    public Action playerEnterAction;
    public Action playerExitAction;
    public int price;

    protected void Awake()
    {
        
        canReceiveTrigger = true;
    }

    // Start is called before the first frame update
    public void Start()
    {
        configHint();
    }

    private void configHint()
    {
        Vector3 hintPosition = new Vector3(transform.parent.position.x, transform.parent.position.y);
        hint = ((GameObject)Instantiate(Resources.Load(string.Format("Hint/{0}", hintPrefabName)), hintPosition, Quaternion.identity)).GetComponent<Hint>();
        hint.transform.parent = transform.parent;
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }


    // Update is called once per frame
    public void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D ohter)
    {
        if (canReceiveTrigger) { 
            if (ohter.CompareTag("Player"))
            {
                //将自己塞入player的InteractionList;
                playerController.Add2InteractionList(this);
                OnTriggerEnter2DBody();
                if(playerEnterAction != null)
                {
                    playerEnterAction();
                }
            }
        }

    }
    private void OnTriggerExit2D(Collider2D ohter)
    {
        if (canReceiveTrigger)
        {
            if (ohter.CompareTag("Player"))
            {
                //将自己移出player的InteractionList;
                playerController.remove2InteractionList(this);
                OnTriggerExit2DBody();
                if (playerExitAction != null)
                {
                    playerExitAction();
                }
            }
        }
    }


    private void OnTriggerEnter2DBody() {
        
    }

    private void OnTriggerExit2DBody()
    {

    }

    //显示和隐藏由playerController控制

    public void ShowFoucsArror()
    {
        ShowFoucsArror(Vector3.zero);
    }


    public void ShowFoucsArror(Vector3 _offset)
    {
        Vector3 temp = transform.parent.position;

        if (_offset == Vector3.zero) {
            if (offset == Vector3.zero) { 
                temp += new Vector3(0,2,0);
            }
            else
            {
                temp += offset;
            }
        }
        else
        {
            
            temp += _offset;
        }
        hint.transform.position = temp;
        if (hintCanShow) { 
            hint.showHint();
        }

    }

    public void HideFoucsArror()
    {
        if (hintCanShow)
        {
            hint.hideHint();
        }

    }

    public virtual void InteractionBody()
    {
        if(interactionBodyAction != null) { 
          interactionBodyAction();
        }
        Debug.Log("InteractionBody:"+gameObject);
    }


}
