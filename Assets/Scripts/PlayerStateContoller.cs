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
    public int ammor;
    public int maxAmmor;
    public int mana;
    public int maxMana;

    // Start is called before the first frame update
    void Start()
    {
        statePanel = GameObject.FindGameObjectWithTag("StatePanel");
        spController = statePanel.GetComponent<StatePanelController>();
        updateStatePlane();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool receiveDamageWithRepel(int damage , Vector3 repelV)
    {
        bool receiveSuccess = true;
        int temp = ammor - damage;

        if(temp >= 0)
        {
            //防御足够吸收
            ammor = temp;
        }
        else
        {
            ammor = 0;
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


    public bool receiveDamageWithRepel(int damage)
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
        spController.changeArmor(ammor, maxAmmor);
        spController.changeMana(mana, maxMana);
    }
}
