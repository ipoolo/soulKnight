using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    private Weapon fireWeapon;
    private Animator animator;
    private float lifeTime = 1.0f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if(animator != null) { 
            lifeTime = animator.runtimeAnimatorController.animationClips[0].length + 0.5f;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void configSwordAttack(Weapon from)
    {
        fireWeapon = from;
    }

    public void Fire()
    {
        animator.SetTrigger("Fire");
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        //近战武器触发伤害
        if (other.CompareTag("Enemy") || other.CompareTag("Obstacle"))
        {
            NPC npc = other.GetComponent<NPC>();
            npc.ReceiveDamageWithRepelVector(fireWeapon.ExcuteHittingBuffEffect(fireWeapon.damage), fireWeapon.weaponPoint.transform.right);
        }
    }



}
