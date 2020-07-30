using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAreaHitboxController : MonoBehaviour
{
    public DoorAreaController dac;
    // Start is called before the first frame update
    void Start()
    {
        ConfigDefault();
    }

    public void ConfigDefault()
    {
        dac = GetComponentInParent<DoorAreaController>();
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("B");
        if (other.CompareTag("Player"))
        {
            Debug.Log("A");
            Rigidbody2D palyerRigid2d = other.GetComponent<Rigidbody2D>();
            dac.ReceivePlayerExit(palyerRigid2d.velocity);
        }
    }


}
