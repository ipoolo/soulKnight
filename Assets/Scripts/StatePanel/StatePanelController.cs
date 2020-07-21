using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatePanelController : MonoBehaviour
{
    public Scrollbar hpScrollbar;
    public Scrollbar armorScrollbar;
    public Scrollbar manaScrollbar;

    public TextMeshProUGUI hpText;
    public TextMeshProUGUI armorText;
    public TextMeshProUGUI manaText;

    public Animator manaScrollbarAnimator;


    // Start is called before the first frame update
    void Start()
    {
        manaScrollbarAnimator = GetComponentInChildren<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeHealth(int curr,int max)
    {
        float temp = 0;
        if (max != 0) { 
            temp = (float) curr / max;
        }
        hpScrollbar.size = temp > 1 ? 1 : temp;
        hpText.text = curr + "/" + max;
        
    }


    public void changeArmor(int curr, int max)
    {
        float temp = 0;
        if (max != 0)
        {
            temp = (float) curr / max;
        }
        armorScrollbar.size = temp > 1 ? 1 : temp;
        armorText.text = curr + "/" + max;
    }

    public void changeMana(int curr, int max)
    {
        float temp = 0;
        if (max != 0)
        {
            temp = (float) curr / max;
        }
        manaScrollbar.size = temp > 1 ? 1 : temp;
        manaText.text = curr + "/" + max;
    }

    public void shakeManaBar()
    {
        manaScrollbarAnimator.Play("Base Layer.shake",0,0.0f);
    }

}
