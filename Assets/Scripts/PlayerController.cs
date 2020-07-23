using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float moveSpeed;


    private Rigidbody2D rid2d;
  
    private Animator animator;
    private WeaponPoint weaponPoint;

    public List<InteractionInterface> interactionList;

    private void Awake()
    {
        interactionList = new List<InteractionInterface>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rid2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        weaponPoint = GetComponentInChildren<WeaponPoint>();
    }

    // Update is called once per frame
    void Update()
    {
        checkAttack();
        checkToward();
        checkInteractionList();
        //test
        test();
    }

    public void test()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject.FindGameObjectWithTag("PlayerStateController").GetComponent<PlayerStateController>().coinAdd(2);
        }

        if (Input.GetMouseButtonDown(1))
        {
            GameObject.FindGameObjectWithTag("PlayerStateController").GetComponent<PlayerStateController>().coinReduce(1);
        }
    }

    private void FixedUpdate()
    {
        playerMove();
        
    }

    void playerMove()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        rid2d.velocity = new Vector2( moveX * moveSpeed , moveY * moveSpeed);

        if(rid2d.velocity.sqrMagnitude > 0.2f)
        {
            animator.SetBool("Run", true);
        }
        else
        {
            animator.SetBool("Run", false);
        }
    }

    void checkAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //attack
            if (!weaponPoint.currWeapon.isStoragePowerWeapon) { 
                weaponPoint.currWeapon.Attack();
            }
            else
            {
                //StoragePower
                weaponPoint.currWeapon.StoragePower();
            }

        }
        else if(Input.GetMouseButtonUp(0))
        {
            if (weaponPoint.currWeapon.isStoragePowerWeapon && weaponPoint.currWeapon.isStoragePower)
            {
                weaponPoint.currWeapon.Attack();
            }
        }

    }

    void checkToward()
    {
        
        if(Camera.main.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0,0,0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180.0f, 0);
        }
    }
    

    public void checkInteractionList()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(interactionList.Count > 0)
            {
                InteractionInterface interaction = interactionList[interactionList.Count - 1];
                interaction.InteractionBody();
            }
        }
    }

    public void Add2InteractionList(InteractionInterface ob)
    {
        Debug.Log("Add2InteractionList");

        if (ob is InteractionInterface)
        {
            //add2list
            if (interactionList.Count > 0)
            {
                //last hide
                InteractionInterface interaction = interactionList[interactionList.Count - 1];
                interaction.HideFoucsArror();
            }
            //add
            interactionList.Add(ob);
            //new last show
            interactionList[interactionList.Count - 1].ShowFoucsArror();

        }

        Debug.Log("listA:" + interactionList.Count);
        
    }

    public void remove2InteractionList(InteractionInterface ob)
    {

        if (ob is InteractionInterface)
        {
            //removeList
            if (interactionList.Count > 0)
            {
                //是否是最后一个
                if (interactionList.IndexOf(ob) == (interactionList.Count - 1))
                {
                    Debug.Log("listR0:" + interactionList.Count);
                    ob.HideFoucsArror();
                }
                interactionList.Remove(ob);
                
                Debug.Log("listR2:" + interactionList.Count);
                if (interactionList.Count > 0)
                {
                    //倒数第二个显示(现在的倒数第一个)
                    interactionList[interactionList.Count - 1].ShowFoucsArror();
                }
            }

        }
        Debug.Log("listR3:" + interactionList.Count);

    }

}
