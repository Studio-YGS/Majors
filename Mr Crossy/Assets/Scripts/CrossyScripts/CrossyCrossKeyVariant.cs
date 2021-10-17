using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class CrossyCrossKeyVariant : MonoBehaviour
{
    Animator animator;
    NavMeshAgent agent;

    public bool run;

    [Space(10)]
    [Header("Movement Variables")]
    [Tooltip("Mr. Crossy's walking speed.")]
    [SerializeField] private float m_WalkSpeed;
    [Tooltip("Mr. Crossy's running speed.")]
    [SerializeField] private float m_RunSpeed;
    private float m_MoveSpeed;
    

    float veloMag;

    [Space(10)]
    [Header("IK Variables")]
    public LayerMask layerMask;
    public string walkableTag;

    [Range(0, 1)] public float lookAtWeight;
    public Transform lookAtTransform;
    public Vector3 lookAtOffset;
    public float IKLeftFootDistance;
    public float IKRightFootDistance;

    [SerializeField] private float distanceToAtk;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        veloMag = agent.velocity.magnitude;

        m_MoveSpeed = (run) ? m_RunSpeed : m_WalkSpeed;
        agent.speed = m_MoveSpeed;

        animator.SetBool("Moving", veloMag >= 0.05);
        animator.SetFloat("VelocityMag", veloMag);

        if (agent.remainingDistance <= distanceToAtk) { animator.SetTrigger("DoSwing"); animator.SetBool("CanSwing", false); }
        else animator.SetBool("CanSwing", true);
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (animator)
        {
            //Noggin Malarkey
            if (lookAtTransform)
            {
                animator.SetLookAtPosition(lookAtTransform.position + lookAtOffset);
                animator.SetLookAtWeight(lookAtWeight);
            }

            //Foot Stuff
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1f);
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1f);

            RaycastHit hit;
            Ray ray;
            //LeftFoot
            ray = new Ray(animator.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);
            if (Physics.Raycast(ray, out hit, IKLeftFootDistance + 1f, layerMask))
            {
                if (hit.transform.CompareTag(walkableTag))
                {
                    Vector3 footPos = hit.point;

                    footPos.y += IKLeftFootDistance;
                    animator.SetIKPosition(AvatarIKGoal.LeftFoot, footPos);

                    animator.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(transform.forward, hit.normal));
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
