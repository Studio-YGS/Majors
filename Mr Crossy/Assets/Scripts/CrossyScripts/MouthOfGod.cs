using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gemstone;
using BehaviorDesigner.Runtime;
using FMOD.Studio;
using FMODUnity;

public class MouthOfGod : MonoBehaviour
{
    [SerializeField]
    private BehaviorTree mouthOfGod;
    public static BehaviorTree MouthOfGodTree;

    #region AudioVariables
    [Header("Main Audio Variables")]
    [SerializeField]
    private OverseerController overseer;

    public EmitterRef emitter;
    EventInstance eventInstance;

    [Header("Titan Audio Variables")]
    [Range(0f, 1f)] [SerializeField]
    private float m_VoiceLineProbability = 0.2f;

    private int m_TitanAmbientNum = 0;

    bool m_TitanVoiceAttempted = false;
    #endregion

    #region UnityMethods
    private void Awake()
    {
        MouthOfGodTree = mouthOfGod;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!overseer.m_IsTutorial)
        {
            CrossyAudioDistance();

            if(!overseer.m_PlayerInSafeHouse)
            {
                TitanCrossyVoiceLines();
            }

            TitanAmbientAudio();

        }
    }
    #endregion

    #region AudioMethods

    void CrossyAudioDistance()
    {
        if(overseer.State != -1)
        {
            if(OverseerController.CrossyPathDistance <= 100f)
            {
                ParameterSet(0, OverseerController.CrossyPathDistance);
            }
            else ParameterSet(0, 100f);
        }
    }

    public void ChaseAudio()
    {
        if(emitter.Params[1].Value != 0f)
        {
            ParameterSet(1, 0f);
        }
    }

    public void SafeAudio()
    {
        StartCoroutine(SafeDelay());
    }

    public void DeathAudio()
    {
        StartCoroutine(DeathDelay());
    }

    public void ResetParameters()
    {
        if(emitter.Params[0].Value != 100f)
        {
            ParameterSet(0, 100f);
        }
        if (emitter.Params[1].Value != 1f)
        {
            ParameterSet(1, 1f);
        }
        if (emitter.Params[2].Value != 1f)
        {
            ParameterSet(2, 1f);
        }

    }

    void TitanCrossyVoiceLines()
    {
        if(overseer.titan.animator.GetCurrentAnimatorStateInfo(0).IsName("TitanCrossyIdle") && !m_TitanVoiceAttempted)
        {
            float chance = Random.Range(0f, 1f);

            if(chance < m_VoiceLineProbability)
            {
                eventInstance = RuntimeManager.CreateInstance("event:/MR_C_Titan/Titan_Oneliners");

                eventInstance.start();
            }

            m_TitanVoiceAttempted = true;
        }
        else if(overseer.titan.animator.GetCurrentAnimatorStateInfo(0).IsName("TitanCrossyIdleHidden"))
        {
            eventInstance.release();
            m_TitanVoiceAttempted = false;
        }
    }

    void TitanAmbientAudio()
    {
        if(overseer.titan.animator.GetCurrentAnimatorStateInfo(0).IsName("TitanCrossyIdleHidden") || overseer.titan.animator.GetCurrentAnimatorStateInfo(0).IsName("TitanCrossyIdle"))
        {
            if (m_TitanAmbientNum != 0)
            {
                StartCoroutine(TitanAmbientReset());
            }
        }
        else if (overseer.titan.animator.GetCurrentAnimatorStateInfo(0).IsName("TitanCrossyRising"))
        {
            if (m_TitanAmbientNum != 1)
            {
                StopCoroutine(TitanAmbientReset());
                m_TitanAmbientNum = 1;
                ParameterSet(3, m_TitanAmbientNum);
            }
        }
        else if (overseer.titan.animator.GetCurrentAnimatorStateInfo(0).IsName("TitanCrossyHide"))
        {
            if (m_TitanAmbientNum != 2)
            {
                StopCoroutine(TitanAmbientReset());
                m_TitanAmbientNum = 2;
                ParameterSet(3, m_TitanAmbientNum);
            }
        }
    }

    IEnumerator SafeDelay()
    {
        if (emitter.Params[1].Value != 1f)
        {
            ParameterSet(1, 1f);
        }
        if (emitter.Params[2].Value != 2f)
        {
            ParameterSet(2, 2f);
        }
        yield return new WaitForSeconds(1f);
        
        if (emitter.Params[2].Value != 1f)
        {
            ParameterSet(2, 1f);
        }
    }

    IEnumerator DeathDelay()
    {
        if (emitter.Params[2].Value != 0f)
        {
            ParameterSet(2, 0f);
        }
        if (emitter.Params[1].Value != 1f)
        {
            ParameterSet(1, 1f);
        }
        yield return new WaitForSeconds(1f);

        if (emitter.Params[2].Value != 1f)
        {
            ParameterSet(2, 1f);
        }
    }

    IEnumerator TitanAmbientReset()
    {
        m_TitanAmbientNum = 0;

        yield return new WaitForSeconds(1f);

        ParameterSet(3, m_TitanAmbientNum);
    }

    void ParameterSet(int index, float value)
    {
        emitter.Params[index].Value = value;
        emitter.Target.SetParameter(emitter.Params[index].Name, emitter.Params[index].Value);
    }
    #endregion

    #region EventRegister

    void OnEnable()
    {
        if(MouthOfGodTree)
        {
            TreeMalarkey.RegisterEventOnTree(MouthOfGodTree, "CallChaseAudio", ChaseAudio);
            TreeMalarkey.RegisterEventOnTree(MouthOfGodTree, "CallSafeAudio", SafeAudio);
            TreeMalarkey.RegisterEventOnTree(MouthOfGodTree, "CallDeathAudio", DeathAudio);
            TreeMalarkey.RegisterEventOnTree(MouthOfGodTree, "CallResetAudio", ResetParameters);
        }
    }

    void OnDisable()
    {
        if (MouthOfGodTree)
        {
            TreeMalarkey.UnregisterEventOnTree(MouthOfGodTree, "CallChaseAudio", ChaseAudio);
            TreeMalarkey.UnregisterEventOnTree(MouthOfGodTree, "CallSafeAudio", SafeAudio);
            TreeMalarkey.UnregisterEventOnTree(MouthOfGodTree, "CallDeathAudio", DeathAudio);
            TreeMalarkey.UnregisterEventOnTree(MouthOfGodTree, "CallResetAudio", ResetParameters);
        }
    }

    #endregion
}
