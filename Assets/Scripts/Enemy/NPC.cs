using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class NPC : Entity
{
    public float moveSpeed;
    public float damage;
    public int health;
    public int maxHealth;
    public int armor;
    public int maxArmor;
    public int mana;
    public int maxMana;

    public bool isRecoverArmor = false;
    public float recoverArmorStepTime;
    public int recoverArmorStepValue;
    public float recoverArmorInterruptTime;
    private float recoverArmorCounter;
    private float recoverArmorInterruptCounter;

    public Vector3 buffPositionOffset;

    private float outControlTime = 0.05f;
    public bool isReceiveDamage = true;

    [HideInInspector] public bool isOutControl;
    [HideInInspector] public Rigidbody2D rigid2d;

    //buff
    private List<Buff> buffList = new List<Buff>();

    public void Start()
    {
        ConfigDefalut();
    }

    void ConfigDefalut()
    {
        rigid2d = GetComponentInParent<Rigidbody2D>();
    }

    public void Update()
    {
        checkRecoverArmor();
    }

    //buffList
    public void AddBuff(Buff _buff)
    {
        Buff existSameBuff = null;
        //验重
        if (CheckBuffIsExist(_buff.ToString(), out existSameBuff))
        {
            buffList.Remove(existSameBuff);
            existSameBuff.BuffUnLoad();
        }

        buffList.Add(_buff);

    }

    private bool CheckBuffIsExist(string _buffName, out Buff _existSameBuff)
    {
        bool returnValue = false;
        _existSameBuff = null;

        foreach (Buff buff in buffList)
        {
            if (buff.ToString().Equals(_buffName))
            {
                returnValue = true;
                _existSameBuff = buff;
            }
        }
        return returnValue;
    }

    public void RemoveBuff(Buff _buff)
    {
        buffList.Remove(_buff);
    }

    public List<Buff> GetBuffList()
    {
        return buffList;
    }

    public float ExcuteHittingBuffEffect(float _damage)
    {
        float tmp = _damage;
        if (this is BuffReceiverInterFace)
        {
            foreach (Buff buff in buffList)
            {
                if (buff is BuffReceiveHittingDamageInterFace)
                {
                    tmp = ((BuffReceiveHittingDamageInterFace)buff).BuffReceiveHittingDamageInterFaceBody(tmp);
                }
            }
        }

        return tmp;
    }

    public float ExcuteHittedBuffEffect(float _damage)
    {
        float tmp = _damage;
        if (this is BuffReceiverInterFace)
        {
            foreach (Buff buff in buffList)
            {
                if (buff is BuffReceiveHittedDamageInterFace)
                {
                    tmp = ((BuffReceiveHittedDamageInterFace)buff).BuffReceiveHittedDamageInterFaceBody(tmp);
                }
            }
        }

        return tmp;
    }

    //恢复

    public void RestoreHealth(int _hp)
    {
        if (_hp > 0)
        {
            int tmp = health + _hp;
            health = tmp > maxHealth ? maxHealth : tmp;
            RestoreHealthBody(_hp);
        }
    }

    public virtual void RestoreHealthBody(int _hp)
    {
        //body都是预留给子类实现的,其实可以写个Interface 强制要求子类实现,代码阅读起来容易点

    }


    public bool canReceiveRepel = true;


    //受伤
    public void ReceiveDamageWithOutRepel(float _damage)
    {
        ReceiveDamageWithRepelVector(_damage, Vector3.zero);
    }

    public virtual void ReceiveDamageWithRepelVector(float _damage, Vector3 _repelVector)
    {
        if (isReceiveDamage)
        {
            if (canReceiveRepel)
            {
                isOutControl = true;
                rigid2d.velocity = _repelVector / outControlTime;
                StartCoroutine("BackToUnderControl");
            }

            ReduceHealth(ExcuteHittedBuffEffect(_damage));

        }
        ReceiveDamageWithRepelVectorBody(_damage, _repelVector);

    }
    public virtual void ReceiveDamageWithRepelVectorBody(float _damage, Vector3 _repelVector)
    {
    }
    IEnumerator BackToUnderControl()
    {
        yield return new WaitForSeconds(outControlTime);
        isOutControl = false;
        BackToUnderControlBody(isOutControl);
    }

    public virtual void BackToUnderControlBody(bool _isOutControl)
    {
        
    }

    //掉血
    public void ReduceHealth(float _reduceValue)
    {
        ReduceHealthBody(_reduceValue);
    }
    public virtual void ReduceHealthBody(float _reduceValue)
    {

    }

    //mana
    public bool ReceiveManaReduce(int manaReduce)
    {
        if (mana == 0)
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

        UpdateStateUI();

        return true;
    }

    public bool ReceiveDamage(int damage)
    {
        bool receiveSuccess = true;
        int temp = armor - damage;

        //开启中断recoverArmorInterruptCounter
        isRecoverArmor = false;
        recoverArmorInterruptCounter = 0;//重新计时

        if (temp >= 0)
        {
            //防御足够吸收
            armor = temp;
        }
        else
        {
            armor = 0;
            health -= Mathf.Abs(temp);
            if (health <= 0)
            {
                //血量不足以吸收伤害
                health = 0;
                receiveSuccess = false;
                //TODO
                Debug.Log("PlayerDeal");
            }
        }
        UpdateStateUI();
        return receiveSuccess;
    }
    private void RecoverArmor(int i)
    {
        armor += 1;
        armor = armor >= maxArmor ? maxArmor : armor;
        UpdateStateUI();
    }

    private void checkRecoverArmor()
    {
        if (isRecoverArmor)
        {
            recoverArmorCounter += Time.deltaTime;
            if (recoverArmorCounter >= recoverArmorStepTime)
            {
                //等待时间触发恢复
                RecoverArmor(recoverArmorStepValue);
                //reset
                recoverArmorCounter = 0;
            }
        }
        else
        {
            recoverArmorInterruptCounter += Time.deltaTime;
            if (recoverArmorInterruptCounter >= recoverArmorInterruptTime)
            {
                isRecoverArmor = true;
                recoverArmorInterruptCounter = 0;
                //中断结束立刻恢复一次(offset一下恢复计数器达到效果)
                recoverArmorCounter = recoverArmorStepTime;
            }
        }

    }


        //界面更新
     private void UpdateStateUI()
    {
        UpdateStateUIBody();
    }

    public virtual void UpdateStateUIBody()
    {

    }

}
