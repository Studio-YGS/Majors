using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossyTheWatcher : MonoBehaviour
{
    [HideInInspector] public Animator animator;

    public Material titanGlow;

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
    public List<GameObject> Rendy = new List<GameObject>();

    public Lighthouse lighthouse;

    private Transform lightHouseBase;
    private Vector3 hiddenPosition;

    public float eyeGlowUpRate;
    public float eyeGlowDownRate;

    bool eyeGlowing;
    bool skinned;
    bool skinCheck;

    [Header("TestWeights")]
    [Range(0,1)]public float heightWeight;
    [Range(0,1)]public float leftWeight;
    [Range(0,1)]public float rightWeight;
    [Range(0, 1)] public float leftGoal;
    [Range(0, 1)] public float rightGoal;


    [HideInInspector] public int m_state;


    [HideInInspector] public bool lighthousing;
    [HideInInspector] public bool hidingTitan;
    [HideInInspector] public bool lightInit = false;

    [HideInInspector] public bool isTutorial = true;
    [HideInInspector] public bool allowHide = true;

    private void Start()
    {
        animator = GetComponent<Animator>();
        transform.rotation = lighthouse.selfTransform.rotation;
        //SetPosToLighthouse();
        if(faceToTransform == null) faceToTransform = GameObject.Find("Fps Character").transform;
        if(lookAtTransform == null) lookAtTransform = GameObject.Find("FirstPersonCharacter").transform;

        SkinningMyBoy();
    }

    // Update is called once per frame
    void Update()
    {
        if(lerpPosition)
        {
            if(weights)
            {
                transform.position = Vector3.Lerp(lighthouse.hiddenPosition, lighthouse.selfTransform.position, heightWeight);
            }
            else
            {
                transform.position = Vector3.Lerp(lighthouse.hiddenPosition, lighthouse.selfTransform.position, animator.GetFloat("HeightWeight"));
            }
            
        }

        if(!isTutorial)
        {
            if (!lighthousing && !hidingTitan && lightInit)
            {
                if(animator.GetCurrentAnimatorStateInfo(0).IsName("TitanCrossyIdleHidden"))
                {
                    if(titanGlow.color.a != 0f)
                    {
                        Color colour = titanGlow.color;
                        colour.a = 0f;
                        titanGlow.color = colour;
                    }
                }

                if (m_state == -1 && animator.GetCurrentAnimatorStateInfo(0).IsName("TitanCrossyIdleHidden"))
                {
                    AwakenTitan();
                }
            }
            else if(hidingTitan)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("TitanCrossyIdleHidden"))
                {
                    if (titanGlow.color.a != 0f)
                    {
                        Color colour = titanGlow.color;
                        colour.a = 0f;
                        titanGlow.color = colour;
                    }
                }
            }

            if (!skinCheck) SkinningMyBoy();
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

                Rotahtee();
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

    public void SkinningMyBoy()
    {
        skinCheck = true;
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("TitanCrossyIdleHidden"))
        {
            if(!skinned && hidingTitan)
            {
                foreach (GameObject skin in Rendy)
                {
                    skin.SetActive(false);
                }
                skinned = true;
            }
        }
        else
        {
            if(skinned && !hidingTitan)
            {
                foreach (GameObject skin in Rendy)
                {
                    skin.SetActive(true);
                }
                skinned = false;
            }
        }
        skinCheck = false;
    }


    /*public void SetPosToLighthouse()
    {
        transform.position = lighthouse.selfTransform.position;
        transform.rotation = lighthouse.selfTransform.rotation;

        //lightHouseBase = lighthouse.selfTransform;

        //hiddenPosition = new Vector3(lightHouseBase.position.x, lightHouseBase.position.y + lighthouse.hiddenYOffset, lightHouseBase.position.z);
    }*/

    //Called When lighthouse Switched
    public void TitanCrossyHouse(Lighthouse newLighthouse)
    {
        lighthousing = true;
        StartCoroutine(SwitchLighthouse(newLighthouse));
    }

    public void Rotahtee()
    {
        Quaternion stepRotation;
        Quaternion rotation = Quaternion.LookRotation(faceToTransform.position - transform.position);

        rotation.eulerAngles = new Vector3(0, rotation.eulerAngles.y, 0);
        stepRotation = Quaternion.RotateTowards(transform.rotation, rotation, turnRate * Time.deltaTime);

        transform.rotation = stepRotation;
    }

    //Awakens Titan Crossy
    public void AwakenTitan()
    {
        animator.SetTrigger("TitanAwaken");
        if (!eyeGlowing) StartCoroutine(TitanEyes());
        allowHide = false;
    }

    //Hides Titan Crossy
    public void HideTitan()
    {
        animator.SetTrigger("TitanSeal");
        if (eyeGlowing)
        {
            StopCoroutine(TitanEyes());
            eyeGlowing = false;
        }
    }

    public IEnumerator SwitchLighthouse(Lighthouse lighthouseNew)
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("TitanCrossyIdleHidden"))
        {
            Debug.Log("Ough Moy Gawsh");
            HideTitan();
        }

        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("TitanCrossyIdleHidden"))
        {
            yield return null;
        }

        lighthouse = lighthouseNew;
        transform.rotation = lighthouse.selfTransform.rotation;
        //SetPosToLighthouse();

        if (m_state == -1) AwakenTitan();

        lighthousing = false;
    }

    public IEnumerator TitanEyes()
    {
        eyeGlowing = true;

        while(!animator.GetCurrentAnimatorStateInfo(0).IsName("TitanCrossyIdle"))
        {
            yield return null;
        }
        Color colour = titanGlow.color;
        while(colour.a < 1f)
        {
            colour.a += Time.deltaTime * eyeGlowUpRate;
            titanGlow.color = colour;
            yield return null;
        }
        colour.a = 1f;
        titanGlow.color = colour;

        while (colour.a > 0f)
        {
            colour.a -= Time.deltaTime * eyeGlowDownRate;
            titanGlow.color = colour;
            yield return null;
        }
        colour.a = 0f;
        titanGlow.color = colour;

        eyeGlowing = false;
    }
}
