using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrossyController : MonoBehaviour
{
    Animator animator;
    NavMeshAgent agent;

    public bool shouldRun = false;

    [Space(10)]
    [SerializeField] private float m_WalkSpeed;
    [SerializeField] private float m_RunSpeed;
    [Space(10)]
    private float accelAmt;
    private float deccelAmt;

    private float m_MoveSpeed;
    [Space(10)]
    private float m_Acceleration;
    
    [SerializeField] private float m_AngularSpeed;
    [SerializeField] private float m_StoppingDistance;
    [SerializeField] private float slowRange;

    public float WalkSpeed { get { return m_WalkSpeed; } set { m_WalkSpeed = value; } }
    public float RunSpeed { get { return m_RunSpeed; } set { m_RunSpeed = value; } }
    public float MoveSpeed { get { return m_MoveSpeed; } set { m_MoveSpeed = value; } }

    public float Acceleration { get { return m_Acceleration; } set { m_Acceleration = value; } }
    public float AngularSpeed { get { return m_AngularSpeed; } set { m_AngularSpeed = value; } }
    public float StoppingDistance { get { return m_StoppingDistance; } set { m_StoppingDistance = value; } }

    [Space(10)]
    [SerializeField] float veloMag;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

    }

    private void Update()
    {
        MoveSpeed = (shouldRun) ? RunSpeed : WalkSpeed;

        veloMag = agent.velocity.magnitude;
        animator.SetBool("Moving", veloMag >= 0.05);
        animator.SetFloat("VelocityMag",veloMag);
    }
}
