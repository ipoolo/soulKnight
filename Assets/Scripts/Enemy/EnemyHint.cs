using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHint : MonoBehaviour
{
    public Vector2 offset;
    public float lifeTime;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x + offset.x, transform.position.y + offset.y,0);
        Destroy(this.gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
