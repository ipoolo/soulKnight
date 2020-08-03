using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hint : MonoBehaviour
{
    [SerializeField]public Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void showHint()
    {
        animator.Play("Base Layer.HintShow",0,0.0f);

    }

    public void hideHint()
    {
        
        animator.Play("Base Layer.HintHide", 0, 0.0f);
    }
}
