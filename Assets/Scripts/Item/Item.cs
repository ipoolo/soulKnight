using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour,InteractionInterface
{
    private Hint hint;
    // Start is called before the first frame update
    void Start()
    {
        configHint();
        
    }

    private void configHint()
    {
        Vector3 hintPosition = new Vector3(transform.parent.position.x, transform.parent.position.y + 2,0);
        hint = ((GameObject)Instantiate(Resources.Load("Hint"), hintPosition,Quaternion.identity)).GetComponent<Hint>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D ohter)
    {
        if (ohter.CompareTag("Player"))
        {
            //将自己塞入player的InteractionList;
            hint.showHint();

        }

    }
    private void OnTriggerExit2D(Collider2D ohter)
    {
        if (ohter.CompareTag("Player"))
        {
            //将自己移出player的InteractionList;
            hint.hideHint();
        }
    }

    public void ShowFoucsArror(Vector3 offset)
    {

    }

    public void HideFoucsArror()
    {

    }

    public void InteractionBody()
    {

    }
}
