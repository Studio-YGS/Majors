using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

public class OverseerController : MonoBehaviour
{
    public static BehaviorTree ObserverTree;
    [HideInInspector] public static bool m_PlayerInHouse;

    CrossyTheWatcher titan;
    MrCrossyDistortion distootle;

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
    [SerializeField] private List<GameObject> m_SpawnLighthouses = new List<GameObject>();

    [Header("Titan Crossy")]
    [SerializeField] private float m_CheckRadius;
    private int m_State;
    private bool m_HideTitan;

    [Header("Voice Variables")]
    [SerializeField] private float m_TitanVoiceTimeMin;
    [SerializeField] private float m_TitanVoiceTimeMax;
    private float m_TitanVoiceTime = 0;
    [SerializeField] private float m_CrossyVoiceTimeMin;
    [SerializeField] private float m_CrossyVoiceTimeMax;
    private float m_CrossyVoiceTime = 0;

    private bool m_TitanLineTiming;
    private bool m_CrossyLineTiming;
    private bool m_PursuitLineTiming;

    private bool m_IsTutorial = true;
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
    public bool HideTitan { get { return m_HideTitan; } set { m_HideTitan = value; } }

    public List<GameObject> SpawnLightHouses { get { return m_SpawnLighthouses; } }

    public bool IsTutorial { get { return m_IsTutorial; } set { m_IsTutorial = value; } }

    public bool IsInHouse { get { return m_PlayerInHouse; } }

    #endregion

    [Space(10)]
    public List<Lighthouse> titanLighthouses = new List<Lighthouse>();


    public Lighthouse storedHouse;
    public float currDist;
    private float storedDist = 0;
    bool vignetteActivated = false;

    private void Awake()
    {
        m_Observer = gameObject;
        ObserverTree = gameObject.GetComponent<BehaviorTree>();
        distootle = FindObjectOfType<MrCrossyDistortion>();
        if (startOnAwake)
        {
            TreeMalarkey.EnableTree(ObserverTree);
        }
        else TreeMalarkey.DisableTree(ObserverTree);
        titan = m_TitanCrossy.GetComponent<CrossyTheWatcher>();

        m_TitanVoiceTime = Random.Range(m_TitanVoiceTimeMin, m_TitanVoiceTimeMax);
        m_CrossyVoiceTime = Random.Range(m_CrossyVoiceTimeMin, m_CrossyVoiceTimeMax);
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
            if(!FindObjectOfType<CrossKeyManager>().doorsLocked)
            {
                FindObjectOfType<CrossKeyManager>().doorsLocked = true;
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
            if (m_State == -1)
            {
                TitanVoiceLineTimer();
            }
            else if (m_State == 1)
            {
                CrossyPatrolVoiceLineTimer();
            }
            else if (m_State == 2)
            {
                CrossyAlertVoiceLineTimer();
            }
        }
        
    }

    #region Methods
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
    #endregion


    #region VoiceStuff

    //Titan Voice Timer - Time Is Randomised
    public void TitanVoiceLineTimer()
    {
        if(!m_TitanLineTiming) StartCoroutine(TitanVoiceTimer());
    }

    IEnumerator TitanVoiceTimer()
    {
        if (m_CrossyLineTiming) StopCoroutine(CrossyPatrolVoiceTimer());

        m_TitanLineTiming = true;

        yield return new WaitForSeconds(m_TitanVoiceTime);

        Debug.Log("LinePlay - Titan Crossy");
        //Do the Titan Crossy VoiceLine

        //Place Something to wait till line ended

        m_TitanVoiceTime = Random.Range(m_TitanVoiceTimeMin, m_TitanVoiceTimeMax);

        m_TitanLineTiming = false;
    }

    //Crossy Patrol Voice Timer - Time Is Randomised
    public void CrossyPatrolVoiceLineTimer()
    {
        if (!m_CrossyLineTiming) StartCoroutine(CrossyPatrolVoiceTimer());
    }

    IEnumerator CrossyPatrolVoiceTimer()
    {
        if (m_TitanLineTiming) StopCoroutine(TitanVoiceTimer());

        m_CrossyLineTiming = true;

        yield return new WaitForSeconds(m_CrossyVoiceTime);

        Debug.Log("LinePlay - Crossy Patrol");
        //Do the Regular Crossy VoiceLine

        //Place Something to wait till line ended

        m_CrossyVoiceTime = Random.Range(m_CrossyVoiceTimeMin, m_CrossyVoiceTimeMax);

        m_CrossyLineTiming = false;
    }

    //Crossy Alert Voice Timer - Time Is Randomised
    public void CrossyAlertVoiceLineTimer()
    {
        if (!m_CrossyLineTiming) StartCoroutine(CrossyAlertVoiceTimer());
    }

    IEnumerator CrossyAlertVoiceTimer()
    {
        if (m_TitanLineTiming) StopCoroutine(TitanVoiceTimer());

        m_CrossyLineTiming = true;

        yield return new WaitForSeconds(m_CrossyVoiceTime);

        Debug.Log("LinePlay - Crossy Alert");
        //Do the Regular Crossy VoiceLine

        //Place Something to wait till line ended

        m_CrossyVoiceTime = Random.Range(m_CrossyVoiceTimeMin, m_CrossyVoiceTimeMax);

        m_CrossyLineTiming = false;
    }

    //Crossy Pursuit Voice Timer - Time Is Randomised
    public void CrossyPursuitVoiceLineTimer()
    {
        if (!m_PursuitLineTiming) StartCoroutine(CrossyPursuitVoiceTimer());
    }

    IEnumerator CrossyPursuitVoiceTimer()
    {
        if (m_CrossyLineTiming) StopCoroutine(TitanVoiceTimer());

        m_PursuitLineTiming = true;

        Debug.Log("LinePlay - Crossy Alert");
        //Do the Crossy Pursuit VoiceLine

        //Replace with something to wait till line ended
        yield return new WaitForSeconds(m_CrossyVoiceTime);

        m_PursuitLineTiming = false;
    }
    #endregion
}
