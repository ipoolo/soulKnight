using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcTimer : MonoBehaviour
{
    private Dictionary<NPC, float> onTriggerNpcs = new Dictionary<NPC, float>();
    private float intervalTime ;
    private float startTimeOffset;

    private void Update()
    {
        CalOnTriggerNpcsTimer();
    }
    public NpcTimer ConfingIntervalTime(float _intervalTime,float _startTimeOffset)
    {
        intervalTime = _intervalTime;
        startTimeOffset = _startTimeOffset;
        return this;
    }

    public void CalOnTriggerNpcsTimer()
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

    public bool CheckTimerForEffect(NPC npc)
    {

        if (onTriggerNpcs.ContainsKey(npc))
        {
            float timer = onTriggerNpcs[npc];
            if (timer >= intervalTime)
            {
                onTriggerNpcs.Remove(npc);
                onTriggerNpcs.Add(npc, 0.0f);//触发后重置为0
                return true;
            }
        }
        return false;
    }

    public void addNpc(NPC npc)
    {
        if (!onTriggerNpcs.ContainsKey(npc)) {
            onTriggerNpcs.Add(npc, startTimeOffset); 
        }
        
    }

    public void removeNpc(NPC npc)
    {
        onTriggerNpcs.Remove(npc);
    }
}
