using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

public class OverseerController : MonoBehaviour
{
    static BehaviorTree ObserverTree;
    CrossyTheWatcher titan;

    #region Fields
    [SerializeField] private bool startOnAwake;
    [SerializeField] private bool usePositioner;

    [Header("GameObjects")]
    public GameObject validationPositioner;

    private GameObject m_Observer;
    [SerializeField] private GameObject m_Crossy;
    [SerializeField] private GameObject m_TitanCrossy;
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

    [Header("Titan Crossy Placement")]
    [SerializeField] private float m_CheckRadius;
    private int m_State;
    #endregion

    #region Properties
    public GameObject Observer { get { return m_Observer; } }
    public GameObject MrCrossy { get { return m_Crossy; } }
    public GameObject TitanCrossy { get { return m_TitanCrossy; } }
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
    public int State { get { return m_State; } set { m_State = value; } }

    public List<GameObject> SpawnLightHouses { get { return m_SpawnLighthouses; } }

    #endregion

    [Space(10)]
    public List<Lighthouse> titanLighthouses = new List<Lighthouse>();

    [SerializeField] private List<GameObject> m_SpawnLighthouses = new List<GameObject>();

    public Lighthouse storedHouse;
    public float currDist;
    private float storedDist = 0;

    [SerializeField] private bool doing;

    private void Awake()
    {
        m_Observer = gameObject;
        ObserverTree = gameObject.GetComponent<BehaviorTree>();
        if (startOnAwake)
        {
            AwakenObserver();
        } else ObserverTree.enabled = false;
        titan = m_TitanCrossy.GetComponent<CrossyTheWatcher>();
    }

    private void Update()
    {
        if (usePositioner) m_ValidationPosition = validationPositioner.transform.position;

        titan.m_state = m_State;

        if(m_State == -1)
        {
            bool left = LeftRadius();

            if (left)
            {
                CheckClosestLighthouse();
            }
        }
    }


    public static void AwakenObserver()
    {
        ObserverTree.enabled = true;
    }

    public bool LeftRadius()
    {
        Vector3 playerPosition = new Vector3(m_Player.transform.position.x, 0f, m_Player.transform.position.z);
        Vector3 check = new Vector3(titan.lighthouse.selfTransform.position.x, 0f, titan.lighthouse.selfTransform.position.z);
        float dist = Vector3.Distance(playerPosition, check);

        if (dist > m_CheckRadius)
        {
            Debug.Log("Checkky");
            return true;
        }
        else 
        { 
            return false; 
        }
    }

    public void CheckClosestLighthouse()
    {
        Debug.Log("StartLight");
        Vector3 playerPosition = new Vector3(m_Player.transform.position.x, 0f, m_Player.transform.position.z);

        storedHouse = titan.lighthouse;

        foreach(Lighthouse lighthouse in titanLighthouses)
        {
            Vector3 check = new Vector3(lighthouse.selfTransform.position.x, 0f, lighthouse.selfTransform.position.z);

            currDist = Vector3.Distance(playerPosition, check);


            if (storedDist == 0) 
            { 
                storedDist = currDist;
                Debug.Log("setdist");
            }

            if(currDist <= storedDist)
            {
                Debug.Log("SetHouse");
                storedDist = currDist;
                storedHouse = lighthouse;
            }

        }
        if(storedHouse != titan.lighthouse && !titan.lighthousing)
        {
            Debug.Log("DiffLight");
            titan.TitanCrossyHouse(storedHouse);
        }
        storedDist = 0;
        Debug.Log("EndLight");
    }
}
