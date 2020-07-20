using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float radius;
    private Vector3 dropPosition;

    private bool canReceiver;

    // Start is called before the first frame update
    void Start()
    {
        //计算随机位移目标
        calculaterDropPosition();
        canReceiver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.SqrMagnitude(dropPosition - transform.position) < 0.1f)
        {
            canReceiver = true;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, dropPosition,0.2f);
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
            Debug.Log("canReceiver");
            Destroy(gameObject);
        }
    }
}
