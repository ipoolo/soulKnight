using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHelper : MonoBehaviour
{
    private PlayerController playerC;
    private Rigidbody2D playerRb;
    private CapsuleCollider2D capC;

    private float verticalRayCastDistance;
    private float horizontalRayCastDistance;

    public bool canMoveUp;
    public bool canMoveRight;
    public bool canMoveDown;
    public bool canMoveLeft;
    public float radiusOffset;

    private void Awake()
    {
        playerC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        capC = GetComponent<CapsuleCollider2D>();
        playerRb = playerC.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!playerC.isDead) { 
            CheckCanMove();
            CheckPlayerHelper();
        }
    }


    public LayerMask raycastLayerMasks;
    private void CheckCanMove()
    {
        float radius = capC.size.x > capC.size.y ? capC.size.y / 2.0f : capC.size.x / 2.0f;
        radiusOffset = radiusOffset > radius ? radius : radiusOffset;
        Vector3 colliderCenterPosition = transform.position + new Vector3(capC.offset.x, capC.offset.y);
        //top
        tmp = new Dictionary<Vector2, float>();
        canMoveUp = CheckRayTouch(colliderCenterPosition + Vector3.up.normalized * capC.size.y/2, radiusOffset);
        canMoveLeft = CheckRayTouch(colliderCenterPosition + Vector3.left.normalized * capC.size.x/2, radiusOffset);
        canMoveDown = CheckRayTouch(colliderCenterPosition + Vector3.down.normalized * capC.size.y / 2, radiusOffset);
        canMoveRight = CheckRayTouch(colliderCenterPosition + Vector3.right.normalized * capC.size.x / 2, radiusOffset);

    }


    private bool CheckRayTouch(Vector3 point,float raidius)
    {

        Collider2D hitCol = Physics2D.OverlapCircle(point, raidius, raycastLayerMasks);
        tmp.Add(point, raidius); ;
        if (!hitCol)
        {
            return true;
        }
        return false;
    }

    //绘制gizmos用
    Dictionary<Vector2, float> tmp = new Dictionary<Vector2, float>();
    void OnDrawGizmos()
    {
        foreach (KeyValuePair<Vector2,float> kp in tmp) {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(kp.Key, kp.Value);
            
        }
    }

    private void CheckPlayerHelper()
    {

        Vector2 extraSpeed = Vector2.right.normalized * playerC.playerStateController.moveSpeed;
        if (Input.GetKey(KeyCode.W) && !canMoveUp)
        {
            if (canMoveRight && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            {
                playerRb.velocity += extraSpeed;
            }
        }
        if (Input.GetKey(KeyCode.S) && !canMoveDown)
        {
            if (canMoveLeft && ! Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            {
                playerRb.velocity += extraSpeed * -1;
            }

        }
        if (Input.GetKey(KeyCode.D) && !canMoveRight)
        {
            if (canMoveDown && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
            {
                Vector3 tmp = extraSpeed;
                extraSpeed = Quaternion.Euler(0, 0, -90) * tmp;
                playerRb.velocity += extraSpeed;
            }

        }
        if (Input.GetKey(KeyCode.A) && !canMoveLeft)
        {
            if (canMoveUp && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
            {
                Vector3 tmp = extraSpeed;
                extraSpeed = Quaternion.Euler(0, 0, 90) * tmp;
                playerRb.velocity += extraSpeed;
            }

        }
        //墙壁没有换材质,现在还有摩擦力,所以实际速度不是 设置速度
    }
}
