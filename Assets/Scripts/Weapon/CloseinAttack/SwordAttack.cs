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
        if (other.CompareTag("Enemy"))
        {
            Enemy emeny = other.GetComponent<Enemy>();
            Transform temp = GameObject.FindGameObjectWithTag("WeaponPoint").GetComponent<Transform>();
            float zRotaion = temp.rotation.eulerAngles.z;
            Vector3 tempV = Quaternion.AngleAxis(zRotaion, Vector3.forward) * Vector3.right;
            emeny.ReceiveDamageWithRepelVector(ExcuteHittingBuffEffect(fireWeapon.damage), tempV);
        }
        
    }
    private float ExcuteHittingBuffEffect(float _damage)
    {
        float tmp = _damage;
        if (fireWeapon.castor is BuffReceiverInterFace)
        {
            List<Buff> buffList = (fireWeapon.castor as BuffReceiverInterFace).GetBuffList();
            foreach (Buff buff in buffList)
            {
                if (buff is BuffReceiveHittingDamageInterFace)
                {
                    tmp = ((BuffReceiveHittingDamageInterFace)buff).BuffReceiveHittingDamageInterFaceBody(tmp);
                }
            }
        }

        return tmp;
    }


}
