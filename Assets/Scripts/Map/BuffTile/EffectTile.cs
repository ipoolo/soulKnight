using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectTile : MonoBehaviour
{
    public float effectgIntervalTime = 1.0f;
    public Action enterAction;
    public Action stayAction;
    public Action exitAction;

    private Dictionary<NPC, float> onTriggerNpcs = new Dictionary<NPC, float>();

    // Start is called before the first frame update
    public void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        CalOnTriggerNpcsTimer();
    }

    private void CalOnTriggerNpcsTimer()
    {
        List<KeyValuePair<NPC, float>> tmpList = new List<KeyValuePair<NPC, float>>();
        foreach (KeyValuePair<NPC, float> kv in onTriggerNpcs)
        {
            float tmpValue = Time.deltaTime + kv.Value;
            tmpList.Add(new KeyValuePair<NPC, float>(kv.Key, tmpValue));
        }

        foreach (KeyValuePair<NPC, float> kv in tmpList)
        {
            onTriggerNpcs.Remove(kv.Key);
            onTriggerNpcs.Add(kv.Key, kv.Value);

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        NPC npc = other.gameObject.GetComponentInChildren<NPC>();
        if (npc != null)
        {
            EffectBody(npc);
            onTriggerNpcs.Add(npc, 0.0f);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {

        NPC npc = other.gameObject.GetComponentInChildren<NPC>();
        if (npc != null)
        {
            if (CheckTimerForEffect(npc))
            {
                EffectBody(npc);
            }

        }
    }

    private bool CheckTimerForEffect(NPC npc)
    {
        if (onTriggerNpcs.ContainsKey(npc)) { 
            float timer = onTriggerNpcs[npc];
            if (timer >= effectgIntervalTime)
            {
                onTriggerNpcs.Remove(npc);
                onTriggerNpcs.Add(npc, 0.0f);
                return true;
            }
        }
        return false;
    }

    private void OnTriggerExit2D(Collider2D other)
    {

        NPC npc = other.gameObject.GetComponentInChildren<NPC>();
        if (npc != null)
        {
            onTriggerNpcs.Remove(npc);
        }

    }

    public virtual void EffectBody(NPC npc)
    {

    }

}
