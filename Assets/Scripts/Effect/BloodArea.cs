using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodArea : MonoBehaviour
{
    private SpriteRenderer sr;
    private Sprite[] sps;
    public Color endColor;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sps = Resources.LoadAll<Sprite>("Effect/Blood");
    }
    // Start is called before the first frame update
    void Start()
    {
        int index = Random.Range(0, sps.Length);
        sr.sprite = sps[index];
        StartCoroutine(BloodTurnDrak());
        
    }
    
    IEnumerator BloodTurnDrak()
    {
        yield return new WaitForSeconds(1.0f);
        sr.DOColor(endColor, 4.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
