using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Lighthouse : MonoBehaviour
{
    [Header("LighthouseTransforms")]
    public Transform selfTransform;
    public Transform leftHandFinalGoal;
    public Transform rightHandFinalGoal;
    [Space(10)]
    public Transform leftHandGuideGoal;
    public Transform rightHandGuideGoal;
    [Space(10)]
    public float hiddenYOffset;
    [HideInInspector] public Vector3 hiddenPosition;
    [Header("Rotation Limits")]
    public float minAngle;
    public float maxAngle;

    [HideInInspector] public float currentAngle;
    [Header("Gizmo settings")]
    [SerializeField] private float gizmoCheckRadius;
    [SerializeField] private float gizmoHandGoalRadius;
    [SerializeField] private float gizmoGuideGoalRadius;
    [Space(10)]
    [SerializeField] private Color radialColour;
    [SerializeField] private Color handGoalColour;
    [SerializeField] private Color guideGoalColour;


    private void Awake()
    {
        selfTransform = transform;
        hiddenPosition = new Vector3(selfTransform.position.x, selfTransform.position.y + hiddenYOffset, selfTransform.position.z);
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
