using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMFollowControl : Singleton<CMFollowControl>
{
    private CinemachineVirtualCamera cm;
    CinemachineFramingTransposer transposer;
    private GameObject player;

    [Header("Camera Range")]
    [Tooltip("镜头的可移动范围,0为全部可移动,0.5为中心点")]
    [Range(0.0f,0.5f)]
    public float leftScreenValue;
    [Tooltip("镜头的可移动范围,0为全部可移动,0.5为中心点")]
    [Range(0.0f, 0.5f)]
    public float topScreenValue;

    // Start is called before the first frame update
    void Start()
    {
        cm = GetComponent<CinemachineVirtualCamera>();
        cm.GetComponentPipeline();
        transposer = cm.GetCinemachineComponent<CinemachineFramingTransposer>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void updateCamera() 
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float ratioHW = (float)Screen.height / Screen.width;
        float screenHeightnUnity = Camera.main.orthographicSize * 2.0f;
        float screenWidthInUnity = screenHeightnUnity / ratioHW;

        
        Vector2 offsetPlayer2Mouse = mousePosition - player.transform.position;
        float newScreenX = 0.5f + ((offsetPlayer2Mouse.x / ((1 - leftScreenValue) * screenWidthInUnity )) * -1.0f *(0.5f - leftScreenValue));
        float newScreenY = 0.5f + ((offsetPlayer2Mouse.y / ((1 - topScreenValue) * screenHeightnUnity)) * 1.0f * (0.5f - topScreenValue));
        transposer.m_ScreenX = Mathf.Clamp(newScreenX, leftScreenValue, 1 - leftScreenValue);
        transposer.m_ScreenY = Mathf.Clamp(newScreenY, topScreenValue, 1 - topScreenValue);
    }

    void Update()
    {
        if (!player.GetComponentInChildren<PlayerController>().isDead) { 
            updateCamera();
        }
    }

    public void PlayerDead()
    {
        transposer.m_ScreenX = 0.5f;
        transposer.m_ScreenY = 0.5f;
        cm.m_Lens.OrthographicSize = 2.0f;
    }
}
