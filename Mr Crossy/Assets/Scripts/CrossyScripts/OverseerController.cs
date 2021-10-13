
using UnityEngine;
using BehaviorDesigner.Runtime;

public class OverseerController : MonoBehaviour
{
    static BehaviorTree ObserverTree;

    [SerializeField] private bool startOnAwake;
    [SerializeField] private bool usePositioner;

    [Header("GameObjects")]
    public GameObject validationPositioner;

    private GameObject m_Observer;
    [SerializeField] private GameObject m_Crossy;
    [SerializeField] private GameObject m_Player;
    //[Space(10)]
    [Header("Behaviour Timers")]
    [SerializeField] private float m_TimeSpawn;
    [SerializeField] private float m_TimeTight;
    [SerializeField] private float m_TimePosUpdate;
    [SerializeField] private float m_TimePosUpdateAlert;
    [SerializeField] private float m_TimeCalm;
    [SerializeField] private float m_TimePursuitSpawn;
    [SerializeField] private float m_TimePursuitBreak;
    //[Space(10)]
    [Header("Area Parameters")]
    [SerializeField] private float m_SearchRadiusMax;
    [SerializeField] private float m_SearchRadiusMin;
    [SerializeField] private float m_SearchTightAmt;
    [SerializeField] private float m_SearchRadiusAggro;
    [SerializeField] private Vector3 m_ValidationPosition;

    public GameObject Observer { get { return m_Observer; } }
    public GameObject MrCrossy { get { return m_Crossy; } }
    public GameObject Player { get { return m_Player; } }

    public float TimeTilSpawn { get { return m_TimeSpawn; } set { m_TimeSpawn = value; } }
    public float TimeTilTighten { get { return m_TimeTight; } }
    public float TimeTilPositionUpdate { get { return m_TimePosUpdate; } }
    public float TimeTilAlertPosUpdate { get { return m_TimePosUpdateAlert; } }
    public float TimeTilPursuitedRespawn { get { return m_TimePursuitSpawn; } }
    public float TimeTilCalmed { get { return m_TimeCalm; } }
    public float TimeTilPursuitBreak { get { return m_TimePursuitBreak; } }

    public float SearchAreaTightenAmount { get { return m_SearchTightAmt; } }
    public float SearchAreaRadiusMax { get { return m_SearchRadiusMax; } }
    public float SearchAreaRadiusMin { get { return m_SearchRadiusMin; } }
    public float SearchAreaRadiusAlert { get { return m_SearchRadiusAggro; } }

    public Vector3 ValidationPosition { get { return m_ValidationPosition; } }

    private void Awake()
    {
        m_Observer = gameObject;
        ObserverTree = gameObject.GetComponent<BehaviorTree>();
        if (startOnAwake)
        {
            AwakenObserver();
        } else ObserverTree.enabled = false;
    }

    private void Update()
    {
        if (usePositioner) m_ValidationPosition = validationPositioner.transform.position;


    }


    public static void AwakenObserver()
    {
        ObserverTree.enabled = true;
    }
}
