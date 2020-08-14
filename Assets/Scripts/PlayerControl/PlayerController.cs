using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class PlayerController : MonoBehaviour
{
    [HideInInspector]public PlayerStateController playerStateController;

    private Rigidbody2D rid2d;
  
    public Animator animator;
    private WeaponPoint weaponPoint;

    public List<InteractionInterface> interactionList;

    [HideInInspector]public bool isOutControl;
    [HideInInspector] public bool isDead = false;

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
        isOutControl = false;
        playerStateController = GetComponentInChildren<PlayerStateController>();
    }


    // Update is called once per frame
    void Update()
    {
        if (!isDead) {
            checkAttack();
       
            checkInteractionList();
            //test
            CheckInput();
        }
    }

    public void CheckInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject.FindGameObjectWithTag("PlayerStateController").GetComponent<PlayerStateController>().CoinAdd(2);
        }

        if (Input.GetMouseButtonDown(1))
        {
            GameObject.FindGameObjectWithTag("PlayerStateController").GetComponent<PlayerStateController>().coinReduce(1);

            Instantiate(Resources.Load("Buff/BuffSpeedIncrementPresent") as GameObject, Vector3.zero, Quaternion.identity).GetComponent<Buff>().BuffLoad(gameObject) ;

        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            weaponPoint.SwitchWeapon();
        }
    }

    private void FixedUpdate()
    {
        if (!isDead) { 
            if (!isOutControl) { 
                playerMove();
            }
        }

    }

    void playerMove()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        rid2d.velocity = new Vector2( moveX * playerStateController.moveSpeed, moveY * playerStateController.moveSpeed);

        if(rid2d.velocity.magnitude > 0.2f)
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
            weaponPoint.playerPressAttackDown();
        }
        else if(Input.GetMouseButtonUp(0))
        {
            weaponPoint.playerPressAttackUp();
        }

    }

    void checkToward()
    {
        if(Camera.main.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x)
        {
            Debug.Log("A");
            transform.rotation = Quaternion.Euler(0,0,0);
        }
        else
        {
            Debug.Log("B");
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

    private void LateUpdate()
    {
        if (!isDead)
        {
            checkToward();
        }
    }

    public void Add2InteractionList(InteractionInterface ob)
    {

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
                    ob.HideFoucsArror();
                }
                interactionList.Remove(ob);
                
                if (interactionList.Count > 0)
                {
                    //倒数第二个显示(现在的倒数第一个)
                    interactionList[interactionList.Count - 1].ShowFoucsArror();
                }
            }

        }

    }


}
