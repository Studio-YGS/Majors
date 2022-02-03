using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tactical;

public class CrossyWeapon : MonoBehaviour
{
    public Animator wielderAnimator;

    public float weaponDamage = 10;
    public bool Attacking { get { return !wielderAnimator.GetCurrentAnimatorStateInfo(1).IsName("NotAttacking"); } }
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
