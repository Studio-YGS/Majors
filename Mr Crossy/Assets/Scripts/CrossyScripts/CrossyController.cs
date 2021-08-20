using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrossyController : MonoBehaviour
{
    Animator animator;
    NavMeshAgent agent;

    [SerializeField] private float m_MoveSpeed;
    [SerializeField] private float m_AngularSpeed;
    [SerializeField] private float m_StoppingDistance;

    public float MoveSpeed { get { return m_MoveSpeed; } set { m_MoveSpeed = value; } }

    public float AngularSpeed { get { return m_AngularSpeed; } set { m_AngularSpeed = value; } }
    public float StoppingDistance { get { return m_StoppingDistance; } set { m_StoppingDistance = value; } }


    [SerializeField] float veloMag;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

    }

    private void Update()
    {

        veloMag = agent.velocity.magnitude;
    }
}
