using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gemstone;
using BehaviorDesigner.Runtime;
using UnityEngine.AI;
using FMOD.Studio;
using FMODUnity;

public class OverseerController : MonoBehaviour
{
    public static BehaviorTree ObserverTree;
    public bool m_PlayerInHouse;

    CrossyTheWatcher titan;
    MrCrossyDistortion distootle;
    CrossKeyManager keyMan;
    NavMeshAgent crossyAgent;

    public EmitterRef emitter;
    EventInstance eventInstance;

    public bool deady = false;
    
    #region Fields
    [SerializeField] private bool startOnAwake;
    [SerializeField] private bool usePositioner;

    [Header("GameObjects")]
    public GameObject validationPositioner;

    private GameObject m_Observer;
    [SerializeField] private GameObject m_Crossy;
    [SerializeField] private GameObject m_TitanCrossy;
    [SerializeField] private GameObject m_TitanHead;
    [SerializeField] private GameObject m_Player;
    public CrossyStreetStalk m_StalkStreet;
    //[Space(10)]
    [Header("Behaviour Timers")]
    [SerializeField] private float m_TimeSpawn;
    [SerializeField] private float m_TimeTight;
    [SerializeField] private float m_TimePosUpdate;
    [SerializeField] private float m_TimePosUpdateAlert;
    [SerializeField] private float m_TimeCalm;
    [SerializeField] private float m_TimePursuitSpawn;
    [SerializeField] private float m_TimePursuitBreak;
    [SerializeField] private float m_TimeStreetStalkTick;
    [SerializeField] private float m_TimeStalkLimit;
    [SerializeField] private float m_StreetStalkChance;
    //[Space(10)]
    [Header("Area Parameters")]
    [SerializeField] private float m_SearchRadiusMax;
    [SerializeField] private float m_SearchRadiusMin;
    [SerializeField] private float m_SearchTightAmt;
    [SerializeField] private float m_SearchRadiusAggro;
    [SerializeField] private Vector3 m_ValidationPosition;
    [SerializeField] private List<GameObject> m_SpawnLighthouses = new List<GameObject>();

    [Header("Titan Crossy")]
    [SerializeField] private float m_CheckRadius;
    private int m_State;
    private bool m_HideTitan;

    [Header("FMOD Variables")]
    [SerializeField] private string distanceParamName = "Distance";
    [SerializeField] private string chaseParamName = "IsChasing";
    [SerializeField] private string deadParamName = "isDead";

    [Range(0,1)] [SerializeField] private float m_FMODDistanceMod = 1f;

    [Range(0f, 1f)] [SerializeField] private float m_TitanVoiceLineChance = 0.5f;

    [SerializeField] private bool m_IsTutorial = true;
    #endregion

    #region Properties
    public GameObject Observer { get { return m_Observer; } }
    public GameObject MrCrossy { get { return m_Crossy; } }
    public GameObject TitanCrossy { get { return m_TitanCrossy; } }
    public GameObject Player { get { return m_Player; } }

    public CrossyStreetStalk StalkStreet { get { return m_StalkStreet; } set { m_StalkStreet = value; } }
    public Vector3 StalkStreetPos { get { return m_StalkStreet.transform.position; } }
    public List<GameObject> StalkStreetPoints { get { return m_StalkStreet.m_StreetStalkPoints; } }

    public float TimeTilSpawn { get { return m_TimeSpawn; } set { m_TimeSpawn = value; } }
    public float TimeTilTighten { get { return m_TimeTight; } }
    public float TimeTilPositionUpdate { get { return m_TimePosUpdate; } }
    public float TimeTilAlertPosUpdate { get { return m_TimePosUpdateAlert; } }
    public float TimeTilPursuitedRespawn { get { return m_TimePursuitSpawn; } }
    public float TimeTilCalmed { get { return m_TimeCalm; } }
    public float TimeTilPursuitBreak { get { return m_TimePursuitBreak; } }
    public float TimeStreetStalkTick { get { return m_TimeStreetStalkTick; } }
    public float TimeStalkLimit { get { return m_TimeStalkLimit; } }
    public float StreetStalkChance { get { return m_StreetStalkChance; } }

    public float SearchAreaTightenAmount { get { return m_SearchTightAmt; } }
    public float SearchAreaRadiusMax { get { return m_SearchRadiusMax; } }
    public float SearchAreaRadiusMin { get { return m_SearchRadiusMin; } }
    public float SearchAreaRadiusAlert { get { return m_SearchRadiusAggro; } }

    public Vector3 ValidationPosition { get { return m_ValidationPosition; } }
    public int State { get { return m_State; } set { m_State = value; } }
    public bool HideTitan { get { return m_HideTitan; } set { m_HideTitan = value; } }

    public List<GameObject> SpawnLightHouses { get { return m_SpawnLighthouses; } }

    public bool IsTutorial { get { return m_IsTutorial; } set { m_IsTutorial = value; } }

