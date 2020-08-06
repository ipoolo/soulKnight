using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour, IBattleState
{
    public float peakOn2OffTime;
    private float peakOn2OffTimer;
    public float peakOff2OnTime;
    private float peakOff2OnTimer;
    public Animator animator;
    private bool isOn;
    private bool isTimerRun = true;
    public float animOffset;
    public float damage;
    private bool isPause = true;
    public float damageIntervalTime = 0.5f;
    private NpcTimer npcTimer;

    // Start is called before the first frame update
    void Start()
    {
        ConfigDefault();
    }

    private void ConfigDefault()
    {
        animator = GetComponent<Animator>();
        isOn = true;
        npcTimer = this.gameObject.AddComponent<NpcTimer>().ConfingIntervalTime(damageIntervalTime, damageIntervalTime - 0.01f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isTimerRun && isPause == false) { 
            if (isOn)
            {
                peakOn2OffTimer += Time.deltaTime;
                if(peakOn2OffTimer > peakOn2OffTime)
                {
                    animator.SetTrigger("Off");
                    isTimerRun = false;
                    Invoke("OffAnimfinishBody", animOffset);

                }
            }
            else
            {
                peakOff2OnTimer += Time.deltaTime;
                if (peakOff2OnTimer > peakOff2OnTime)
                {
                    animator.SetTrigger("On");
                    isTimerRun = false;
                    Invoke("OnAnimfinishBody", animOffset);

                }
            }
        }
    }


    private void  OffAnimfinishBody()
    {
        peakOn2OffTimer = 0;
        isOn = false;
        isTimerRun = true;
    }

    private void OnAnimfinishBody()
    {
        peakOff2OnTimer = 0;
        isOn = true;
        isTimerRun = true;
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

    protected void EffectBody(NPC npc)
    {
        Debug.Log("R_time" + Time.time);
        Vector2 repel = npc.transform.position - transform.position;
        npc.ReceiveDamageWithRepelVector(Mathf.FloorToInt(damage), repel);
    }

    public void BattleStart()
    {
        animator.SetTrigger("On");
        isPause = false;
        isTimerRun = false;
        Invoke("OnAnimfinishBody", animOffset);
    }

    public void BattleEnd()
    {

    }
}
