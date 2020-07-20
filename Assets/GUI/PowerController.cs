using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerController : MonoBehaviour
{

    private Scrollbar scrollBar;
    private CanvasGroup canvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        scrollBar = GetComponent<Scrollbar>();
        canvasGroup = GetComponentInParent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
        hidePowerBar();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void showPowerBar()
    {
        resetValue();
        canvasGroup.alpha = 1.0f;
        
    }

    public void hidePowerBar()
    {
        gameObject.SetActive(true);
        canvasGroup.alpha = 0.0f;
    }

    public void updatePowerBarValue(float _value)
    {
        scrollBar.size = _value > 1 ? 1 : _value;
    }

    public void resetValue()
    {
        scrollBar.size = 0.0f;
    }
}
