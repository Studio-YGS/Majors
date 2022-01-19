using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tactical;

public class CrossyWeapon : MonoBehaviour
{
    public Animator animator;

    public float weaponDamage = 1;
    public bool Attacking { get { return !animator.GetCurrentAnimatorStateInfo(1).IsName("NotAttacking"); } }
    private void OnCollisionEnter(Collision collision)
    {
        IDamageable target;

        if(Attacking)
        {
            if((target = collision.gameObject.GetComponent(typeof(IDamageable)) as IDamageable) != null)
            {
                target.Damage(weaponDamage);
            }
        }
    }
}
