using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using FMODUnity;
using FMOD.Studio;


//If you want stuff to happen when mr crossy attacks you, stuff it in the "CrossyAttack" method way down yonder

//State Stuff -- Despawned = -1, Idle = 0, Patrol = 1, Alert = 2, Pursuit = 3

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class CrossyController : MonoBehaviour
{
    public static BehaviorTree crossyTree;
    Animator animator;
    NavMeshAgent agent;
    EventInstance attackLines;
    EventInstance attackChild;

    [Header("Debug Booleans")]
    [HideInInspector] public bool overrideShouldRun;
    [HideInInspector] public bool run;
    [HideInInspector] public bool accelManipulation;
    [HideInInspector] public bool lookCondition;
    [SerializeField] private bool allowScaryRun;
    public bool speedDebugLog;

    //private Transform headBone;
    [Space(1)]
    [SerializeField] private Transform m_Vision;
    [SerializeField] private Transform m_HindVision;
    [SerializeField] private Transform m_CrossyDespawn;

    [Header("Eye Glow")]
    [SerializeField] private Material crossyGlow;
    [SerializeField] private float m_AlertUnSeenValue;
    [SerializeField] private float m_AlertPerifValue;
    [SerializeField] private float m_AlertFocalValue;
    [SerializeField] private float m_PursitUnseenValue;
    [SerializeField] private float m_PursitFocalValue;

    [SerializeField] private float m_EyeRaiseRate;
    [SerializeField] private float m_EyeLowerRate;
    bool raisingEye;
    bool loweringEye;

    [Header("Movement Variables")]
    [Tooltip("Mr. Crossy's walking speed.")]
    [SerializeField] private float m_WalkSpeed;
    [Tooltip("Mr. Crossy's full running speed.")]
    [SerializeField] private float m_FullRunSpeed;
    [Tooltip("Percentage of full running speed.")]
    [Range(0, 100)] [SerializeField] private int m_SubRunPercent;
    private float m_RunSpeed;
    private float m_MoveSpeed;

    [Tooltip("Mr. Crossy's acceleration rate.")]
    [SerializeField] private float m_BaseAcceleration;
    /*[SerializeField] */
    private float m_StoppingAcceleration = 120f;
    /*[SerializeField] */
    private float m_Acceleration;

    [Tooltip("Mr. Crossy's turning speed.")]
    /*[SerializeField]*/
    private float m_WalkAngularSpeed;
    /*[SerializeField]*/
    private float m_RunAngularSpeed;
    [SerializeField] private float m_AngularSpeed;

    [Tooltip("Distance from destination that Mr. Crossy can stop at.")]
    [SerializeField] private float m_StoppingDistance;
    [SerializeField] private float m_AttackDistanceOffset;
    /*[SerializeField] */
    private float m_CornerThreshold;

    [SerializeField] private float m_PatrolRunDistance;
    [SerializeField] private float m_AlertRunDistance;
    [SerializeField] private float m_ScaryDistance;
    private float m_RunDistance;

    private float m_DistanceToCorner;

    private int m_Mask;
    private bool m_InSight = false;
    private bool m_InPeripheral = false;
    private bool m_ShouldRun = false;
    private bool m_ShouldBeStopped = false;
    private int m_State = -1;

    [Header("Detection Variables")]
    [SerializeField] private float m_FocalViewDist;
    [SerializeField] private float m_PeripheralViewDist;
    [SerializeField] private float m_PursuitDistance;
    [SerializeField] private float m_UpperQuartDistance;
    [SerializeField] private float m_LowerQuartDistance;
    [Space(10)]
    [SerializeField] private float m_DetectionTime;
    [SerializeField] private float m_HighestQuartMulti;
    [SerializeField] private float m_UpperQuartMulti;
    [SerializeField] private float m_LowerQuartMulti;
    [SerializeField] private float m_ReverseMulti;

    [SerializeField] private float m_FocalViewCone;
    [SerializeField] private float m_PeripheralViewCone;
    [Space]
    [Range(0.01f, 0.9f)]
    [SerializeField] private float m_EyeAlertPoint = 0.5f;

    [Header("Particle Effects")]
    [SerializeField] private GameObject m_WarpParticleOne;
    [SerializeField] private GameObject m_WarpParticleTwo;
    [Space]
    [SerializeField] private float m_ParticleWait;


    private float distInterpolator;

    [Space(10)]
    [Header("IK Variables")]
    public LayerMask layerMask;
    public string walkableTag;

    [Range(0, 1)] public float lookAtWeight;
    [SerializeField] private float lookUpRate;
    [SerializeField] private float lookDownRate;
    private float currentWeight = 0f;
    public Transform lookAtTransform;
    public Vector3 lookAtOffset;
    private bool increaseLook;
    private bool decreaseLook;

    [Range(0, 2)] public float IKLeftFootDistance;
    [Range(0, 2)] public float IKRightFootDistance;

    #region Properties
    public Transform Vision { get { return m_Vision; } }
    public Transform HindVision { get { return m_HindVision; } }
    public float WalkSpeed { get { return m_WalkSpeed; } set { m_WalkSpeed = value; } }
    public float FullRunSpeed { get { return m_FullRunSpeed; } set { m_FullRunSpeed = value; } }
    public float SubRunSpeed { get { return m_FullRunSpeed * (m_SubRunPercent / 100f); } }
    public float RunSpeed { get { return (m_InSight) ? FullRunSpeed : SubRunSpeed; } }
    public float MoveSpeed { get { return (m_ShouldRun && RunSpeed > WalkSpeed) ? RunSpeed : WalkSpeed; } }

    public float Acceleration { get { return (m_ShouldBeStopped) ? m_StoppingAcceleration : m_BaseAcceleration; } set { m_Acceleration = value; } }
    public float AngularSpeed { get { return m_AngularSpeed; } set { m_AngularSpeed = value; } }
    public float StoppingDistance { get { return m_StoppingDistance; } set { m_StoppingDistance = value; } }
    public float RunDistance { get { return (m_State == 2) ? m_AlertRunDistance : m_PatrolRunDistance; } }

    public int NavMeshMask { get { return m_Mask; } }
    public bool ShouldRun { get { return m_ShouldRun; } set { m_ShouldRun = value; } }
    public bool InSight { get { return m_InSight; } set { m_InSight = value; } }
    public bool InPeripheral { get { return m_InPeripheral; } set { m_InPeripheral = value; } }
    public bool ShouldBeStopped { get { return m_ShouldBeStopped; } set { m_ShouldBeStopped = value; } }
    public int State { get { return m_State; } set { m_State = value; } }
    public Vector3 CrossyDespawn { get { return m_CrossyDespawn.position; } set { m_CrossyDespawn.position = value; } }

    public float PursuitDistance { get { return m_PursuitDistance; } }
    public float AttackDistanceOffset { get { return m_AttackDistanceOffset; } }

    public float BaseDetectTime { get { return m_DetectionTime; } }
    public float CloseDetectTime { get { return m_DetectionTime * 3; } }

    public float FocalViewCone { get { return m_FocalViewCone; } set { m_FocalViewCone = value; } }
    public float PeripheralViewCone { get { return m_PeripheralViewCone; } set { m_PeripheralViewCone = value; } }
    public float FocalViewDist { get { return m_FocalViewDist; } set { m_FocalViewDist = value; } }
    public float UpperQuartDistance { get { return m_UpperQuartDistance; } set { m_UpperQuartDistance = value; } }
    public float LowerQuartDistance { get { return m_LowerQuartDistance; } set { m_LowerQuartDistance = value; } }
    public float PeripheralViewDist { get { return m_PeripheralViewDist; } set { m_PeripheralViewDist = value; } }
    public float HighestQuartMulti { get { return m_HighestQuartMulti; } set { m_HighestQuartMulti = value; } }
    public float UpperQuartMulti { get { return m_UpperQuartMulti; } set { m_UpperQuartMulti = value; } }
    public float LowerQuartMulti { get { return m_LowerQuartMulti; } set { m_LowerQuartMulti = value; } }
    public float ReverseMulti { get { return m_ReverseMulti; } set { m_ReverseMulti = value; } }

    public GameObject WarpParticleOne { get { return m_WarpParticleOne; } }
    public GameObject WarpParticleTwo { get { return m_WarpParticleTwo; } }

    public ParticleSystem PartiboiOne { get { return m_WarpParticleOne.GetComponent<ParticleSystem>(); } }
    public ParticleSystem PartiboiTwo { get { return m_WarpParticleTwo.GetComponent<ParticleSystem>(); } }
    public StudioEventEmitter SoundiboiOne { get { return m_WarpParticleOne.GetComponent<StudioEventEmitter>(); } }
    public StudioEventEmitter SoundiboiTwo { get { return m_WarpParticleTwo.GetComponent<StudioEventEmitter>(); } }
    public float ParticleWaitTime { get { return m_ParticleWait; } }

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
        crossyGlow.EnableKeyword("_EMISSION");
        crossyTree = GetComponent<BehaviorTree>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        m_Mask = agent.areaMask;
        //headBone = animator.GetBoneTransform(HumanBodyBones.Head);
        //m_RunDistance = (m_State == 2) ? m_AlertRunDistance : m_PatrolRunDistance;
    }

    private void Update()
    {
        if(speedDebugLog)
        {
            Debug.Log("SPEEDSPEED: Base Walk Speed: " + WalkSpeed);
            Debug.Log("SPEEDSPEED: Sub Run Speed: " + SubRunSpeed + ", at Percentage: " + m_SubRunPercent + "%");
            Debug.Log("SPEEDSPEED: Full Run Speed: " + FullRunSpeed);
            Debug.Log("SPEEDSPEED: Set Run Speed: " + RunSpeed);
            Debug.Log("SPEEDSPEED: Current Move Speed: " + MoveSpeed);
        }

        //m_RunDistance = (m_State == 2) ? m_AlertRunDistance : m_PatrolRunDistance;
        m_DistanceToCorner = Vector3.Distance(transform.position, agent.steeringTarget);

        CrossyEye();

        Running();
        if(allowScaryRun) ScaryRunCondition();
        StoppyStop();

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Spinspin")) animator.applyRootMotion = true;
        else animator.applyRootMotion = false;

        //RunSpeed = (m_InSight) ? FullRunSpeed : SubRunSpeed;
        //MoveSpeed = (m_ShouldRun) ? RunSpeed : WalkSpeed;

        //Acceleration = (m_ShouldBeStopped) ? m_StoppingAcceleration : m_BaseAcceleration;

        //agent.acceleration = Acceleration;

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
            if (m_State < 1) { m_ShouldRun = false; }
            else if (m_State == 1 || m_State == 2)
            {
                if (agent.remainingDistance > RunDistance) m_ShouldRun = true;
                else m_ShouldRun = false;
            }
            else if (m_State == 3) { m_ShouldRun = true; }
        }
        else { m_ShouldRun = run; }
    }

    public void ScaryRunCondition()
    {
        float distance = agent.remainingDistance;

        if(m_State == 3)
        {
            if(distance <= m_ScaryDistance) animator.SetBool("ScaryVariant", true);
            else animator.SetBool("ScaryVariant", false);
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

    public void StoppyStop()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Spinspin") || animator.GetCurrentAnimatorStateInfo(0).IsName("Scream") || animator.GetCurrentAnimatorStateInfo(0).IsName("Agony") || animator.GetCurrentAnimatorStateInfo(0).IsName("LookAround"))
        {
            if(animator.GetCurrentAnimatorStateInfo(0).IsName("Movement") || animator.GetCurrentAnimatorStateInfo(0).IsName("AngyMove"))
            {
                m_ShouldBeStopped = false;
            }
            else m_ShouldBeStopped = true;
        }
        else m_ShouldBeStopped = false;
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
                    StartCoroutine(IncreaseLook());
                }
                else
                {
                    StartCoroutine(DecreaseLook());
                }

                animator.SetLookAtPosition(lookAtTransform.position + lookAtOffset);
                animator.SetLookAtWeight(currentWeight);
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

    IEnumerator IncreaseLook()
    {
        increaseLook = true;
        StopCoroutine(DecreaseLook());
        while (currentWeight < lookAtWeight)
        {
            currentWeight += lookUpRate * Time.deltaTime;

            yield return null;
        }
        currentWeight = lookAtWeight;

        increaseLook = false;
    }
    IEnumerator DecreaseLook()
    {
        decreaseLook = true;
        StopCoroutine(IncreaseLook());
        while (currentWeight > 0f)
        {
            currentWeight -= lookDownRate * Time.deltaTime;

            yield return null;
        }
        currentWeight = 0f;

        decreaseLook = false;
    }

    public void CrossyEye()
    {
        if(crossyGlow)
        {
            Color colour = crossyGlow.color;
            Color emColour = crossyGlow.GetColor("_EmissionColor");

            if (m_State < 1)
            {
                if (colour.a > 0f)
                {
                    if (raisingEye) { StopCoroutine(RaiseEyeGlow(colour, 0f)); raisingEye = false; }
                    if (!loweringEye) StartCoroutine(LowerEyeGlow(colour, 0f));
                }
            }
            else if (m_State >= 1)
            {
                if(!m_InPeripheral && !m_InSight)
                {
                    emColour.a = 0.5f;
                    if(colour.a < m_AlertUnSeenValue)
                    {
                        if (loweringEye) { StopCoroutine(LowerEyeGlow(colour, m_AlertUnSeenValue)); loweringEye = false; }
                        if (!raisingEye) StartCoroutine(RaiseEyeGlow(colour, m_AlertUnSeenValue));
                    }
                    else if (colour.a > m_AlertUnSeenValue)
                    {
                        if (raisingEye) { StopCoroutine(RaiseEyeGlow(colour, m_AlertUnSeenValue)); raisingEye = false; }
                        if (!loweringEye) StartCoroutine(LowerEyeGlow(colour, m_AlertUnSeenValue));
                    }
                    crossyGlow.SetColor("_EmissionColor", emColour);
                }
                else if (m_InPeripheral && !m_InSight)
                {
                    emColour.a = 0.7f;
                    if (colour.a < m_AlertPerifValue)
                    {
                        if (loweringEye) { StopCoroutine(LowerEyeGlow(colour, m_AlertPerifValue)); loweringEye = false; }
                        if (!raisingEye) StartCoroutine(RaiseEyeGlow(colour, m_AlertPerifValue));
                    }
                    else if (colour.a > m_AlertPerifValue)
                    {
                        if (raisingEye) { StopCoroutine(RaiseEyeGlow(colour, m_AlertPerifValue)); raisingEye = false; }
                        if (!loweringEye) StartCoroutine(LowerEyeGlow(colour, m_AlertPerifValue));
                    }
                    crossyGlow.SetColor("_EmissionColor", emColour);
                }
                else if (m_InSight)
                {
                    emColour.a = 1f;
                    if (colour.a < m_AlertFocalValue)
                    {
                        if (loweringEye) { StopCoroutine(LowerEyeGlow(colour, m_AlertFocalValue)); loweringEye = false; }
                        if (!raisingEye) StartCoroutine(RaiseEyeGlow(colour, m_AlertFocalValue));
                    }
                    else if (colour.a > m_AlertFocalValue)
                    {
                        if (raisingEye) { StopCoroutine(RaiseEyeGlow(colour, m_AlertFocalValue)); raisingEye = false; }
                        if (!loweringEye) StartCoroutine(LowerEyeGlow(colour, m_AlertFocalValue));
                    }
                    crossyGlow.SetColor("_EmissionColor", emColour);
                }
            }
        }
    }

    public IEnumerator RaiseEyeGlow(Color colour, float raiseTo)
    {
        raisingEye = true;
        while(colour.a < raiseTo)
        {
            colour.a += Time.deltaTime * m_EyeRaiseRate;
            crossyGlow.color = colour;
            yield return null;
        }
        colour.a = raiseTo;
        crossyGlow.color = colour;
        raisingEye = false;
    }

    public IEnumerator LowerEyeGlow(Color colour, float lowerTo)
    {
        loweringEye = true;
        while (colour.a > lowerTo)
        {
            colour.a -= Time.deltaTime * m_EyeLowerRate;
            crossyGlow.color = colour;
            yield return null;
        }
        colour.a = lowerTo;
        crossyGlow.color = colour;
        loweringEye = false;
    }

    public void OnEnable()
    {
        TreeMalarkey.RegisterEventOnTree(crossyTree, "Darken", DarkenEvent);
        TreeMalarkey.RegisterEventOnTree(crossyTree, "PlayAttack", AttackSound);
        TreeMalarkey.RegisterEventOnTree(crossyTree, "PlayDamage", AttackHitSound);
        TreeMalarkey.RegisterEventOnTree(crossyTree, "WooshOne", WooshSingle);
        TreeMalarkey.RegisterEventOnTree(crossyTree, "WooshTwo", WooshBothle);
    }

    public void RegisterEvents()
    {
        TreeMalarkey.RegisterEventOnTree(OverseerController.ObserverTree, "WooshOne", WooshSingle);
        TreeMalarkey.RegisterEventOnTree(OverseerController.ObserverTree, "WooshTwo", WooshBothle);
    }

    public void DarkenEvent()
    {
        Debug.Log("EVENT received");
        FindObjectOfType<MrCrossyDistortion>().DarkenScreen(1.5f);
        Debug.Log("EVENT methid called");
    }

    public void AttackSound()
    {
        attackLines = RuntimeManager.CreateInstance("event:/MR_C_Attack/Mr_C_Attack");
        attackLines.start();
    }

    public void AttackHitSound()
    {
        attackChild = RuntimeManager.CreateInstance("event:/Character/Hit and damage/Child Hit and Damage");
        attackChild.start();
    }

    public void WooshSingle()
    {
        PartiboiOne.Play();
        SoundiboiOne.Play();
    }

    public void WooshBothle()
    {
        PartiboiOne.Play();
        SoundiboiOne.Play();
        PartiboiTwo.Play();
        SoundiboiTwo.Play();
    }

    public void OnDisable()
    {
        TreeMalarkey.UnregisterEventOnTree(crossyTree, "Darken", DarkenEvent);
        TreeMalarkey.UnregisterEventOnTree(crossyTree, "PlayAttack", AttackSound);
        TreeMalarkey.UnregisterEventOnTree(crossyTree, "PlayDamage", AttackHitSound);
        TreeMalarkey.UnregisterEventOnTree(crossyTree, "WooshOne", WooshSingle);
        TreeMalarkey.UnregisterEventOnTree(crossyTree, "WooshTwo", WooshBothle);
        TreeMalarkey.UnregisterEventOnTree(OverseerController.ObserverTree, "WooshOne", WooshSingle);
        TreeMalarkey.UnregisterEventOnTree(OverseerController.ObserverTree, "WooshTwo", WooshBothle);
    }
}
