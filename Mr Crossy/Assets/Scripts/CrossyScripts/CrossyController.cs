using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrossyController : MonoBehaviour
{
    Animator animator;
    NavMeshAgent agent;

    public Transform headBone;
    public Transform vision;

    [Space(10)]
    [Header("Movement Variables")]
    [Tooltip("Mr. Crossy's walking speed.")]
    [SerializeField] private float m_WalkSpeed;
    [Tooltip("Mr. Crossy's running speed.")]
    [SerializeField] private float m_RunSpeed;
    private float m_MoveSpeed;
    [Tooltip("Mr. Crossy's acceleration rate.")]
    /*[SerializeField]*/ private float m_Acceleration;
    [Tooltip("Mr. Crossy's turning speed.")]
    [SerializeField] private float m_AngularSpeed;
    [Tooltip("Distance from destination that Mr. Crossy can stop at.")]
    [SerializeField] private float m_StoppingDistance;
    //[SerializeField] private float m_SlowRange;

    private int m_Mask;
    private bool m_ShouldRun = false;
    [Space(10)]
    [Header("Detection Variables")]
    [SerializeField] private float m_DetectionTime;

    public float WalkSpeed { get { return m_WalkSpeed; } set { m_WalkSpeed = value; } }
    public float RunSpeed { get { return m_RunSpeed; } set { m_RunSpeed = value; } }
    public float MoveSpeed { get { return m_MoveSpeed; } set { m_MoveSpeed = value; } }

    public float Acceleration { get { return m_Acceleration; } set { m_Acceleration = value; } }
    public float AngularSpeed { get { return m_AngularSpeed; } set { m_AngularSpeed = value; } }
    public float StoppingDistance { get { return m_StoppingDistance; } set { m_StoppingDistance = value; } }
    public float VisionRotation { get { return vision.eulerAngles.x; }}
    public Vector3 VisionPosition { get { return vision.localPosition; }}

    public int NavMeshMask { get { return m_Mask; } }
    public bool ShouldRun { get { return m_ShouldRun; } set { m_ShouldRun = value; } }

    public float BaseDetectTime { get { return m_DetectionTime; } }
    public float CloseDetectTime { get { return m_DetectionTime*3; } }


    [Space(10)]
    float veloMag;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        m_Mask = agent.areaMask;
    }

    private void Update()
    {
        vision.position = headBone.position;
        vision.rotation = headBone.rotation;
        MoveSpeed = (m_ShouldRun) ? RunSpeed : WalkSpeed;

        veloMag = agent.velocity.magnitude;
        animator.SetBool("Moving", veloMag >= 0.05);
        animator.SetFloat("VelocityMag",veloMag);
    }
}
