using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreamSpeedBuffTile : MonoBehaviour
{
    private float timer;
    public float effectgIntervalTime = 1.0f;
    public string buffName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        LanuchBuff(other);

    }

    private void LanuchBuff(Collider2D other)
    {
        NPC npc = other.gameObject.GetComponentInChildren<NPC>();
        Buff buff = Instantiate((GameObject)Resources.Load("Buff/" + buffName)).GetComponentInChildren<Buff>();
        buff.BuffLoad(npc);
    }
}
