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
            //今晚太困了,这里偷懒写了TODO
            float offsetX = 0;
            float offsetY = 0;
            if (gameObject.name.Equals("MapLocation"))
            {
                offsetX = 6.0f;
                offsetY = 5.5f;
            }            
            Vector3 temp = Vector3.Lerp(transform.position, new Vector3(playerGb.transform.position.x+ offsetX, playerGb.transform.position.y+ offsetY, transform.position.z),
                followStepScale);
            
            //transform.position = temp;
        }
    }
}
