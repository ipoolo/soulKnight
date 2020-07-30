using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private GameObject playerGb;
    [SerializeField] public float followStepScale;

    // Start is called before the first frame update
    void Start()
    {
        playerGb = GameObject.FindGameObjectWithTag("Player");
    }
    
    // Update is called once per frame
    void Update()
    {
        followPlayer();
    }

    void followPlayer()
    {
        if(playerGb != null) {
            //position.y 为了保持Z方向不变
            
            Vector3 temp = Vector3.Lerp(transform.position, new Vector3(playerGb.transform.position.x, playerGb.transform.position.y, transform.position.z),
                followStepScale);
            transform.position = temp;
        }
    }
}
