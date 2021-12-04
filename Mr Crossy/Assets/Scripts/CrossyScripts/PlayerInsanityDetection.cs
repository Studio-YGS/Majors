using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInsanityDetection : MonoBehaviour
{
    private MrCrossyDistortion distortion;
    public bool allowInsanity = true;

    private GameObject m_Crossy;
    OverseerController seer;
    [SerializeField] private Transform m_Camera;

    [SerializeField] private float m_FieldOfView;
    [SerializeField] private float m_VisualDistance;
    [SerializeField] private float m_PhysicalDistance;

    private bool m_Insane;

    private bool calledInsane;


    public GameObject Crossy { get { return m_Crossy; } set { m_Crossy = value; } }
    public Transform Camera { get { return m_Camera; } }

    public float FieldOfView { get { return m_FieldOfView; } set { m_FieldOfView = value; } }
    public float VisualDistance { get { return m_VisualDistance; } set { m_VisualDistance = value; } }
    public float PhysicalDistance { get { return m_PhysicalDistance; } set { m_PhysicalDistance = value; } }

    public bool Insane { get { return m_Insane; } set { m_Insane = value; } }

    void Awake()
    {
        distortion = FindObjectOfType<MrCrossyDistortion>();
        seer = FindObjectOfType<OverseerController>();
    }

    // Update is called once per frame
    void Update()
    {
        //if(m_Insane)
        //{
        //    calledInsane = true;
        //    distortion.IncreaseInsanity(m_Crossy);
        //}
        //else if(!m_Insane && calledInsane)
        //{
        //    calledInsane = false;
        //    distortion.ReduceInsanity();
        //}

        if(allowInsanity)
        {
            //float distance = Vector3.Distance(transform.position, m_Crossy.transform.position);
            Debug.Log("SANE Insanity Distance: " + OverseerController.CrossyPathDistance);
            if (!seer.m_PlayerInHouse && seer.State != -1)
            {
                if (OverseerController.CrossyPathDistance < 25f && OverseerController.CrossyPathDistance > 15f)
                {
                    calledInsane = true;
                    distortion.IncreaseInsanity(m_Crossy);
                }
                else if (OverseerController.CrossyPathDistance < 15f && OverseerController.CrossyPathDistance > 10f)
                {
                    distortion.increasingInsanity = false;
                }
                else if (OverseerController.CrossyPathDistance < 10f)
                {
                    distortion.LerpInsanity();
                }
                else if (OverseerController.CrossyPathDistance > 25f && calledInsane)
                {
                    distortion.ReduceInsanity();
                    calledInsane = false;
                }
            }
            else if (calledInsane)
            {
                distortion.ReduceInsanity();
                calledInsane = false;
            }
        }
        else if (calledInsane)
        {
            distortion.ReduceInsanity();
            calledInsane = false;
        }
    }
}
