using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private GameObject playerGb;
    [SerializeField] public float followStepScale;
    [SerializeField] private Transform rightTop;
    [SerializeField] private Transform leftBottom;

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
            transform.position = new Vector3(Mathf.Clamp(temp.x, leftBottom.position.x, rightTop.position.x),
                Mathf.Clamp(temp.y, leftBottom.position.y, rightTop.position.y),transform.position.z);
        }
    }

    public void setCameraLimitSize(Transform _topRight, Transform _bottomLeft)
    {
        rightTop = _topRight;
        leftBottom = _bottomLeft;
    }
}
