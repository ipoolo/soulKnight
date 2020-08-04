using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IndicatorPanelController : MonoBehaviour
{
    public TextMeshProUGUI damageText;
    public SpriteRenderer sRenderer;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        damageText = GetComponentInChildren<TextMeshProUGUI>();
        sRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void ShowIndicatorPanel(string damage,Sprite sp)
    {
        gameObject.SetActive(true);
        damageText.text = damage;
        sRenderer.sprite = sp;
    }

    public void HideIndicatorPanel()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
