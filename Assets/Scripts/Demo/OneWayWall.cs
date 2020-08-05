using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayWall : MonoBehaviour
{
    public Vector2 passDirection = new Vector2(0,1);
    private Collider2D c2d;

    // Start is called before the first frame update
    void Start()
    {
        c2d = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("other.GetComponent<Rigidbody2D>().velocity"+ other.GetComponent<Rigidbody2D>().velocity);
        Vector2 otherVelocity = other.GetComponent<Rigidbody2D>().velocity;
        if(Vector2.Dot(otherVelocity , passDirection)< 0) {
            c2d.isTrigger = false;
        } 
    }
}
