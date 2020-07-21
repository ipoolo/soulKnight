using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerStateContoller : MonoBehaviour
{
    public GameObject statePanel;
    public StatePanelController spController;

    public int health;
    public int maxHealth;
    public int armor;
    public int maxArmor;
    public int mana;
    public int maxMana;

    public float recoverArmorStepTime;
    public int recoverArmorStepValue;
    public float recoverArmorInterruptTime;
    private float recoverArmorCounter;
    private float recoverArmorInterruptCounter;

    private bool isRecoverArmor;


    // Start is called before the first frame update
    void Start()
    {
        statePanel = GameObject.FindGameObjectWithTag("StatePanel");
        spController = statePanel.GetComponent<StatePanelController>();
        updateStatePlane();
        isRecoverArmor = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRecoverArmor)
        {
            recoverArmorCounter += Time.deltaTime;
            if (recoverArmorCounter >= recoverArmorStepTime)
            {
                //等待时间触发恢复
                recoverArmor(recoverArmorStepValue);
                //reset
                recoverArmorCounter = 0;
            }
        }
        else
        {
            recoverArmorInterruptCounter += Time.deltaTime;
            if(recoverArmorInterruptCounter >= recoverArmorInterruptTime)
            {
                isRecoverArmor = true;
                recoverArmorInterruptCounter = 0;
                //中断结束立刻恢复一次(offset一下恢复计数器达到效果)
                recoverArmorCounter = recoverArmorStepTime;
            }
        }
    }

    public bool receiveDamageWithRepel(int damage , Vector3 repelV)
    {
        bool receiveSuccess = true;
        int temp = armor - damage;

        //开启中断recoverArmorInterruptCounter
        isRecoverArmor = false;

        if (temp >= 0)
        {
            //防御足够吸收
            armor = temp;
        }
        else
        {
            armor = 0;
            health -= math.abs(temp);
            if(health <= 0)
            {
                //血量不足以吸收伤害
                health = 0;
                receiveSuccess = false;
            }
        }
        updateStatePlane();
        return receiveSuccess;
    }


    public bool receiveDamage(int damage)
    {
        return receiveDamageWithRepel(damage , Vector3.zero);
    }

    public bool receiveManaReduce(int manaReduce)
    {
        if(mana == 0)
        {
            return false;
        }

        int temp = mana - manaReduce;
        if (temp >= 0)
        {
            mana = temp;
        }
        else
        {
            mana = 0;
        }

        updateStatePlane();

        return true;
    }

    private void updateStatePlane()
    {
        spController.changeHealth(health, maxHealth);
        spController.changeArmor(armor, maxArmor);
        spController.changeMana(mana, maxMana);
    }

    private void recoverArmor(int i)
    {
        armor += 1;
        armor = armor >= maxArmor ? maxArmor : armor;
        updateStatePlane();
    }

}
