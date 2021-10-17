using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class CrossyController : MonoBehaviour
{
    Animator animator;
    NavMeshAgent agent;

    public bool overrideShouldRun;
    public bool run;
    public bool conditionIsTrue;

    public Transform headBone;
    public Transform vision;
    [SerializeField] private Transform m_CrossyDespawn;

    [SerializeField] private float m_PatrolRunDistance;
    [SerializeField] private float m_AlertRunDistance;
    private float m_RunDistance;

    [Space(10)]
    [Header("Movement Variables")]
    [Tooltip("Mr. Crossy's walking speed.")]
    [SerializeField] private float m_WalkSpeed;
    [Tooltip("Mr. Crossy's running speed.")]
    [SerializeField] private float m_RunSpeed;
    private float m_MoveSpeed;
    [Tooltip("Mr. Crossy's acceleration rate.")]
    [SerializeField] private float m_WalkAcceleration;
    [SerializeField] private float m_RunAcceleration;
    [SerializeField] private float m_Acceleration;
    [Tooltip("Mr. Crossy's turning speed.")]
    [SerializeField] private float m_WalkAngularSpeed;
    [SerializeField] private float m_RunAngularSpeed;
    [SerializeField] private float m_AngularSpeed;
    [Tooltip("Distance from destination that Mr. Crossy can stop at.")]
    [SerializeField] private float m_StoppingDistance;

    private int m_Mask;
    private bool m_ShouldRun = false;
    private int m_State = -1;
    [Space(10)]
    [Header("Detection Variables")]
    [SerializeField] private float m_DetectionTime;

    [Space(10)]
    [Header("IK Variables")]
    public LayerMask layerMask;
    public string walkableTag;

    [Range(0,1)]public float lookAtWeight;
    public Transform lookAtTransform;
    public Vector3 lookAtOffset;
    public float IKLeftFootDistance;
    public float IKRightFootDistance;

    #region Properties
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
    public int State { get { return m_State; } set { m_State = value; } }
    public Vector3 CrossyDespawn { get { return m_CrossyDespawn.position; } set { m_CrossyDespawn.position = value; } }

    public float BaseDetectTime { get { return m_DetectionTime; } }
    public float CloseDetectTime { get { return m_DetectionTime*3; } }
    #endregion

    [Space(10)]
    [SerializeField] float veloMag;
    [SerializeField] float veloDesire;
    [SerializeField] float interpolator;

    [Space(10)]
    [Header("Debuggles")]
    [SerializeField] private Vector3 showNorma;
    [SerializeField] private Quaternion showLeftRotation;
    [SerializeField] private Quaternion showRightRotation;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        m_Mask = agent.areaMask;
    }

    private void Update()
    {
        vision.position = headBone.position;
        vision.rotation.SetLookRotation(headBone.up, -headBone.right);
        veloMag = agent.velocity.magnitude;
        veloDesire = agent.desiredVelocity.magnitude;

        if (overrideShouldRun == false)
        {
            if (m_State < 1) { m_ShouldRun = false; }
            else if (m_State == 1 || m_State == 2)
            {
                m_RunDistance = (m_State == 1) ? m_AlertRunDistance : m_PatrolRunDistance;

                if (agent.remainingDistance > m_RunDistance) m_ShouldRun = true;
                else m_ShouldRun = false;
            }
            else if (m_State > 2) { m_ShouldRun = true; }

        }
        else { m_ShouldRun = run; }


        //NavAgent fiddling
        MoveSpeed = (m_ShouldRun) ? RunSpeed : WalkSpeed;
        /*
        interpolator = Mathf.InverseLerp(0, RunSpeed, veloDesire);

        m_AngularSpeed = Mathf.Lerp(m_WalkAngularSpeed, m_RunAngularSpeed, interpolator);
        m_Acceleration = Mathf.Lerp(m_WalkAcceleration, m_RunAcceleration, interpolator);
        */
        agent.acceleration = Acceleration;

        //Animator Actions
        
        animator.SetBool("Moving", veloMag >= 0.05);
        animator.SetFloat("VelocityMag",veloMag);
    }

    private void LateUpdate()
    {
        
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (animator)
        {
            //Noggin Malarkey
            if (lookAtTransform)
            {
                if (conditionIsTrue)
                {
                    animator.SetLookAtPosition(lookAtTransform.position + lookAtOffset);
                    animator.SetLookAtWeight(lookAtWeight);
                }
                else animator.SetLookAtWeight(0f);
            }

            //Foot Stuff
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1f);
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1f);

            showLeftRotation = animator.GetIKRotation(AvatarIKGoal.LeftFoot);
            showRightRotation = animator.GetIKRotation(AvatarIKGoal.RightFoot);

            RaycastHit hit;
            Ray ray;
            //LeftFoot
            ray = new Ray(animator.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);
            if(Physics.Raycast(ray, out hit, IKLeftFootDistance + 1f, layerMask))
            {
                if(hit.transform.CompareTag(walkableTag))
                {
                    Vector3 footPos = hit.point;

                    footPos.y += IKLeftFootDistance;
                    animator.SetIKPosition(AvatarIKGoal.LeftFoot, footPos);

                    animator.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(transform.forward, hit.normal));
                    showNorma = hit.normal;
                }
            }

            //LeftFoot
            ray = new Ray(animator.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up, Vector3.down);
            if (Physics.Raycast(ray, out hit, IKRightFootDistance + 1f, layerMask))
            {
                if (hit.transform.CompareTag(walkableTag))
                {
                    Vector3 footPos = hit.point;

                    footPos.y += IKRightFootDistance;
                    animator.SetIKPosition(AvatarIKGoal.RightFoot, footPos);
                    animator.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.LookRotation(transform.forward, hit.normal));
                }
            }
        }
    }
}
