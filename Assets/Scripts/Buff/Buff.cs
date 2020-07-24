using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DeBuffType
{
    deBuffTypeNotDebuff,
    deBuffTypeDot,
    deBuffTypeStatedurative,//状态持续性
    deBuffTypeHittingReceiveDamage,
    deBuffTypeHittedReceiveDamage,

}

public enum BuffType
{
    buffTypeNotBuff,
    buffTypeDot,
    buffTypeStatedurative,//状态持续性
    buffTypeHittingReceiveDamage,
    buffTypeHittedReceiveDamage,

}

public enum PersistentStateTargetType
{
    noTarget,
    player,
    enemy

}


public class Buff : MonoBehaviour
{
    [SerializeField] public float duration;
    [HideInInspector]public PersistentStateTargetType targetType;
    [HideInInspector]public PlayerStateController playerStateController;
    [HideInInspector]public Enemy enemy;
    [SerializeField]public string buffPrefabsResPath = "BuffIndicator";
    [SerializeField]public GameObject buffSpritePrefab;
    [SerializeField]public GameObject buffSprite;
    private GameObject targetGb;

    //[SerializeField] public BuffType buffType;
    //[SerializeField] public BuffType buffType;

    // Start is called before the first frame update
     void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void BuffLoad(GameObject _target, PersistentStateTargetType _targetType)
    {
        targetType = _targetType;
        switch (_targetType)
        {
            case PersistentStateTargetType.enemy:
                enemy = _target.GetComponentInParent<Enemy>();
                targetGb = enemy.gameObject;
                break;
            case PersistentStateTargetType.player:
                playerStateController = _target.GetComponentInChildren<PlayerStateController>();
                targetGb = playerStateController.transform.parent.gameObject;
                playerStateController.addBuff(this);
                break;
            case PersistentStateTargetType.noTarget:
                break;
        }
        transform.position = targetGb.transform.position;
        transform.parent = targetGb.transform;
        
        if (BuffLoadBodyAndIsInvoke())
        {
            Debug.Log("BUG");
            Invoke("BuffUnLoad", duration);
        }


    }
    public virtual bool BuffLoadBodyAndIsInvoke()
    {
        bool isInvoke = true;
        //加载BUFF状态 显示效果
        if(buffPrefabsResPath != null && buffSpritePrefab ==null)
        {
            buffSpritePrefab = (GameObject)Resources.Load(buffPrefabsResPath);
        }

        if(buffSpritePrefab != null)
        {
            Vector3 buffPositionOffset = Vector3.zero;
            switch (targetType)
            {
                case PersistentStateTargetType.enemy:
                    buffPositionOffset = enemy.buffPositionOffset;
                    break;
                case PersistentStateTargetType.player:
                    buffPositionOffset = playerStateController.buffPositionOffset;
                    break;
                case PersistentStateTargetType.noTarget:
                    break;
            }
            Vector3 position = targetGb.transform.position + buffPositionOffset;
            buffSprite = Instantiate(buffSpritePrefab, position,Quaternion.identity);
            buffSprite.transform.parent = transform;
            
        }
        return isInvoke;
    }


    public void BuffUnLoad()
    {
        BuffUnLoadBody();
        if (enemy != null)
        {
            enemy.removeBuff(this);
        }
        if (playerStateController != null)
        {
            playerStateController.removeBuff(this);
        }
        Destroy(buffSprite);
        Destroy(gameObject);
    }
    public virtual void BuffUnLoadBody()
    {

    }

}
