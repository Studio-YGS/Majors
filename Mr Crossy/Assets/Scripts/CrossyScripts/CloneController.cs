using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gemstone;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tactical;
using FMODUnity;
using FMOD.Studio;


[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class CloneController : MonoBehaviour, IAttackAgent
{
    #region ExternalVariables
    public BehaviorTree cloneTree;

    OverseerController overseer;

    Animator animator;
    NavMeshAgent agent;

    EventInstance attackLines;
    EventInstance attackChild;
    #endregion

    #region MainVariables

    #region Main/VisionVariables
    [Header("Vision Variables")]
    [SerializeField] private Transform m_Vision;
    [Space(5)]
    [SerializeField] private float m_FocalViewDist;
    [SerializeField] private float m_PeripheralViewDist;

    [SerializeField] private float m_FocalViewCone;
    [SerializeField] private float m_PeripheralViewCone;

    private bool m_InOwnSight = false;
    private bool m_InPeripheral = false;
    #endregion

    #region Main/MovementVariables
    [Header("Movement Variables")]
    [SerializeField] private float m_WalkSpeed;
    [SerializeField] private float m_FullRunSpeed;
    [Range(0, 100)] [SerializeField] private int m_SubRunPercent;

    [SerializeField] private float m_BaseAcceleration;
    [SerializeField] private float m_StoppingAcceleration = 120f;

    [SerializeField] private float m_AngularSpeed;

    [SerializeField] private float m_StoppingDistance;

    float mSpeed;
    float tSpeed;
    #endregion

    #region Main/AttackVariables

    [SerializeField] private float m_AttackDistance;
    [SerializeField] private float m_AttackDelay;
    [SerializeField] private float m_AttackAngle;

    private float m_SinceLastAttack;
    #endregion

    #region Main/EyeGlowVariables
    [Header("Eye Glow Variables")]
    [SerializeField] private Material cloneGlow;

    [SerializeField] private float m_AlertUnSeenValue;
    [SerializeField] private float m_AlertPerifValue;
    [SerializeField] private float m_AlertFocalValue;

    [SerializeField] private float m_EyeRaiseRate;
    [SerializeField] private float m_EyeLowerRate;

    bool raisingEye;
    bool loweringEye;
    #endregion

    #region Main/IKVariables
    [Header("IK Variables")]
    public LayerMask layerMask;
    public string walkableTag;

    [Range(0, 1)] public float lookAtWeight;
    [SerializeField] private float lookUpRate;
    [SerializeField] private float lookDownRate;
    private float currentWeight = 0f;

    public Transform lookAtTransform;
    public Vector3 lookAtOffset;

    [Range(0, 2)] public float IKLeftFootDistance;
    [Range(0, 2)] public float IKRightFootDistance;
    #endregion

    #region Main/MiscVariables
    //Misc Variables
    private int m_State = -1;
    private int m_Mask;

    //Particles
    public GameObject warpParticle;
    #endregion

    #endregion

    #region Properties
    //Vision Properties
    public Transform Vision { get { return m_Vision; } }
    public float FocalViewDist { get { return m_FocalViewDist; } set { m_FocalViewDist = value; } }
    public float PeripheralViewDist { get { return m_PeripheralViewDist; } set { m_PeripheralViewDist = value; } }
    public float FocalViewCone { get { return m_FocalViewCone; } set { m_FocalViewCone = value; } }
    public float PeripheralViewCone { get { return m_PeripheralViewCone; } set { m_PeripheralViewCone = value; } }
    public bool InOwnSight { get { return m_InOwnSight; } set { m_InOwnSight = value; } }
    public bool InPeripheral { get { return m_InPeripheral; } set { m_InPeripheral = value; } }
    public bool LookCondition { get { return InOwnSight || InPeripheral; } }

    //Movement Properties
    public float WalkSpeed { get { return m_WalkSpeed; } }
    public float FullRunSpeed { get { return m_FullRunSpeed; } }
    public float SubRunSpeed { get { return m_FullRunSpeed * (m_SubRunPercent / 100f); } }
    public float RunSpeed { get { return (m_InOwnSight) ? FullRunSpeed : SubRunSpeed; } }
    public float MoveSpeed { get { return (LookCondition && RunSpeed > WalkSpeed) ? RunSpeed : WalkSpeed; } }

    public float Acceleration { get { return (ShouldBeStopped) ? m_StoppingAcceleration : m_BaseAcceleration; } }
    public float AngularSpeed { get { return m_AngularSpeed; } set { m_AngularSpeed = value; } }
    public float StoppingDistance { get { return m_StoppingDistance; } set { m_StoppingDistance = value; } }

    //Misc Properties
    public int NavMeshMask { get { return m_Mask; } }
    public bool ShouldBeStopped { get { return animator.GetCurrentAnimatorStateInfo(0).IsName("Spinspin") || animator.GetCurrentAnimatorStateInfo(0).IsName("Agony") || animator.GetCurrentAnimatorStateInfo(0).IsName("LookAround"); } }
    public int State { get { return m_State; } set { m_State = value; } }

    #endregion

    #region UnityMethods
    private void Awake()
    {
        overseer = FindObjectOfType<OverseerController>();

        cloneTree = GetComponent<BehaviorTree>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        
        m_Mask = agent.areaMask;

        cloneGlow.EnableKeyword("_EMISSION");

        overseer.crossyClones.Add(this);

        lookAtTransform = overseer.Player.GetComponent<Player_Controller>().cam;
    }

    private void Update()
    {
        agent.acceleration = Acceleration;

        CloneEye();

        //Animator
        GetMotionHashValues();

        animator.SetInteger("State", m_State);

        animator.SetFloat("Speed", mSpeed);
        animator.SetFloat("Turn", tSpeed);
    }

    #endregion

    #region MainMethods
    public void GetMotionHashValues()
    {
        Vector3 velocity = agent.transform.InverseTransformDirection(agent.velocity);

        mSpeed = velocity.z;
        tSpeed = velocity.x;
    }

    public void PrepDespawn()
    {
        Instantiate(warpParticle, transform.position, transform.rotation);
        overseer.crossyClones.Remove(this);
    }

    void AttackNoise()
    {
        attackLines = RuntimeManager.CreateInstance("event:/MR_C_Attack/Mr_C_Attack");

        attackLines.start();

        attackLines.release();
    }

    #endregion

    #region AttackInterfaceMethods
    public float AttackDistance()
    {
        return m_AttackDistance;
    }
    public bool CanAttack()
    {
        return (m_SinceLastAttack + m_AttackDelay < Time.time && animator.GetCurrentAnimatorStateInfo(1).IsName("NotAttacking"));
    }
    public float AttackAngle()
    {
        return m_AttackAngle;
    }
    public void Attack(Vector3 targetPosition)
    {
        animator.SetInteger("AttackVar", Random.Range(0, 2));
        animator.SetTrigger("Attack");
        AttackNoise();
        m_SinceLastAttack = Time.time;
    }

    #endregion

    #region EyeGlowMethods
    public IEnumerator RaiseEyeGlow(Color colour, float raiseTo)
    {
        raisingEye = true;
        while (colour.a < raiseTo)
        {
            colour.a += Time.deltaTime * m_EyeRaiseRate;
            cloneGlow.color = colour;
            yield return null;
        }
        colour.a = raiseTo;
        cloneGlow.color = colour;
        raisingEye = false;
    }
    public IEnumerator LowerEyeGlow(Color colour, float lowerTo)
    {
        loweringEye = true;
        while (colour.a > lowerTo)
        {
            colour.a -= Time.deltaTime * m_EyeLowerRate;
            cloneGlow.color = colour;
            yield return null;
        }
        colour.a = lowerTo;
        cloneGlow.color = colour;
        loweringEye = false;
    }

    public void CloneEye()
    {
        if (cloneGlow)
        {
            Color colour = cloneGlow.color;
            Color emColour = cloneGlow.GetColor("_EmissionColor");

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
                if (!m_InPeripheral && !m_InOwnSight)
                {
                    emColour.a = 0.5f;
                    if (colour.a < m_AlertUnSeenValue)
                    {
                        if (loweringEye) { StopCoroutine(LowerEyeGlow(colour, m_AlertUnSeenValue)); loweringEye = false; }
                        if (!raisingEye) StartCoroutine(RaiseEyeGlow(colour, m_AlertUnSeenValue));
                    }
                    else if (colour.a > m_AlertUnSeenValue)
                    {
                        if (raisingEye) { StopCoroutine(RaiseEyeGlow(colour, m_AlertUnSeenValue)); raisingEye = false; }
                        if (!loweringEye) StartCoroutine(LowerEyeGlow(colour, m_AlertUnSeenValue));
                    }
                    cloneGlow.SetColor("_EmissionColor", emColour);
                }
                else if (m_InPeripheral && !m_InOwnSight)
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
                    cloneGlow.SetColor("_EmissionColor", emColour);
                }
                else if (m_InOwnSight)
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
                    cloneGlow.SetColor("_EmissionColor", emColour);
                }
            }
        }
    }

    #endregion

    #region AnimatorMethods
    IEnumerator IncreaseLook()
    {
        StopCoroutine(DecreaseLook());
        while (currentWeight < lookAtWeight)
        {
            currentWeight += lookUpRate * Time.deltaTime;

            yield return null;
        }
        currentWeight = lookAtWeight;
    }
    IEnumerator DecreaseLook()
    {
        StopCoroutine(IncreaseLook());
        while (currentWeight > 0f)
        {
            currentWeight -= lookDownRate * Time.deltaTime;

            yield return null;
        }
        currentWeight = 0f;
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (animator)
        {
            //Noggin Malarkey
            if (lookAtTransform)
            {
                if (LookCondition)
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
            //Left Foot
            ray = new Ray(animator.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);
            if (Physics.Raycast(ray, out hit, IKLeftFootDistance + 1f, layerMask))
            {
                if (hit.transform.CompareTag(walkableTag))
                {
                    Vector3 footPos = hit.point;

                    footPos.y += IKLeftFootDistance;
                    animator.SetIKPosition(AvatarIKGoal.LeftFoot, footPos);
                    animator.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(-animator.GetBoneTransform(HumanBodyBones.LeftFoot).up, hit.normal));
                }
            }

            //Right Foot
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

    #endregion



    #region EventMethods

    void OnEnable()
    {
        TreeMalarkey.RegisterEventOnTree(cloneTree, "PrepDespawn", PrepDespawn);
    }

    void OnDisable()
    {
        TreeMalarkey.UnregisterEventOnTree(cloneTree, "PrepDespawn", PrepDespawn);
    }

    #endregion
}
