using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;



public enum BuffType
{
    buff,
    debuff

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
    [HideInInspector]public NPC targetNpc;
    [SerializeField]public string buffPrefabsResPath = "BuffIndicator";
    [SerializeField]public GameObject buffSpritePrefab;
    [SerializeField]public GameObject buffSprite;
    public GameObject targetGb;
    [SerializeField] public BuffType buffType;

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

    public Buff BuffLoad(GameObject _target, PersistentStateTargetType _targetType)
    {
        //switch (_targetType)
        //{
        //    case PersistentStateTargetType.enemy:
        //        enemy = _target.GetComponentInChildren<Enemy>();
        //        enemy.AddBuff(this);
        //        targetGb = enemy.gameObject;
        //        break;
        //    case PersistentStateTargetType.player:
        //        playerStateController = _target.GetComponentInChildren<PlayerStateController>();
        //        targetGb = playerStateController.gameObject;
        //        playerStateController.AddBuff(this);
        //        break;
        //    case PersistentStateTargetType.noTarget:
        //        break;
        //}

        targetNpc = _target.GetComponentInChildren<NPC>();
        targetNpc.AddBuff(this);
        targetGb = targetNpc.gameObject;

        transform.position = targetGb.transform.position;
        transform.parent = targetGb.transform;

        //加载BUFF状态 显示效果
        if (buffPrefabsResPath != null && buffSpritePrefab == null)
        {
            buffSpritePrefab = (GameObject)Resources.Load(buffPrefabsResPath);
        }

        if (buffSpritePrefab != null)
        {
            Vector3 buffPositionOffset = Vector3.zero;
            //switch (targetType)
            //{
            //    case PersistentStateTargetType.enemy:
            //        buffPositionOffset = enemy.buffPositionOffset;
            //        break;
            //    case PersistentStateTargetType.player:
            //        buffPositionOffset = playerStateController.buffPositionOffset;
            //        break;
            //    case PersistentStateTargetType.noTarget:
            //        break;
            //}
            buffPositionOffset = targetNpc.buffPositionOffset;
            Vector3 position = targetGb.transform.position + buffPositionOffset;
            buffSprite = Instantiate(buffSpritePrefab, position, Quaternion.identity);
            buffSprite.transform.parent = transform;

        }

        if (BuffLoadBodyAndIsInvokeBody())
        {
            Invoke("BuffUnLoad", duration);
        }

        return this;
    }

    public void BuffUnLoad()
    {
        BuffUnLoadBody();
        //if (enemy != null)
        //{
        //    enemy.RemoveBuff(this);
        //}
        //if (playerStateController != null)
        //{
        //    playerStateController.RemoveBuff(this);
        //}

        targetNpc.RemoveBuff(this);
        Destroy(buffSprite);
        Destroy(gameObject);
    }

    public virtual bool BuffLoadBodyAndIsInvokeBody()
    {
        bool isInvoke = true;
        return isInvoke;
    }

    public virtual void BuffUnLoadBody()
    {

    }

}
