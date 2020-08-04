using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class IndicatorPanelController : MonoBehaviour
{
    public TextMeshProUGUI damageText;
    public Image imageUI;
    public CanvasGroup cg;
    // Start is called before the first frame update
    void Start()
    {
        cg = GetComponent<CanvasGroup>();
        damageText = GetComponentInChildren<TextMeshProUGUI>();
        Hide();
    }

    public void ShowIndicatorPanel(string damage,Sprite sp)
    {

        damageText.text = damage;
        imageUI.sprite = sp;
        Show();

    }

    public void HideIndicatorPanel()
    {
        Hide();
    }

    private void Show()
    {
        cg.alpha = 1;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }

    private void Hide()
    {
        cg.alpha = 0;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
