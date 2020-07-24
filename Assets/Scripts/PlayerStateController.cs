using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerStateController : MonoBehaviour,BuffInterFace
{
    public GameObject statePanel;
    public StatePanelController spController;

    public int health;
    public int maxHealth;
    public int armor;
    public int maxArmor;
    public int mana;
    public int maxMana;

    public int coinNum;
    public float receiveRepelVectorScale;
    public GameObject player;

    public float recoverArmorStepTime;
    public int recoverArmorStepValue;
    public float recoverArmorInterruptTime;
    private float recoverArmorCounter;
    private float recoverArmorInterruptCounter;

    private bool isRecoverArmor;

    private CoinAreaController cac;

    public GameObject psEffect;//粒子效果
    public GameObject canvasDamage;
    public SpriteRenderer render;
    private Color originalColor;
    private bool isLayerShake;
    private bool isReceiveDamage;

    public float canvasDamageOffsetY;
    public Rigidbody2D playerRigidbody2D;
    public PlayerController pc;

    private float outControlTime = 0.05f;
    public Vector3 buffPositionOffset = new Vector3(0, 0, 0);

    public string restoreEffectPathStrInRes;
    public float restoreEffectColorTime;
    public Color restoreEffectColor;


    //buff
    private List<Buff> buffList = new List<Buff>();
    //debuff

    [SerializeField] public float invincibilityTime;

    // Start is called before the first frame update
    void Start()
    {
        ConfigDefault();
        UpdateStatePlane();


    }

    private void ConfigDefault()
    {
        //coin 
        coinNum = 0;
        player = GameObject.FindGameObjectWithTag("Player") as GameObject;
        playerRigidbody2D = player.GetComponent<Rigidbody2D>();

        statePanel = GameObject.FindGameObjectWithTag("StatePanel");
        spController = statePanel.GetComponent<StatePanelController>();

        cac = GameObject.FindGameObjectWithTag("CoinAreaController").GetComponent<CoinAreaController>();
        psEffect = Resources.Load("Partcles/PS_BloodEffect") as GameObject;
        canvasDamage = Resources.Load("CanvasDamage") as GameObject;
        pc = player.GetComponent<PlayerController>();

        isRecoverArmor = true;
        render = gameObject.GetComponentInParent<SpriteRenderer>();
        originalColor = render.color;

        if (invincibilityTime == 0)
        {
            invincibilityTime = 2.0f;
        }
        if(canvasDamageOffsetY == 0)
        {
            canvasDamageOffsetY = 0.5f;
        }
        if(receiveRepelVectorScale == 0.0f)
        {
            receiveRepelVectorScale = 1.0f;
        }

        isReceiveDamage = true;
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

    //RestoreHealth
    public void RestoreHealth(int _hp)
    {
        if(_hp > 0) { 
            int tmp = health + _hp;
            health = tmp > maxHealth ? maxHealth : tmp;
            //restoreEffect
            RestoreEffect();
            UpdateStatePlane();
        }
    }

    private void RestoreEffect()
    {
        //加一点粒子效果
        if (restoreEffectPathStrInRes.Length != 0)
        {
            GameObject effectPSOb = Instantiate(Resources.Load(restoreEffectPathStrInRes) as GameObject, player.transform.position, Quaternion.identity);
            effectPSOb.transform.parent = player.transform;
            render.color = restoreEffectColor;
        }
        StartCoroutine("renderBackOriginColor");
    }

    IEnumerator RenderBackOriginColor()
    {
        yield return new WaitForSeconds(restoreEffectColorTime);
        render.color = originalColor;
    }



    
    private bool ReceiveDamage(int damage)
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
            health -= math.abs(temp);
            if (health <= 0)
            {
                //血量不足以吸收伤害
                health = 0;
                receiveSuccess = false;
                //TODO
                Debug.Log("PlayerDeal");
            }
        }
        UpdateStatePlane();
        return receiveSuccess;
    }

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

        UpdateStatePlane();

        return true;
    }

    private void UpdateStatePlane()
    {
        spController.changeHealth(health, maxHealth);
        spController.changeArmor(armor, maxArmor);
        spController.changeMana(mana, maxMana);
    }

    private void RecoverArmor(int i)
    {
        armor += 1;
        armor = armor >= maxArmor ? maxArmor : armor;
        UpdateStatePlane();
    }

    //coin
    //coinSetting
    public void CoinAdd(int addeNum)
    {
        coinNum += addeNum;
        //update coinUI
        UpdateCoinUI();
    }

    public void UpdateCoinUI()
    {
        cac.updateCoinnNum(coinNum);
    }

    public bool coinReduce(int reduceNum)
    {
        if (coinNum >= reduceNum)
        {
            coinNum -= reduceNum;
            //update coinUI
            UpdateCoinUI();
            return true;
        }
        else
        {
            //coin ui_text Shake
            cac.CoinAreaShake();
            return false;
        }

    }

    //receiveDamage

    public void ReceiveDamageWithOutRepel(float _damage)
    {
        ReceiveDamageWithRepelVector(_damage, Vector3.zero);
    }
    public void ReceiveDamageWithRepelVector(float _damage, Vector3 _repelVector)
    {

        if (isReceiveDamage)
        {
            isReceiveDamage = false;
            Vector3 tmp = _repelVector.normalized / outControlTime;
            playerRigidbody2D.velocity = tmp;

            pc.isOutController = true;
            StartCoroutine("PlayerBackToContoller");
            
            ReduceHealth(ExcuteHittedBuffEffect(_damage));
            RenderWhiteAndTurnInvincibilityLayer();
        }
    }

    private float ExcuteHittedBuffEffect(float _damage)
    {
        float tmp = _damage;
        if (this is BuffInterFace)
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

    IEnumerator PlayerBackToContoller()
    {
        yield return new WaitForSeconds(outControlTime);
        pc.isOutController = false;
        
    } 

    private void ReduceHealth(float _reduceValue)
    {
        int floorValue = Mathf.FloorToInt(_reduceValue);

        //掉血粒子效果 （可以设置专门的出血位）
        Instantiate(psEffect, transform.position, Quaternion.identity);
        //掉血数值 (暴击值和普通值这里应该通过配置设置不同效果)
        Vector3 tmp = transform.position;
        tmp += new Vector3(0, canvasDamageOffsetY,0);
        Instantiate(canvasDamage, tmp, Quaternion.identity).GetComponent<DamageText>().setDamageText(floorValue);
        //扣血
        ReceiveDamage(floorValue);

    }

    private void RenderWhiteAndTurnInvincibilityLayer()
    {
        render.color = Color.red;
        StartCoroutine("BackToOriginalColorAndUninvincibility");
        player.layer = LayerMask.NameToLayer("InvincibilityLayer");
    }

    IEnumerator BackToOriginalColorAndUninvincibility()
    {
        isLayerShake = true;
        StartCoroutine("LayerShake");
        yield return new WaitForSeconds(invincibilityTime);
        isLayerShake = false;
        render.color = originalColor;
        isReceiveDamage = true;
        player.layer = LayerMask.NameToLayer("Player");
    }

    IEnumerator LayerShake()
    {
        while (isLayerShake)
        {
            if (render.color != Color.red)
            {
                render.color = Color.red;
            }
            else
            {
                render.color = Color.yellow;
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    //buffList
    public void AddBuff(Buff _buff)
    {
        Buff existSameBuff = null;
        //验重
        if(CheckBuffIsExist(_buff.ToString(),out existSameBuff))
        {
            buffList.Remove(existSameBuff);
            existSameBuff.BuffUnLoad();
        }

        buffList.Add(_buff);

        Debug.Log("buffList"+ buffList.Count);
    }

    private bool CheckBuffIsExist(string _buffName,out Buff _existSameBuff)
    {
        bool returnValue = false;
        _existSameBuff = null;

        foreach (Buff buff in buffList)
        {
            if(buff.ToString().Equals(_buffName))
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
}
