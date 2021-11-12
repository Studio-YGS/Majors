using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;


//If you want stuff to happen when mr crossy attacks you, stuff it in the "CrossyAttack" method way down yonder

//State Stuff -- Despawned = -1, Idle = 0, Patrol = 1, Alert = 2, Pursuit = 3

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class CrossyController : MonoBehaviour
{
    public static BehaviorTree crossyTree;
    Animator animator;
    NavMeshAgent agent;

    [Header("Debug Booleans")]
    [HideInInspector] public bool overrideShouldRun;
    [HideInInspector] public bool run;
    [HideInInspector] public bool accelManipulation;
    [HideInInspector] public bool lookCondition;

    private Transform headBone;
    [Space(1)]
    public Transform vision;
    [SerializeField] private Transform m_CrossyDespawn;

    [Header("Movement Variables")]
    [Tooltip("Mr. Crossy's walking speed.")]
    [SerializeField] private float m_WalkSpeed;
    [Tooltip("Mr. Crossy's running speed.")]
    [SerializeField] private float m_RunSpeed;
    private float m_MoveSpeed;

    [Tooltip("Mr. Crossy's acceleration rate.")]
    [SerializeField] private float m_BaseAcceleration;
    /*[SerializeField] */private float m_CornerAcceleration;
    /*[SerializeField] */private float m_Acceleration;

    [Tooltip("Mr. Crossy's turning speed.")]
    /*[SerializeField]*/ private float m_WalkAngularSpeed;
    /*[SerializeField]*/ private float m_RunAngularSpeed;
    [SerializeField] private float m_AngularSpeed;

    [Tooltip("Distance from destination that Mr. Crossy can stop at.")]
    [SerializeField] private float m_StoppingDistance;
    [SerializeField] private float m_AttackDistanceOffset;
    /*[SerializeField] */private float m_CornerThreshold;

    [SerializeField] private float m_PatrolRunDistance;
    [SerializeField] private float m_AlertRunDistance;
    [SerializeField] private float m_ScaryDistance;
    private float m_RunDistance;

    private float m_DistanceToCorner;

    private int m_Mask;
    private bool m_InSight = false;
    private bool m_InPeripheral = false;
    private bool m_ShouldRun = false;
    private int m_State = -1;
    [Header("Detection Variables")]
    [SerializeField] private float m_PursuitDistance;
    [SerializeField] private float m_DetectionTime;

    [SerializeField] private float m_FocalViewCone;
    [SerializeField] private float m_PeripheralViewCone;

    [SerializeField] private float m_FocalViewDist;
    [SerializeField] private float m_PeripheralViewDist;

    private float distInterpolator;

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
    public bool InSight { get { return m_InSight; } set { m_InSight = value; } }
    public bool InPeripheral { get { return m_InPeripheral; } set { m_InPeripheral = value; } }
    public int State { get { return m_State; } set { m_State = value; } }
    public Vector3 CrossyDespawn { get { return m_CrossyDespawn.position; } set { m_CrossyDespawn.position = value; } }

    public float PursuitDistance { get { return m_PursuitDistance; } }
    public float AttackDistanceOffset { get { return m_AttackDistanceOffset; } }

    public float BaseDetectTime { get { return m_DetectionTime; } }
    public float CloseDetectTime { get { return m_DetectionTime*3; } }

    public float FocalViewCone { get { return m_FocalViewCone; } set { m_FocalViewCone = value; } }
    public float PeripheralViewCone { get { return m_PeripheralViewCone; } set { m_PeripheralViewCone = value; } }
    public float FocalViewDist { get { return m_FocalViewDist; } set { m_FocalViewDist = value; } }
    public float PeripheralViewDist { get { return m_PeripheralViewDist; } set { m_PeripheralViewDist = value; } }
    #endregion

    [Space(10)]
    /*[SerializeField] */float mSpeed;
    /*[SerializeField] */float tSpeed;
    //[SerializeField] float veloDesire;
    float interpolator;

    /*[Space(10)]
    [Header("Debuggles")]
    [SerializeField] private Vector3 showNorma;
    [SerializeField] private Quaternion showLeftRotation;
    [SerializeField] private Quaternion showRightRotation;
    */

    private void Awake()
    {
        crossyTree = GetComponent<BehaviorTree>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        m_Mask = agent.areaMask;
        headBone = animator.GetBoneTransform(HumanBodyBones.Head);
    }

    private void Update()
    {
        //veloMag = agent.velocity.magnitude;
        m_DistanceToCorner = Vector3.Distance(transform.position, agent.steeringTarget);

        Running();
        ScaryRunCondition();

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

        if (m_InSight || m_InPeripheral) lookCondition = true;
        else lookCondition = false;

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

    public void Running()
    {
        if (overrideShouldRun == false) // Sets 'm_ShouldRun' based on state and distance from target.
        {
            if (m_State <= 1) { m_ShouldRun = false; }
            else if (m_State == 1 || m_State == 2)
            {
                m_RunDistance = (m_State == 2) ? m_AlertRunDistance : m_PatrolRunDistance;

                if (agent.remainingDistance > m_RunDistance) m_ShouldRun = true;
                else m_ShouldRun = false;
            }
            else if (m_State == 3) { m_ShouldRun = true; }
        }
        else { m_ShouldRun = run; }
    }

    public void ScaryRunCondition()
    {
        float distance = agent.remainingDistance;

        if(m_State == 3 && distance <= m_ScaryDistance)
        {
            animator.SetBool("ScaryVariant", true);
        }
        else if (m_State == 4)
        {
            animator.SetBool("ScaryVariant", true);
        }
        else
        {
            animator.SetBool("ScaryVariant", false);
        }
    }

    public void ForceDespawn()
    {
        crossyTree.SendEvent("Despawn");
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

    public void OnEnable()
    {
        TreeMalarkey.RegisterEventOnTree(crossyTree, "Schlepp", Flibbity);
    }

    public void Flibbity()
    {
        FindObjectOfType<MrCrossyDistortion>().DarkenScreen(1.5f);
    }

    public void OnDisable()
    {
        TreeMalarkey.UnregisterEventOnTree(crossyTree, "Schlepp", Flibbity);
    }
}
