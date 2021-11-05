using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Lighthouse : MonoBehaviour
{
    public Transform selfTransform;
    public Transform leftHandFinalGoal;
    public Transform rightHandFinalGoal;

    public Transform leftHandGuideGoal;
    public Transform rightHandGuideGoal;

    public float minAngle;
    public float maxAngle;

    /*[HideInInspector] */public float currentAngle;

    [SerializeField] private float gizmoCheckRadius;
    [SerializeField] private float gizmoHandGoalRadius;
    [SerializeField] private float gizmoGuideGoalRadius;
    [SerializeField] private Color radialColour;
    [SerializeField] private Color handGoalColour;
    [SerializeField] private Color guideGoalColour;


    private void Awake()
    {
        selfTransform = transform;
    }



    public void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        Handles.color = radialColour;
        Handles.DrawSolidDisc(selfTransform.position, selfTransform.up, gizmoCheckRadius);

        Gizmos.color = handGoalColour;
        Gizmos.DrawSphere(leftHandFinalGoal.position, gizmoHandGoalRadius);
        Gizmos.DrawSphere(rightHandFinalGoal.position, gizmoHandGoalRadius);

        Gizmos.color = guideGoalColour;
        Gizmos.DrawSphere(leftHandGuideGoal.position, gizmoGuideGoalRadius);
        Gizmos.DrawSphere(rightHandGuideGoal.position, gizmoGuideGoalRadius);

#endif
    }

}
