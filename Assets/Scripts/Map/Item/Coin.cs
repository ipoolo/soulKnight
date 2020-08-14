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

        if (senseRaidus == 0)
        {
            senseRaidus = 10;
        }

        if (followSpeed == 0)
        {
            followSpeed = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(dropPosition, transform.position) < 0.1f)
        {
            canReceiver = true;
        }
        else
        {
            MoveToRandomLocation();

        }

        if (canReceiver)
        {
            if (isMovePlayer)
            {
                MoveToPlayer();
            }
            else
            {
                checkIsFollowToPlayer();
            }
        }
    }

    private void MoveToRandomLocation()
    {
        transform.position = Vector3.Lerp(transform.position, dropPosition, 0.2f);
    }

    private void MoveToPlayer()
    {
        movePlayerTimer += Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, movePlayerTimer * followSpeed * 2);

    }

    public void checkIsFollowToPlayer()
    {
        Vector3 playerPosition = player.transform.position;
        if ((playerPosition - transform.position).magnitude < senseRaidus)
        {
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
        if (canReceiver && other.CompareTag("Player"))
        {
            AudioManager.Instance.PlaySound("Voices/" + "Money");
            psc.CoinAdd(coinValue);
            Destroy(gameObject);
        }
    }
}
