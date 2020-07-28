using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchSense : MonoBehaviour
{
    public CircleCollider2D touchCollider;
    public Enemy ownerEnemy;

    private void Awake()
    {
        defaultConfig();
    }
    void Start()
    {

    }

    void defaultConfig()
    {
        touchCollider = GetComponent<CircleCollider2D>();
        ownerEnemy = GetComponentInParent<Enemy>();

    }

    public void setSenceDinstance(float distance)
    {
        touchCollider.radius = distance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("OnTriggerEnter2D true");
            ownerEnemy.isTouchSensePalyer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("OnTriggerExit2D false");
            ownerEnemy.isTouchSensePalyer = false;
        }
    }
}
