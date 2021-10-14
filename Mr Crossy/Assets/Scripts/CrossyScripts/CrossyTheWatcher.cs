using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossyTheWatcher : MonoBehaviour
{
    Animator animator;

    public bool conditionIsTrue;

    public Transform lookAtTransform;

    public float turnRate;

    [Range(0,1)] public float headLookAtWeight;
    [Range(0,1)] public float spineLookAtWeight;
    [Range(0,1)] public float spinerLookAtWeight;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if(animator)
        {
            //Noggin Malarkey
            if (lookAtTransform)
            {
                if (conditionIsTrue)
                {
                    animator.SetLookAtPosition(lookAtTransform.position);
                    animator.SetLookAtWeight(headLookAtWeight);
                }
                else animator.SetLookAtWeight(0f);

                Quaternion rotation = Quaternion.LookRotation(lookAtTransform.position - transform.position);

            }
        }
    }
}
