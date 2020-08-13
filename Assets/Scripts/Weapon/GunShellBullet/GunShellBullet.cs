using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GunShellBullet : MonoBehaviour
{
    private PlayerController playerC;
    private Rigidbody2D rb;
    private BoxCollider2D collider;

    private void Awake()
    {
        playerC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
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
        s.Append(rb.DOJump(new Vector3(transform.position.x + playerC.transform.right.normalized.x * randomX, transform.position.y + randomY), 1.3f,1,1.0f));
        s.Join(transform.DORotate(new Vector3(0, 0, 1800.0f), 1.0f));
        s.AppendCallback(()=>
        {
            GetComponent<SpriteRenderer>().sortingLayerName = "BackGround";
            rb.velocity = new Vector2(0, 0);
            collider.enabled = false;

        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //配置了只碰撞强
    }
}
