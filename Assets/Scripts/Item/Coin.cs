using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float radius;
    private Vector3 dropPosition;
    public int coinValue;
    public float followSpeed;

    private PlayerStateController psc;
    private GameObject player;

    public float senseRaidus;
    private bool canReceiver;
    private bool isMovePlayer;
    private float movePlayerTimer;


    // Start is called before the first frame update
    void Start()
    {
        //计算随机位移目标
        calculaterDropPosition();
        configDefault();

    }


    private void configDefault()
    {
        canReceiver = false;
        psc = GameObject.FindGameObjectWithTag("PlayerStateController").GetComponent<PlayerStateController>();
        player = GameObject.FindGameObjectWithTag("Player");
        if (coinValue == 0)
        {
            coinValue = 1;
        }

        if(senseRaidus == 0)
        {
            senseRaidus = 10;
        }

        if(followSpeed == 0)
        {
            followSpeed = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(dropPosition ,transform.position) < 0.1f)
        {
            canReceiver = true;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, dropPosition,0.2f);
        }

        if (canReceiver)
        {
            if (isMovePlayer)
            {
                movePlayerTimer += Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, movePlayerTimer * followSpeed);
            }
            else { 
                checkIsFollowToPlayer();
            }
        }
    }

    public void checkIsFollowToPlayer()
    {
        Vector3 playerPosition = player.transform.position;
        if ((playerPosition - transform.position).magnitude < senseRaidus)
        {
            //在感知范围（只要在范围内一次）
            isMovePlayer = true;
        }

    }


    void calculaterDropPosition()
    {
        float ramX = Random.Range(transform.position.x - radius, transform.position.x + radius);
        float ramY = Random.Range(transform.position.y - radius, transform.position.y + radius);
        dropPosition = new Vector3(ramX, ramY, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(canReceiver && other.CompareTag("Player"))
        {
            psc.CoinAdd(coinValue);
            Destroy(gameObject);
        }
    }
}
