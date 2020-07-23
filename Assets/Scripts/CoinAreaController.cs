using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinAreaController : MonoBehaviour
{

    public TextMeshProUGUI coinNumText;
    public Animator animator;

    private GameController gc;

    // Start is called before the first frame update
    void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        updateCoinnNum(gc.coinNum);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CoinAreaShake()
    {
        animator.Play("Base Layer.Shake", 0, 0.0f);
    }

    public void updateCoinnNum(int coinNum)
    {
        coinNumText.text = string.Format("{0:00}", coinNum);
    }
}
