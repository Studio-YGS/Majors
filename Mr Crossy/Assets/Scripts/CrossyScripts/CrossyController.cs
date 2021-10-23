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

    [Header("Debug Booleans")]
    public bool overrideShouldRun;
    public bool run;
    public bool accelManipulation;
    public bool lookCondition;

    private Transform headBone;
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
    [SerializeField] private float m_BaseAcceleration;
    [SerializeField] private float m_CornerAcceleration;
    [SerializeField] private float m_Acceleration;
    [Tooltip("Mr. Crossy's turning speed.")]
    /*[SerializeField]*/ private float m_WalkAngularSpeed;
    /*[SerializeField]*/ private float m_RunAngularSpeed;
    [SerializeField] private float m_AngularSpeed;
    [Tooltip("Distance from destination that Mr. Crossy can stop at.")]
    [SerializeField] private float m_StoppingDistance;
    [SerializeField] private float m_CornerThreshold;

    private float m_DistanceToCorner;

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

    [Range(0,2)] public float IKLeftFootDistance;
    [Range(0,2)] public float IKRightFootDistance;

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
    [SerializeField] float mSpeed;
    [SerializeField] float tSpeed;
    //[SerializeField] float veloDesire;
    [SerializeField] float interpolator;

    /*[Space(10)]
    [Header("Debuggles")]
    [SerializeField] private Vector3 showNorma;
    [SerializeField] private Quaternion showLeftRotation;
    [SerializeField] private Quaternion showRightRotation;
    */

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        m_Mask = agent.areaMask;
        headBone = animator.GetBoneTransform(HumanBodyBones.Head);
    }

    private void Update()
    {
        //veloMag = agent.velocity.magnitude;
        m_DistanceToCorner = Vector3.Distance(transform.position, agent.steeringTarget);

        if (overrideShouldRun == false) // Sets 'm_ShouldRun' based on state and distance from target.
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

        //NavAgent Fiddling
        MoveSpeed = (m_ShouldRun) ? RunSpeed : WalkSpeed;
        
        if(accelManipulation)
        {
            if (m_DistanceToCorner <= m_CornerThreshold)
            {
                interpolator = Mathf.InverseLerp(m_CornerThreshold, 0f, m_DistanceToCorner);

            }
            else if (interpolator < 1f)
            {
                interpolator = 1f;
            }
        }

        Acceleration = (accelManipulation) ? Mathf.Lerp(m_BaseAcceleration, m_CornerAcceleration, interpolator) : m_BaseAcceleration;

        agent.acceleration = Acceleration;

        GetMotionHashValues();

        //Animator Actions
        animator.SetBool("Moving", mSpeed >= 0.05);
        animator.SetFloat("Speed", mSpeed);
        animator.SetFloat("Turn", tSpeed);
    }

    public void GetMotionHashValues()
    {
        Vector3 velocity = agent.transform.InverseTransformDirection(agent.velocity);

        mSpeed = velocity.z;
        tSpeed = velocity.x;
    }


    private void OnAnimatorIK(int layerIndex)
    {
        if (animator)
        {
            //Noggin Malarkey
            if (lookAtTransform)
            {
                if (lookCondition)
                {
                    animator.SetLookAtPosition(lookAtTransform.position + lookAtOffset);
                    animator.SetLookAtWeight(lookAtWeight);
                }
                else animator.SetLookAtWeight(0f);
            }
            
            //Foot Stuff
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, animator.GetFloat("IKLeftFootWeight"));
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, animator.GetFloat("IKLeftFootWeight"));
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, animator.GetFloat("IKRightFootWeight"));
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, animator.GetFloat("IKRightFootWeight"));

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
                    animator.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(-animator.GetBoneTransform(HumanBodyBones.LeftFoot).up, hit.normal));
                    //showNorma = hit.normal;
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
                    animator.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.LookRotation(animator.GetBoneTransform(HumanBodyBones.RightFoot).up, hit.normal));
                }
            }
        }
    }
}