    public bool IsInHouse { get { return m_PlayerInHouse; } set { m_PlayerInHouse = value; } }

    #endregion

    [Space(10)]
    public List<Lighthouse> titanLighthouses = new List<Lighthouse>();


    public Lighthouse storedHouse;
    public float currDist;
    private float storedDist = 0;
    bool vignetteActivated = false;
    float pathDistance = 100f;
    [SerializeField] bool playedTitanLine = false;
    private void Awake()
    {
        m_Observer = gameObject;
        ObserverTree = gameObject.GetComponent<BehaviorTree>();
        distootle = FindObjectOfType<MrCrossyDistortion>();
        keyMan = FindObjectOfType<CrossKeyManager>();


        emitter.Target.SetParameter(distanceParamName, 100f);
        emitter.Target.SetParameter(chaseParamName, 1f);
        emitter.Target.SetParameter(deadParamName, 1f);

        if (startOnAwake)
        {
            TreeMalarkey.EnableTree(ObserverTree);
        }
        else TreeMalarkey.DisableTree(ObserverTree);
        titan = m_TitanCrossy.GetComponent<CrossyTheWatcher>();

        crossyAgent = m_Crossy.GetComponent<NavMeshAgent>();
    }


    private void Update()
    {
        if (usePositioner) m_ValidationPosition = validationPositioner.transform.position;

        titan.m_state = m_State;
        titan.hidingTitan = m_HideTitan;

        if(m_State == -1 && !m_IsTutorial)
        {
            bool left = LeftRadius();

            if (left)
            {
                CheckClosestLighthouse();
            }
        }

        if(m_State == 3)
        {
            vignetteActivated = true;
            Debug.Log("potoatosondwich");
            distootle.DistanceVignette(m_Crossy);
            if(!keyMan.doorsLocked)
            {
                keyMan.doorsLocked = true;
            }
        }
        else if (m_State != 3 && vignetteActivated)
        {
            vignetteActivated = false;
            Debug.Log("VignetteNooooooo");
            distootle.DecreaseVignette();
        }

        if(!m_IsTutorial)
        {
            if (m_State >= 1) CrossyFMODFiddling(crossyAgent, m_Player.transform.position, m_State, deady);
            if (m_State == -1) TitanCrossyVoiceLines();
        }

        

    }

    #region Methods

    public void SetStalkies(CrossyStreetStalk stalky)
    {
        m_StalkStreet = stalky;
    }

    public void TutorialActive()
    {
        m_IsTutorial = true;
        titan.isTutorial = true;
    }

    public void TutorialEnded()
    {
        m_IsTutorial = false;
        titan.isTutorial = false;
    }

    public void AwakenObserver()
    {
        TreeMalarkey.EnableTree(ObserverTree);
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

    public void CrossyFMODFiddling(NavMeshAgent agent, Vector3 playerPos, int state, bool dead)
    {
        
        pathDistance = Mathf.Clamp(pathDistance, 0f, 100f);

        NavMeshPath navPath = new NavMeshPath();

        if(agent.CalculatePath(playerPos, navPath))
        {
            if(navPath.status == NavMeshPathStatus.PathComplete)
            {
                pathDistance = Emerald.GetPathLength(navPath);
                Debug.Log("CrossyFMOD PathDistance: " + pathDistance);
            }
            
        }

        if(pathDistance <= 100f)
        {
            Debug.Log("CrossyFMOD ShouldBeSetting");
            emitter.Target.SetParameter(distanceParamName, pathDistance);
            Debug.Log("CrossyFMOD DidSet");
        }
        else emitter.Target.SetParameter(distanceParamName, 100f);

        if (state == 3)
        {
            emitter.Target.SetParameter(chaseParamName, 0f);
        }
        else emitter.Target.SetParameter(chaseParamName, 1f);

        if (dead)
        {
            emitter.Target.SetParameter(deadParamName, 0f);
        }
        else emitter.Target.SetParameter(deadParamName, 1f);

    }
    #endregion


    #region VoiceStuff

    void TitanCrossyVoiceLines()
    {
        if (titan.animator.GetCurrentAnimatorStateInfo(0).IsName("TitanCrossyIdle") && !playedTitanLine)
        {
            float probability = Random.Range(0f, 1f);
            Debug.Log("TitanFMOD probability: " + probability);
            if (probability < m_TitanVoiceLineChance)
            {
                eventInstance = RuntimeManager.CreateInstance("event:/MR_C_Titan/Titan_Oneliners");
                eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(m_TitanHead));
                Debug.Log("TitanFMOD TitanPlayLine");
                eventInstance.start();
            }

            playedTitanLine = true;
        }
        else if (titan.animator.GetCurrentAnimatorStateInfo(0).IsName("TitanCrossyIdleHidden")) 
        {
            Debug.Log("TitanFMOD TitanPlayReset");
            eventInstance.release();
            playedTitanLine = false; 
        }
    }

    #endregion
    }
