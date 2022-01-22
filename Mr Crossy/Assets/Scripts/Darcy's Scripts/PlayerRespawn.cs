using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class PlayerRespawn : MonoBehaviour
{
    Player_Controller player;

    JournalController journal;

    PuzzleController puzzleController;

    [SerializeField]
    Transform crossyPosition;
    Transform respawnPosition;
    [SerializeField]
    Transform[] respawnPoints;

    Vector3 originalposition;

    [SerializeField]
    TextMeshProUGUI deathCountText;

    [SerializeField]
    GameObject street;

    [SerializeField]
    GameObject deathVideoObject;
    [SerializeField] float deathWaitTime = 7.3f;

    public UnityEvent deathTutorial;

    int deathCount;

    bool hasMoved = false, tutorialPlayed;
    OverseerController seer;
    public ExternalInteralSwitch exSwitch;
    [HideInInspector] public bool crossyDeath;

    void Start()
    {
        originalposition = crossyPosition.position;
        seer = FindObjectOfType<OverseerController>();
        player = FindObjectOfType<Player_Controller>();
        journal = FindObjectOfType<JournalController>();
        respawnPosition = respawnPoints[0];
    }

    public void Register()
    {
        TreeMalarkey.RegisterEventOnTree(CrossyController.crossyTree, "DeadNoises", PlayerDie);
    }

    public void OnDisable()
    {
        TreeMalarkey.UnregisterEventOnTree(CrossyController.crossyTree, "DeadNoises", PlayerDie);
    }

    void Update()
    {
        if(crossyPosition.position != originalposition && !hasMoved)
        {
            Register();
            hasMoved = true;
        }
    }

    public void PlayerDie()
    {
        FindObjectOfType<CrossKeyManager>().doorsLocked = false;
        FindObjectOfType<ObjectHolder>().DeathDrop();
        seer.deady = true;
        if (!seer.attemptingDie) seer.DeadNoises();
        FindObjectOfType<MrCrossyDistortion>().ResetDamage();
        player.cam.gameObject.SetActive(false);
        player.DisableController();
        player.transform.position = respawnPosition.position;

        journal.OpenMap();
        journal.DisableJournal();

        deathCount++;
        deathCountText.text = deathCount.ToString();
        if (!crossyDeath)
        {
            deathVideoObject.SetActive(true);

            StartCoroutine(WaitForRespawn(deathWaitTime));
        }
        else
        {
            StartCoroutine(WaitForRespawn(2));
        }
        
    }

    public void ReleaseToPlayer()
    {
        player.EnableController();

        if (seer.emitter.Params[2].Value != 1f)
        {
            seer.emitter.Params[2].Value = 1f;
            seer.emitter.Target.SetParameter(seer.emitter.Params[1].Name, seer.emitter.Params[1].Value);
        }
        //seer.emitter.Params[2].Value = 1f;
        if (seer.emitter.Params[0].Value != 100f)
        {
            seer.emitter.Params[0].Value = 100f;
            seer.emitter.Target.SetParameter(seer.emitter.Params[0].Name, seer.emitter.Params[0].Value);
        }
        //seer.emitter.Params[0].Value = 100f;
        if (seer.emitter.Params[1].Value != 1f)
        {
            seer.emitter.Params[1].Value = 1f;
            seer.emitter.Target.SetParameter(seer.emitter.Params[1].Name, seer.emitter.Params[1].Value);
        }
        //seer.emitter.Params[1].Value = 1f;

        exSwitch.WalkIn();
        journal.EnableJournal();
        FindObjectOfType<OverseerController>().deady = false;
    }

    public void SwitchRespawnPoint(int whichPoint)
    {
        respawnPosition = respawnPoints[whichPoint - 1];
    }

    IEnumerator WaitForRespawn(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        FindObjectOfType<MrCrossyDistortion>().ReduceInsanity();
        FindObjectOfType<MrCrossyDistortion>().DecreaseVignette();
        FindObjectOfType<MrCrossyDistortion>().colorAdjustments[0].colorFilter.value = Color.white;
        FindObjectOfType<MrCrossyDistortion>().colorAdjustments[1].colorFilter.value = Color.white;
        deathVideoObject.SetActive(false);

        puzzleController = FindObjectOfType<PuzzleController>();
        puzzleController.MistakeCounter();
        crossyDeath = false;
        player.cam.gameObject.SetActive(true);
        //player.enabled = true;
        player.ResetHealth();

        street.GetComponent<TextMeshProUGUI>().text = "Home.";
        FindObjectOfType<MrCrossyDistortion>().mask.GetComponent<Animator>().SetTrigger("FadeAway");
        if (!tutorialPlayed)
        {
            tutorialPlayed = true;

            deathTutorial.Invoke();
        }
        else
        {
            ReleaseToPlayer();
        }
    }
}
