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
    void Start()
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
        base.Update();
    }

    //RestoreHealth
    //public void RestoreHealth(int _hp)
    //{
    //    if(_hp > 0) { 
    //        int tmp = health + _hp;
    //        health = tmp > maxHealth ? maxHealth : tmp;
    //        //restoreEffect
    //        RestoreEffect();
    //        UpdateStatePlane();
    //    }
    //}

    public override void RestoreHealthBody(int _hp)
    {
        RestoreEffect();
        UpdateStatePlane();
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

    public override void UpdateStateUIBody()
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

    //receiveDamage

    public override void ReceiveDamageWithRepelVectorBody(float _damage, Vector3 _repelVector)
    {
        if (isReceiveDamage) { 
           isReceiveDamage = false;
            RenderWhiteAndTurnInvincibilityLayer();
            ReduceHealth(ExcuteHittedBuffEffect(_damage));

        }
        if (isOutControl)
        {
            pc.isOutController = true;
        }
    }


    public override void BackToUnderControlBody(bool _isOutControl)
    {
        pc.isOutController = _isOutControl;
    }

    public override void ReduceHealthBody(float _reduceValue)
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


}
