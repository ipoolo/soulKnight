using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peak : MonoBehaviour, IBattleState
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
    // Start is called before the first frame update
    void Start()
    {
        ConfigDefault();
    }

    private void ConfigDefault()
    {
        animator = GetComponent<Animator>();
        isOn = true;
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
            Vector2 repel = npc.transform.position - transform.position;
            npc.ReceiveDamageWithRepelVector(Mathf.FloorToInt(damage), repel.normalized);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        NPC npc = other.gameObject.GetComponentInChildren<NPC>();
        if (npc != null)
        {
            Vector2 repel = npc.transform.position - transform.position;
            npc.ReceiveDamageWithRepelVector(Mathf.FloorToInt(damage), repel.normalized);
        }
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
