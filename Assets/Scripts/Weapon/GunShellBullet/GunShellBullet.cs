using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShellBullet : MonoBehaviour
{
    private PlayerController playerC;

    private void Awake()
    {
        playerC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Sequence s = DOTween.Sequence();
        //float randomX = Random.Range(0.0f, 0.5f);
        //float randomY = Random.Range(0.5f, 0.7f);
        //s.Join(transform.DOMove(new Vector3(transform.position.x + playerC.transform.right.normalized.x  * -1 * randomX, transform.position.y + randomY),0.5f));
        //s.Join(transform.DORotate(new Vector3(0,0,1800.0f),0.5f));

        float randomX = Random.Range(-1.5f, -0.5f);
        float randomY = Random.Range(-0.6f, -0.1f);

        //s.Append(transform.DOMove(new Vector3(transform.position.x + playerC.transform.right.normalized.x * -1 * randomX, transform.position.y + randomY), 1.0f));
        //s.Join(transform.DORotate(new Vector3(0, 0, 1800.0f), 1.0f));

        s.Join(transform.DOJump(new Vector3(transform.position.x + playerC.transform.right.normalized.x * randomX, transform.position.y + randomY), 1.3f,1,1.0f));
        s.Join(transform.DORotate(new Vector3(0, 0, 1800.0f), 0.5f));

        s.AppendCallback(()=>
        {
            GetComponent<SpriteRenderer>().sortingLayerName = "BackGround"; 
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
