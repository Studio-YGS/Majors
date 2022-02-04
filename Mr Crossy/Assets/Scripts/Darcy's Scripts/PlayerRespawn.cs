using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class PlayerRespawn : MonoBehaviour //this script handles the respawning after the player dies to mr crossy, or during a crosskey puzzle
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
    public ExternalInteralSwitch exSwitch;
    [HideInInspector] public bool crossyDeath;

    void Start()
    {
        originalposition = crossyPosition.position;
        player = FindObjectOfType<Player_Controller>();
        journal = FindObjectOfType<JournalController>();
        respawnPosition = respawnPoints[0];
    }

    //public void Register()
    //{
    //    TreeMalarkey.RegisterEventOnTree(CrossyController.crossyTree, "DeadNoises", PlayerDie);
    //}

    //public void OnDisable()
    //{
    //    TreeMalarkey.UnregisterEventOnTree(CrossyController.crossyTree, "DeadNoises", PlayerDie);
    //}

    void Update()
    {
        //if(crossyPosition.position != originalposition && !hasMoved)
        //{
        //    Register();
        //    hasMoved = true;
        //}
    }

    public void PlayerDie() //this method gets called when the player dies from anything
    {
        FindObjectOfType<CrossKeyManager>().doorsLocked = false;
        FindObjectOfType<ObjectHolder>().DeathDrop();
        //seer.deady = true;
        //if (!seer.attemptingDie) seer.DeadNoises();
        FindObjectOfType<MrCrossyDistortion>().ResetDamage();

        player.cam.gameObject.SetActive(false); //turns off the player and disables it
        player.DisableController();
        player.transform.position = respawnPosition.position;

        journal.OpenMap(); //disables the journal too
        journal.DisableJournal();

        deathCount++; //counts a death up
        deathCountText.text = deathCount.ToString();
        if (!crossyDeath) //if the player died from mr crossy, it plays a video and has a different respawn time
        {
            deathVideoObject.SetActive(true);

            StartCoroutine(WaitForRespawn(deathWaitTime));
        }
        else //otherwise, 2 seconds respawn time
        {
            StartCoroutine(WaitForRespawn(2));
        }
        
    }

    public void ReleaseToPlayer() //this method releases control back to the player
    {
        player.EnableController();

        //if (seer.emitter.Params[2].Value != 1f)
        //{
        //    seer.emitter.Params[2].Value = 1f;
        //    seer.emitter.Target.SetParameter(seer.emitter.Params[1].Name, seer.emitter.Params[1].Value);
        //}
        ////seer.emitter.Params[2].Value = 1f;
        //if (seer.emitter.Params[0].Value != 100f)
        //{
        //    seer.emitter.Params[0].Value = 100f;
        //    seer.emitter.Target.SetParameter(seer.emitter.Params[0].Name, seer.emitter.Params[0].Value);
        //}
        ////seer.emitter.Params[0].Value = 100f;
        //if (seer.emitter.Params[1].Value != 1f)
        //{
        //    seer.emitter.Params[1].Value = 1f;
        //    seer.emitter.Target.SetParameter(seer.emitter.Params[1].Name, seer.emitter.Params[1].Value);
        //}
        ////seer.emitter.Params[1].Value = 1f;

        exSwitch.WalkIn();
        journal.EnableJournal();
        //FindObjectOfType<OverseerController>().deady = false;
    }

    public void SwitchRespawnPoint(int whichPoint) //this method is called whenever the player enters a new district, so that the proper respawn point can be set
    {
        respawnPosition = respawnPoints[whichPoint - 1];
    }

    IEnumerator WaitForRespawn(float waitTime) //after this co-routine finishes counting, every thing is reset back to normal as if the game just started, however the player is respawned back in the house, and their mistake counter is counted up
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
