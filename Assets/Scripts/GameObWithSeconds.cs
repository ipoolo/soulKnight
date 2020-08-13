using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObWithSeconds : MonoBehaviour
{
    public float liftTime;
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Destroy(this.gameObject, liftTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
