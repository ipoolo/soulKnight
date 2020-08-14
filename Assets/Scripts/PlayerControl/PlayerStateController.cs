using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerStateController : NPC,BuffReceiverInterFace
{
    public GameObject statePanel;
    public StatePanelController spController;

    //TODO这类可以再向上抽象接口 npc来实现 基础的血量属性这些的统一(skill buff处就不用判断是玩家还是npc了直接 面向接口编程 ，这里有空再改吧，enemy处和这里统一调整)


    public int coinNum;
    public float receiveRepelVectorScale;
    public GameObject player;



    private CoinAreaController cac;

    public GameObject psEffect;//粒子效果
    public GameObject canvasDamage;
    public SpriteRenderer render;
    private Color originalColor;
    private bool isLayerShake;


    public float canvasDamageOffsetY;
    public Rigidbody2D playerRigidbody2D;
    public PlayerController pc;


    public string restoreEffectPathStrInRes;
    public float restoreEffectColorTime;
    public Color restoreEffectColor;

    [SerializeField] public float invincibilityTime;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
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
        pc = GetComponentInParent<PlayerController>();

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
    new void Update()
    {
        if (!pc.isDead) { 
            base.Update();
            checkRecoverArmor();
        }
    }


    protected override void RestoreHealthBody(int _hp)
    {
        RestoreEffect();
        UpdateStatePlane();
    }


    private void RestoreEffect()
    {
        AudioManager.Instance.PlaySound3WithTime("Voices/Health", 0.0f);
        //加一点粒子效果
        if (restoreEffectPathStrInRes.Length != 0)
        {
            GameObject effectPSOb = Instantiate(Resources.Load(restoreEffectPathStrInRes) as GameObject, player.transform.position, Quaternion.identity);
            effectPSOb.transform.parent = player.transform;
            render.color = restoreEffectColor;
        }
        StartCoroutine("RenderBackOriginColor");
    }

    IEnumerator RenderBackOriginColor()
    {
        yield return new WaitForSeconds(restoreEffectColorTime);
        render.color = originalColor;
    }

    public void UpdateStateUIBody()
    {
        UpdateStatePlane();
    }

    private void UpdateStatePlane()
    {
        spController.changeHealth(health, maxHealth);
        spController.changeArmor(armor, maxArmor);
        spController.changeMana(mana, maxMana);
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

    protected override void BackToUnderControlBody()
    {
        base.BackToUnderControlBody();
        pc.isOutControl = false;
    }
    protected override void TurnOutControlBody()
    {
        base.TurnOutControlBody();
        pc.isOutControl = true;
    }

    protected override bool ReceiveDamageWithRepelVectorBody(float _damage, Vector3 _repelVector)
    {
        bool isDeath = false;
        if (_damage > 0) {
            AudioManager.Instance.PlaySound("Voices/Damaged3");
            isDeath = ReduceHealthBody(_damage);
            RenderWhiteAndTurnInvincibilityLayer(isDeath);
            PlayerBloodEffectAnim();
        }
        return isDeath;

    }

    public void PlayerBloodEffectAnim()
    {
        GameObject gb = Instantiate((GameObject)Resources.Load("Effect/Player/PlayerBlood"));
        gb.transform.parent = player.transform;
        gb.transform.position = player.transform.position;

        Instantiate((GameObject)Resources.Load("Effect/BloodArea"), transform.position, Quaternion.identity);
    }


    protected bool ReduceHealthBody(float _reduceValue)
    {
        bool isDeath = false;
        int floorValue = Mathf.FloorToInt(_reduceValue);

        //掉血粒子效果 （可以设置专门的出血位）
        Instantiate(psEffect, transform.position, Quaternion.identity);
        //掉血数值 (暴击值和普通值这里应该通过配置设置不同效果)
        Vector3 tmp = transform.position;
        tmp += new Vector3(0, canvasDamageOffsetY,0);
        Instantiate(canvasDamage, tmp, Quaternion.identity).GetComponent<DamageText>().setDamageText(floorValue);
        //扣血
        isDeath = !ReceiveDamage(floorValue);
        return isDeath;

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
            health -= Mathf.Abs(temp);
            if (health <= 0)
            {
                //血量不足以吸收伤害
                health = 0;
                receiveSuccess = false;
                //TODO

            }
        }
        UpdateStateUI();
        return receiveSuccess;
    }

    protected override void DeathBody()
    {
        base.DeathBody();
        //不受控
        player.layer = LayerMask.NameToLayer("InvincibilityLayer");

        pc.isDead = true;

        Time.timeScale = 0.5f;
        //怪物停止
        GameObject[] eob = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject eb  in eob)
        {
            eb.GetComponent<Enemy>().isPause = true;
        }

        //播放音乐
        AudioManager.Instance.PlaySoundWithTime("Voices/GameOver", 0.0f);
        
        //拉近镜头
        CMFollowControl.Instance.PlayerDead();
        //死亡动画
        Sequence s = DOTween.Sequence();
        pc.animator.SetTrigger("Dead");
        s.Join(pc.transform.DOJump(transform.position - Vector3.right * 1.0f, 1.0f, 1, 1.0f));
        s.AppendCallback(() => { pc.GetComponent<Rigidbody2D>().velocity = Vector3.zero; });

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

    private void RecoverArmor(int i)
    {
        armor += 1;
        armor = armor >= maxArmor ? maxArmor : armor;
        UpdateStateUI();
    }

    private void UpdateStateUI()
    {
        UpdateStateUIBody();
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




    private void RenderWhiteAndTurnInvincibilityLayer(bool isDead)
    {
        isReceiveDamage = false;
        render.color = Color.red;
        StartCoroutine(BackToOriginalColorAndUninvincibility(isDead));
        player.layer = LayerMask.NameToLayer("InvincibilityLayer");
    }

    IEnumerator BackToOriginalColorAndUninvincibility(bool isDead)
    {
        isLayerShake = true;
        StartCoroutine("LayerShake");
        yield return new WaitForSeconds(invincibilityTime);
        isLayerShake = false;
        if (!isDead)
        {
            render.color = originalColor;
            isReceiveDamage = true;
            player.layer = LayerMask.NameToLayer("Player");
        }
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


}
