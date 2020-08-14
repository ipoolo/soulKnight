using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class NPC : Entity, BuffReceiverInterFace
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
    protected float recoverArmorCounter;
    protected float recoverArmorInterruptCounter;



    public Vector3 buffPositionOffset;

    private float outControlTime = 0.05f;
    public bool isReceiveDamage = true;

    [HideInInspector] public bool isOutControl;
    [HideInInspector] public Rigidbody2D rigid2d;
    public SpriteRenderer sr;

    //buff
    private List<Buff> buffList = new List<Buff>();

    public bool isPause = false;

    [Header("shadowEffect")]
    public Color trailColor;
    public Color fadeColor;
    public float fadeTime;

    public void Start()
    {
        ConfigDefalut();
    }

    void ConfigDefalut()
    {
        rigid2d = GetComponentInParent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void Update()
    {
        
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

    protected virtual void RestoreHealthBody(int _hp)
    {
        //body都是预留给子类实现的,其实可以写个Interface 强制要求子类实现,代码阅读起来容易点

    }


    public bool canReceiveRepel = true;


    //受伤
    public void ReceiveDamageWithOutRepel(float _damage)
    {
        ReceiveDamageWithRepelVector(_damage, Vector3.zero);
    }

    public void ReceiveDamageWithRepelVector(float _damage, Vector3 _repelVector)
    {
        if (!isPause) { 
            if (isReceiveDamage)
            {
                if (canReceiveRepel)
                {
                    TurnOutControl();
                    rigid2d.velocity = _repelVector / outControlTime;
                    StartCoroutine("BackToUnderControl");
                }
                if (ReceiveDamageWithRepelVectorBody(ExcuteHittedBuffEffect(_damage), _repelVector))
                {
                    DeathBody();
                }

            }
        }
    }

    
    protected virtual bool ReceiveDamageWithRepelVectorBody(float _damage, Vector3 _repelVector)
    {
        return false;
    }

    protected virtual void DeathBody()
    {

    }
    private void TurnOutControl()
    {
        isOutControl = true;
        TurnOutControlBody();
    }

    protected virtual void TurnOutControlBody()
    {
        
    }

    IEnumerator BackToUnderControl()
    {
        yield return new WaitForSeconds(outControlTime);
        isOutControl = false;
        BackToUnderControlBody();
    }

    protected virtual void BackToUnderControlBody()
    {

    }

    public void SpwanSilderShadow()
    {
        Sequence s = DOTween.Sequence();
        //创建一个阴影
        GameObject gb = (GameObject)Resources.Load("Materials/Shadow");
        Transform currentshadow = Instantiate(gb).transform;
        s.AppendCallback(() => currentshadow.position = transform.position);
        if (Vector2.Dot(transform.right, Vector2.right) > 0)
        {
            s.AppendCallback(() => currentshadow.GetComponent<SpriteRenderer>().flipX = false);
        }
        else
        {
            s.AppendCallback(() => currentshadow.GetComponent<SpriteRenderer>().flipX = true);
        }

        s.AppendCallback(() => currentshadow.GetComponent<SpriteRenderer>().sprite = sr.sprite);
        s.Append(currentshadow.GetComponent<SpriteRenderer>().material.DOColor(trailColor, 0));
        s.AppendCallback(() => FadeSprite(currentshadow));
    }

    private void FadeSprite(Transform shadownTransform)
    {
        shadownTransform.GetComponent<SpriteRenderer>().material.DOKill();
        shadownTransform.GetComponent<SpriteRenderer>().material.DOColor(fadeColor, fadeTime);
    }



}