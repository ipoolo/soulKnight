using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : Item
{
    private Animator treasureAnimator;
    private bool isOpened;
    private float openTimeOffset = 0.667f;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        DefaultConfig();
    }

    private void DefaultConfig()
    {
        treasureAnimator = GetComponentInParent<Animator>();
        //this.interactionBodyAction = new System.Action(OpenTreasure);
        this.playerEnterAction = new System.Action(OpenTreasure);
        offset = new Vector2(-0.5f,1.5f);
        isOpened = false;
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }

    private void OpenTreasure()
    {
        if (!isOpened) {
            isOpened = true;
            treasureAnimator.SetTrigger("Open");
            Invoke("SpwanCoin", openTimeOffset);
            HideFoucsArror();
            hintCanShow = false;
        }
    }

    private void SpwanCoin()
    {
        int coinCount = Random.Range(3, 7);
        for (int i = 0; i < coinCount; i++) {
            Coin coin = Instantiate((GameObject)Resources.Load("Coin")).GetComponent<Coin>();
            coin.transform.position = transform.parent.position;
            coin.radius = 4.0f;
        }
    }



}
