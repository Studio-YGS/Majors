using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossyTheWatcher : MonoBehaviour
{
    Animator animator;

    public bool conditionIsTrue;
    public bool lerpPosition;
    public bool weights;
    public bool hands;

    public Transform lookAtTransform;
    public Transform faceToTransform;

    public float turnRate;

    [Range(0,1)] public float headLookAtWeight;
    //[Range(0,1)] public float spineLookAtWeight;
    //[Range(0,1)] public float spinerLookAtWeight;
    

    public Lighthouse lighthouse;
    public float hiddenYOffset;

    private Transform lightHouseBase;
    private Vector3 hiddenPosition;

    [Header("TestWeights")]
    [Range(0,1)]public float heightWeight;
    [Range(0,1)]public float leftWeight;
    [Range(0,1)]public float rightWeight;
    [Range(0, 1)] public float leftGoal;
    [Range(0, 1)] public float rightGoal;


    private void Start()
    {
        animator = GetComponent<Animator>();
        InitFromLighthouse();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetLayerWeight(1, animator.GetFloat("LayerWeight"));

        if(lerpPosition)
        {
            if(weights)
            {
                transform.position = Vector3.Lerp(hiddenPosition, lightHouseBase.position, heightWeight);
            }
            else
            {
                transform.position = Vector3.Lerp(hiddenPosition, lightHouseBase.position, animator.GetFloat("HeightWeight"));
            }
            
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if(animator)
        {
            if(weights)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftWeight);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftWeight);
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, rightWeight);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, rightWeight);
            }
            else
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, animator.GetFloat("IKLeftHandWeight"));
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, animator.GetFloat("IKLeftHandWeight"));
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, animator.GetFloat("IKRightHandWeight"));
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, animator.GetFloat("IKRightHandWeight"));
            }
            

            //Noggin Malarkey
            if (lookAtTransform)
            {
                if (conditionIsTrue)
                {
                    animator.SetLookAtPosition(lookAtTransform.position);
                    animator.SetLookAtWeight(Mathf.Lerp(1f, headLookAtWeight, animator.GetFloat("IKLookWeight")));
                }
                else animator.SetLookAtWeight(0f);

                Quaternion stepRotation = Quaternion.identity;
                Quaternion rotation = Quaternion.LookRotation(faceToTransform.position - transform.position);

                rotation.eulerAngles = new Vector3(0, rotation.eulerAngles.y, 0);
                stepRotation = Quaternion.RotateTowards(transform.rotation, rotation, turnRate * Time.deltaTime);

                transform.rotation = stepRotation;
            }

            if(hands)
            {
                
                if (weights)
                {
                    animator.SetIKPosition(AvatarIKGoal.LeftHand, Vector3.Lerp(lighthouse.leftHandGuideGoal.position, lighthouse.leftHandFinalGoal.position, leftGoal));
                    animator.SetIKRotation(AvatarIKGoal.LeftHand, Quaternion.Slerp(lighthouse.leftHandGuideGoal.rotation, lighthouse.leftHandFinalGoal.rotation, leftGoal));
                    animator.SetIKPosition(AvatarIKGoal.RightHand, Vector3.Lerp(lighthouse.rightHandGuideGoal.position, lighthouse.rightHandFinalGoal.position, rightGoal));
                    animator.SetIKRotation(AvatarIKGoal.RightHand, Quaternion.Slerp(lighthouse.rightHandGuideGoal.rotation, lighthouse.rightHandFinalGoal.rotation, rightGoal));
                }
                else
                {
                    animator.SetIKPosition(AvatarIKGoal.LeftHand, Vector3.Lerp(lighthouse.leftHandGuideGoal.position, lighthouse.leftHandFinalGoal.position, animator.GetFloat("IKLeftGoal")));
                    animator.SetIKRotation(AvatarIKGoal.LeftHand, Quaternion.Slerp(lighthouse.leftHandGuideGoal.rotation, lighthouse.leftHandFinalGoal.rotation, animator.GetFloat("IKLeftGoal")));
                    animator.SetIKPosition(AvatarIKGoal.RightHand, Vector3.Lerp(lighthouse.rightHandGuideGoal.position, lighthouse.rightHandFinalGoal.position, animator.GetFloat("IKRightGoal")));
                    animator.SetIKRotation(AvatarIKGoal.RightHand, Quaternion.Slerp(lighthouse.rightHandGuideGoal.rotation, lighthouse.rightHandFinalGoal.rotation, animator.GetFloat("IKRightGoal")));
                }
            }
        }
    }


    public void InitFromLighthouse()
    {
        lightHouseBase = lighthouse.selfTransform;

        hiddenPosition = new Vector3(lightHouseBase.position.x, lightHouseBase.position.y + hiddenYOffset, lightHouseBase.position.z);
    }
}
