using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectTile : MonoBehaviour
{
    public float effectgIntervalTime;
    public Action enterAction;
    public Action stayAction;
    public Action exitAction;

    private NpcTimer npcTimer;

    private void Awake()
    {
        npcTimer = this.gameObject.AddComponent<NpcTimer>().ConfingIntervalTime(effectgIntervalTime, effectgIntervalTime - 0.01f);
    }
    // Start is called before the first frame update
    public void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {

    }


    private void OnTriggerEnter2D(Collider2D other)
    {

        NPC npc = other.gameObject.GetComponentInChildren<NPC>();
        if (npc != null)
        {
            npcTimer.addNpc(npc);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {

        NPC npc = other.gameObject.GetComponentInChildren<NPC>();
        if (npc != null)
        {
            if (npcTimer.CheckTimerForEffect(npc))
            {
                EffectBody(npc);
            }

        }
    }



    private void OnTriggerExit2D(Collider2D other)
    {

        NPC npc = other.gameObject.GetComponentInChildren<NPC>();
        if (npc != null)
        {
            npcTimer.removeNpc(npc);
        }

    }

    public virtual void EffectBody(NPC npc)
    {

    }

}
