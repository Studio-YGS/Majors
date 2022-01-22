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
#if UNITY_EDITOR
    [Header("EditorOnly")]
    public bool CrossyQuickSpawn;
    public float QuickTime;
#endif

    public static BehaviorTree ObserverTree;

    public static float CrossyPathDistance = 0f;

    public bool m_PlayerInSafeHouse;
    public bool m_PlayerInStreetHouse;
    public bool allowVignette = true;

    [HideInInspector]
    public CrossyTheWatcher titan;
    MrCrossyDistortion distootle;
    CrossKeyManager keyMan;
    NavMeshAgent crossyAgent;
    JournalController journCont;
    JournalOnSwitch journSwitch;
    MenuManager menu;
    CrossyController crossyController;

    public EmitterRef emitter;
    EventInstance eventInstance;

    public bool deady = false;
    
    #region Fields
    [SerializeField] private bool startOnAwake;
    [SerializeField] private bool usePositioner;
    [SerializeField] private Vector3 validationOffset;

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

    public List<GameObject> houseScoutPoints = new List<GameObject>();
    public List<float> houseScoutRadius = new List<float>();

    public List<GameObject> distOneSpawnLighthouses = new List<GameObject>();
    public List<GameObject> distTwoSpawnLighthouses = new List<GameObject>();
    public List<GameObject> distThreeSpawnLighthouses = new List<GameObject>();
    private List<GameObject> m_SpawnLighthouses = new List<GameObject>();

    [Header("Heal 'N' Die")]
    [SerializeField] private int m_DeathChanceMax;
    private int m_DeathChanceRemain;
    [SerializeField] private float m_HealTimer;

    [Header("Titan Crossy")]
    [SerializeField] private float m_CheckRadius;
    private int m_State;
    private bool m_HideTitan;
    private bool m_TimerActive;
    [SerializeField] private float m_TitanAwakenThresh;

    [Header("FMOD Variables")]
    //public string distanceParamName = "Distance";
    //public string chaseParamName = "IsChasing";
    //public string deadParamName = "isDead";
    //public string titanParamName = "Titan";

    [SerializeField] bool attemptingSafe = false;
    public bool attemptingDie = false;
    [SerializeField] bool hasChased = false;
    [SerializeField] bool fiddleFMOD = false;

    [Range(0f, 1f)] [SerializeField] private float m_TitanVoiceLineChance = 0.5f;
    private int m_TitanNoisyNum = 0;
    [Space(10)]
    public bool m_IsTutorial = true;

    private bool m_InSight;

    public List<CloneController> crossyClones = new List<CloneController>();

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

    public float HealTimer { get { return m_HealTimer; } }
    public int DeathChanceMaximum { get { return m_DeathChanceMax; } }
    public int DeathChanceRemaining { get { return m_DeathChanceRemain; } set { m_DeathChanceRemain = value; } }

    public Vector3 ValidationPosition { get { return (usePositioner) ? validationPositioner.transform.position : m_ValidationPosition; } }
    public int State { get { return m_State; } set { m_State = value; } }
    public bool HideTitan { get { return m_HideTitan; } set { m_HideTitan = value; } }

    public List<GameObject> SpawnLightHouses { get { return m_SpawnLighthouses; } }

    public bool IsTutorial { get { return m_IsTutorial; } set { m_IsTutorial = value; } }

    public bool IsInSafeHouse { get { return m_PlayerInSafeHouse; } set { m_PlayerInSafeHouse = value; } }
    public bool IsInStreetHouse { get { return m_PlayerInStreetHouse; } set { m_PlayerInStreetHouse = value; } }



    public bool Chasing { get { return hasChased; } }
    public bool AttemptingSafe { get { return attemptingSafe; } }
    public bool AttemptingDie { get { return attemptingDie; } }
    public bool Puzzling { get { return keyMan.puzzleOn; } }

    public bool InSight { get { return m_InSight; } set { m_InSight = value; } }

    #endregion

    [Space(10)]

    public List<Lighthouse> districtOneLighthouses = new List<Lighthouse>();
    public List<Lighthouse> districtTwoLighthouses = new List<Lighthouse>();
    public List<Lighthouse> districtThreeLighthouses = new List<Lighthouse>();

    [HideInInspector] public List<Lighthouse> titanLighthouses = new List<Lighthouse>();


    public Lighthouse storedHouse;
    public float currDist;
    private float storedDist = 0;
    bool vignetteActivated = false;
    [SerializeField] bool playedTitanLine = false;
    private void Awake()
    {
#if UNITY_EDITOR
        if (CrossyQuickSpawn) m_TimeSpawn = QuickTime;
#endif

        m_Observer = gameObject;
        ObserverTree = gameObject.GetComponent<BehaviorTree>();
        distootle = FindObjectOfType<MrCrossyDistortion>();
        keyMan = FindObjectOfType<CrossKeyManager>();
        journCont = FindObjectOfType<JournalController>();
        journSwitch = FindObjectOfType<JournalOnSwitch>();
        menu = FindObjectOfType<MenuManager>();

        if (m_Player == null) m_Player = GameObject.Find("Fps Character");

        
        emitter.Params[0].Value = 100f;
        emitter.Params[1].Value = 1f;
        emitter.Params[2].Value = 1f;
        emitter.Params[3].Value = 0f;

        ParameterHell();


        if (startOnAwake)
        {
            TreeMalarkey.EnableTree(ObserverTree);
        }
        else TreeMalarkey.DisableTree(ObserverTree);
        titan = m_TitanCrossy.GetComponent<CrossyTheWatcher>();

        crossyAgent = m_Crossy.GetComponent<NavMeshAgent>();
        crossyController = m_Crossy.GetComponent<CrossyController>();

    }

    private void Update()
    {
        //if (usePositioner) m_ValidationPosition = validationPositioner.transform.position;

        titan.m_state = m_State;
        titan.hidingTitan = m_HideTitan;

        NavAreaDetection();

        if(!m_IsTutorial)
        {
            if (m_State >= 1 && !m_PlayerInSafeHouse || keyMan.puzzleOn) fiddleFMOD = true;
            else fiddleFMOD = false;

            GetCrossyPlayerDistance();

            if (fiddleFMOD) CrossyFmodDistance();
            else
            {
                if(emitter.Params[0].Value != 100f)
                {
                    emitter.Params[0].Value = 100f;
                    emitter.Target.SetParameter(emitter.Params[0].Name, emitter.Params[0].Value);
                }
            }

            TitanUppyDownyNoisies();
            VignetteProcessor();

            if (m_State == -1)
            {
                if (titan.animator.GetCurrentAnimatorStateInfo(0).IsName("TitanCrossyIdle"))
                {
                    if (!m_TimerActive && !titan.allowHide)
                    {
                        StartCoroutine(AllowHideTimer());
                    }
                }

                if (!m_PlayerInSafeHouse)
                {
                    TitanCrossyVoiceLines();
                    
                }
                if (!titan.lighthousing)
                {
                    bool left = LeftRadius();

                    if (left)
                    {
                        if (titan.allowHide) CheckClosestLighthouse();
                    }
                }
                
            }

            if (m_State == 3)
            {
                if (journSwitch.open) journSwitch.ForceOpenOrClose();
                if (!journCont.disabled && !menu.menuOpen) journCont.disabled = true;
            }
            else
            {
                if (journCont.disabled) journCont.disabled = false;
            }
        }
        if (!hasChased && m_State == 3)
        {
            PursuitAudio();
        }
        else if (hasChased)
        {
            if(deady)
            {
                if (!attemptingSafe && !attemptingDie) DeadAudio();
            }
            else if (m_State != 3 && !keyMan.puzzleOn && !deady || m_PlayerInSafeHouse && !keyMan.puzzleOn)
            {
                if (!attemptingSafe && !attemptingDie) StartCoroutine(WaitForPuzzleOff());
            }
        }

        PlayerIsBeingSeen();

    }

    #region Methods

    IEnumerator AllowHideTimer()
    {
        m_TimerActive = true;

        yield return new WaitForSeconds(m_TitanAwakenThresh);
        titan.allowHide = true;
        m_TimerActive = false;
    }

    IEnumerator TitanSoundNeutral()
    {
        m_TitanNoisyNum = 0;
        yield return new WaitForSeconds(1);
        emitter.Params[3].Value = m_TitanNoisyNum;
        emitter.Target.SetParameter(emitter.Params[3].Name, emitter.Params[3].Value);
    }

    public void VignetteProcessor()
    {
        if (m_State == 3)
        {
            if (allowVignette)
            {
                vignetteActivated = true;
                //Debug.Log("potoatosondwich");
                distootle.DistanceVignette(m_Crossy);
            }

            if (!keyMan.doorsLocked)
            {
                keyMan.doorsLocked = true;
            }
        }
        else if (m_State != 3)
        {
            if (allowVignette)
            {
                if(vignetteActivated && !deady)
                {
                    vignetteActivated = false;
                    //Debug.Log("VignetteNooooooo");
                    distootle.DecreaseVignette();
                }
            }

            if (keyMan.doorsLocked)
            {
                keyMan.doorsLocked = false;
            }
        }
    }
    public void NavAreaDetection()
    {
        NavMeshHit baseHit;
        
        NavMesh.SamplePosition(m_Player.transform.position, out baseHit, 1, NavMesh.AllAreas);
        Debug.Log("NavArea: " + baseHit.mask.ToString());
        if (baseHit.mask == 256)
        {
            NavMeshHit validationHit;

            m_PlayerInSafeHouse = true;
            m_PlayerInStreetHouse = false;
            if(keyMan.doorsLocked) keyMan.doorsLocked = false;

            

            NavMesh.SamplePosition(validationPositioner.transform.position, out validationHit, 1, NavMesh.AllAreas);
            if(validationHit.mask == 256)
            {
                NavMeshHit validHit;
                if(NavMesh.SamplePosition(validationPositioner.transform.position, out validHit, 3, 8))
                {
                    validationPositioner.transform.position = validHit.position;
                }
                else if (NavMesh.SamplePosition(validationPositioner.transform.position, out validHit, 3, 16))
                {
                    validationPositioner.transform.position = validHit.position;
                }
                else if (NavMesh.SamplePosition(validationPositioner.transform.position, out validHit, 3, 32))
                {
                    validationPositioner.transform.position = validHit.position;
                }
            }
        }
        else if (baseHit.mask == 8 || baseHit.mask == 16 || baseHit.mask == 32)
        {
            m_PlayerInSafeHouse = false;
            m_PlayerInStreetHouse = false;

            validationPositioner.transform.position = new Vector3
            (
                m_Player.transform.position.x,
                baseHit.position.y,
                m_Player.transform.position.z
            );
        }
        else if(baseHit.mask == 512)
        {
            m_PlayerInSafeHouse = false;
            m_PlayerInStreetHouse = true;

            validationPositioner.transform.position = new Vector3
            (
                m_Player.transform.position.x,
                baseHit.position.y,
                m_Player.transform.position.z
            );
        }
    }

    public void SetStalkies(CrossyStreetStalk stalky)
    {
        m_StalkStreet = stalky;
    }

    public void PlayerIsBeingSeen()
    {
        if(crossyClones.Count == 0)
        {
            InSight = crossyController.InOwnSight;
        }
        else
        {
            InSight = crossyController.InOwnSight || CloneSight();
        }
    }

    bool CloneSight()
    {
        foreach(CloneController clone in crossyClones)
        {
            if(clone.InOwnSight)
            {
                return true;
            }
        }

        return false;
    }

    public void SetLighthouseGroup(int district)
    {
        switch (district)
        {
            case 1:
                {
                    //titanLighthouses.Clear();
                    m_SpawnLighthouses = distOneSpawnLighthouses;
                    titanLighthouses = districtOneLighthouses;
                    crossyController.HouseScoutVariableSetter(houseScoutPoints[district-1].transform.position, houseScoutRadius[district-1]);
                    break;
                }
            case 2:
                {
                    //titanLighthouses.Clear();
                    m_SpawnLighthouses = distTwoSpawnLighthouses;
                    titanLighthouses = districtTwoLighthouses;
                    crossyController.HouseScoutVariableSetter(houseScoutPoints[district - 1].transform.position, houseScoutRadius[district - 1]);
                    break;
                }
            case 3:
                {
                    //titanLighthouses.Clear();
                    m_SpawnLighthouses = distThreeSpawnLighthouses;
                    titanLighthouses = districtThreeLighthouses;
                    break;
                }
        }
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
        SetLighthouseGroup(1);
        TreeMalarkey.EnableTree(ObserverTree);
        distootle.ShoobyDooby();
        CheckClosestLighthouse();
        m_Crossy.GetComponent<CrossyController>().RegisterEvents();
        ObserverRegister();
    }

    public bool LeftRadius()
    {
        Vector3 playerPosition = new Vector3(m_Player.transform.position.x, 0f, m_Player.transform.position.z);
        //Vector3 check = new Vector3(titan.lighthouse.selfTransform.position.x, 0f, titan.lighthouse.selfTransform.position.z);
        Vector3 check = titan.lighthouse.CheckCentre;
        float dist = Vector3.Distance(playerPosition, check);

        if (dist > titan.lighthouse.checkRadius)
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

        if (!titan.lightInit) titan.lightInit = true;
        
    }

    public void GetCrossyPlayerDistance()
    {
        NavMeshPath navPath = new NavMeshPath();

        if(NavMesh.CalculatePath(m_Crossy.transform.position, m_Player.transform.position, crossyAgent.areaMask, navPath))
        {
            if(navPath.status != NavMeshPathStatus.PathInvalid)
            {
                CrossyPathDistance = Emerald.GetPathLength(navPath);
            }
        }
    }

    public void ParameterHell()
    {
        emitter.Target.SetParameter(emitter.Params[0].Name, emitter.Params[0].Value);
        emitter.Target.SetParameter(emitter.Params[1].Name, emitter.Params[1].Value);
        emitter.Target.SetParameter(emitter.Params[2].Name, emitter.Params[2].Value);
        emitter.Target.SetParameter(emitter.Params[3].Name, emitter.Params[3].Value);
    }

    public void CrossyFmodDistance()
    {
        if(CrossyPathDistance <= 100)
        {
            emitter.Params[0].Value = CrossyPathDistance;
            emitter.Target.SetParameter(emitter.Params[0].Name, emitter.Params[0].Value);
        }
        else
        {
            emitter.Params[0].Value = 100f;
            emitter.Target.SetParameter(emitter.Params[0].Name, emitter.Params[0].Value);
        }
    }

    public void CrossyFMODFiddling(float pathDistance, int state, bool dead)
    {

        if (pathDistance <= 100f && state != -1)
        {
            Debug.Log("CrossyFMOD ShouldBeSetting");
            emitter.Params[0].Value = pathDistance;
            emitter.Target.SetParameter(emitter.Params[0].Name, emitter.Params[0].Value);
            Debug.Log("ADAPTIVE: Distance: " + pathDistance);
        }
        else
        {
            emitter.Params[0].Value = 100f;
            emitter.Target.SetParameter(emitter.Params[0].Name, emitter.Params[0].Value);
        }

        
        if (!m_PlayerInSafeHouse && state < 3 && !keyMan.puzzleOn)
        {
            if (hasChased && !attemptingSafe && !keyMan.puzzleOn)
            {
                StartCoroutine(WaitForPuzzleOff());
                Debug.Log("ADAPTIVE: Chase Stopped: State = " + state + ", PuzzleOn = " + keyMan.puzzleOn);
            }
        }
        else if (state == 3 && !attemptingSafe && !attemptingDie || keyMan.puzzleOn && !attemptingSafe && !attemptingDie)
        {
            hasChased = true;
            if (emitter.Params[1].Value != 0f)
            {
                emitter.Params[1].Value = 0f;
                emitter.Target.SetParameter(emitter.Params[1].Name, emitter.Params[1].Value);
            }
            Debug.Log("ADAPTIVE: Chase Started: State = " + state + ", PuzzleOn = " + keyMan.puzzleOn);
        }


        //if (dead)
        //{
        //    if(!attemptingDie) DeadNoises();
        //    Debug.Log("ADAPTIVE: Dead Played: " + 0f);
        //}
        //else if(!dead && !attemptingSafe)
        //{ 
        //    emitter.Target.SetParameter(deadParamName, 1f);
        //    Debug.Log("ADAPTIVE: Not Dead/Safe: " + 1f);
        //}

        if (m_PlayerInSafeHouse)
        {
            if (hasChased && !attemptingSafe && !attemptingDie)
            {
                StartCoroutine(WaitForPuzzleOff());
            }
        }

        
    } //Obsolete hopefully

    IEnumerator WaitForPuzzleOff()
    {
        attemptingSafe = true;
        //yield return new WaitForSecondsRealtime(0.5f);
        while (keyMan.puzzleOn)
        {
            yield return null;
        }

        Safe();

        yield return new WaitForSeconds(1f);

        if (emitter.Params[2].Value != 1f)
        {
            emitter.Params[2].Value = 1f;
            emitter.Target.SetParameter(emitter.Params[2].Name, emitter.Params[2].Value);
        }

        hasChased = false;
        Debug.Log("ADAPTIVE: Safe Played: " + 2f);
        attemptingSafe = false;
        StopCoroutine(WaitForPuzzleOff());
    }

    void Safe()
    {
        Debug.Log("ADAPTIVE: Safe Method Called");
        if (emitter.Params[1].Value != 1f)
        {
            Debug.Log("ADAPTIVE: Chasing set false");
            emitter.Params[1].Value = 1f;
            emitter.Target.SetParameter(emitter.Params[1].Name, emitter.Params[1].Value);
        }
        if (emitter.Params[2].Value != 2f)
        {
            emitter.Params[2].Value = 2f;
            emitter.Target.SetParameter(emitter.Params[2].Name, emitter.Params[2].Value);
        }
    }

    public void DeadNoises()
    {
        Debug.Log("ADAPTIVE: Dead Method Called");
        StartCoroutine(DeadSounds());
    }
    IEnumerator DeadSounds()
    {
        attemptingDie = true;

        if (emitter.Params[2].Value != 0f)
        {
            emitter.Params[2].Value = 0f;
            emitter.Target.SetParameter(emitter.Params[2].Name, emitter.Params[2].Value);
        } //Makes dead
        if (emitter.Params[1].Value != 1f)
        {
            Debug.Log("ADAPTIVE: Chasing set false");
            emitter.Params[1].Value = 1f;
            emitter.Target.SetParameter(emitter.Params[1].Name, emitter.Params[1].Value);
        } //Makes chase stop
        yield return new WaitForSeconds(1f);
        if (emitter.Params[2].Value != 1f)
        {
            emitter.Params[2].Value = 1f;
            emitter.Target.SetParameter(emitter.Params[2].Name, emitter.Params[2].Value);
        } // Makes not dead
        hasChased = false;
        attemptingDie = false;
    }
    #endregion

    #region TreeEvents
    private void OnEnable()
    {
        TreeMalarkey.RegisterEventOnTree(CrossyController.crossyTree, "DespawnAudio", TheDespawnThing);
        TreeMalarkey.RegisterEventOnTree(CrossyController.crossyTree, "PursuitAudio", PursuitAudio);
        TreeMalarkey.RegisterEventOnTree(CrossyController.crossyTree, "DeadAudio", DeadAudio);
        TreeMalarkey.RegisterEventOnTree(CrossyController.crossyTree, "SafeAudio", SafeAudio);
    }

    public void ObserverRegister()
    {
        
        TreeMalarkey.RegisterEventOnTree(ObserverTree, "PursuitAudio", PursuitAudio);
        TreeMalarkey.RegisterEventOnTree(ObserverTree, "DeadAudio", DeadAudio);
        TreeMalarkey.RegisterEventOnTree(ObserverTree, "SafeAudio", SafeAudio);
    }

    public void PursuitAudio()
    {
        Debug.Log("EVENT FROM TREE: Pursuit");
        hasChased = true;
        if (emitter.Params[1].Value != 0f)
        {
            emitter.Params[1].Value = 0f;
            emitter.Target.SetParameter(emitter.Params[1].Name, emitter.Params[1].Value);
            
        }
    }

    public void DeadAudio()
    {
        Debug.Log("EVENT FROM TREE: Dead");
        StartCoroutine(DeadSounds());
    }

    public void SafeAudio()
    {
        Debug.Log("EVENT FROM TREE: Safe");
        StartCoroutine(WaitForPuzzleOff());
    }

    public void TheDespawnThing()
    {
        Debug.Log("EVENT FROM TREE: Despawn");
        StartCoroutine(WaitForPuzzleOff());
    }

    private void OnDisable()
    {
        TreeMalarkey.UnregisterEventOnTree(CrossyController.crossyTree, "DespawnAudio", TheDespawnThing);
        TreeMalarkey.UnregisterEventOnTree(CrossyController.crossyTree, "PursuitAudio", PursuitAudio);
        TreeMalarkey.UnregisterEventOnTree(CrossyController.crossyTree, "DeadAudio", DeadAudio);
        TreeMalarkey.UnregisterEventOnTree(CrossyController.crossyTree, "SafeAudio", SafeAudio);
        TreeMalarkey.UnregisterEventOnTree(ObserverTree, "PursuitAudio", PursuitAudio);
        TreeMalarkey.UnregisterEventOnTree(ObserverTree, "DeadAudio", DeadAudio);
        TreeMalarkey.UnregisterEventOnTree(ObserverTree, "SafeAudio", SafeAudio);
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

    void TitanUppyDownyNoisies()
    {
        if(titan.animator.GetCurrentAnimatorStateInfo(0).IsName("TitanCrossyIdleHidden") || titan.animator.GetCurrentAnimatorStateInfo(0).IsName("TitanCrossyIdle"))
        {
            if(m_TitanNoisyNum != 0)
            {
                Debug.Log("UPPYDOWNY: Doing Neutral");
                StartCoroutine(TitanSoundNeutral());
            }
        }
        else if (titan.animator.GetCurrentAnimatorStateInfo(0).IsName("TitanCrossyRising"))
        {
            if(m_TitanNoisyNum != 1)
            {
                Debug.Log("UPPYDOWNY: Doing Uppy");
                StopCoroutine(TitanSoundNeutral());
                m_TitanNoisyNum = 1;
                emitter.Params[3].Value = m_TitanNoisyNum;
                emitter.Target.SetParameter(emitter.Params[3].Name, emitter.Params[3].Value);
            }
        }
        else if (titan.animator.GetCurrentAnimatorStateInfo(0).IsName("TitanCrossyHide"))
        {
            if (m_TitanNoisyNum != 2)
            {
                Debug.Log("UPPYDOWNY: Doing Downy");
                StopCoroutine(TitanSoundNeutral());
                m_TitanNoisyNum = 2;
                emitter.Params[3].Value = m_TitanNoisyNum;
                emitter.Target.SetParameter(emitter.Params[3].Name, emitter.Params[3].Value);
            }
        }
    }

    #endregion
}
