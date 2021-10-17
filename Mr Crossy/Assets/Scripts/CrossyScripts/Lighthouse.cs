using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    //Why
    private void Awake()
    {
        selfTransform = transform;
    }
}
