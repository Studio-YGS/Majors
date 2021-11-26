using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    Player_Controller player;

    JournalController journal;

    PuzzleController puzzleController;

    [SerializeField]
    Transform respawnPosition, crossyPosition;

    Vector3 originalposition;

    [SerializeField]
    GameObject respawningText;

    bool hasMoved = false;
    OverseerController seer;
    public ExternalInteralSwitch exSwitch;

    void Start()
    {
        originalposition = crossyPosition.position;
        seer = FindObjectOfType<OverseerController>();
        player = FindObjectOfType<Player_Controller>();
        journal = FindObjectOfType<JournalController>();
        puzzleController = FindObjectOfType<PuzzleController>();
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
        FindObjectOfType<MrCrossyDistortion>().ResetDamage();
        player.gameObject.SetActive(false);
        player.DisableController();
        player.transform.position = respawnPosition.position;

        journal.OpenMap();
        journal.DisableJournal();

        if(!puzzleController.GameOverCheck())
        {
            respawningText.SetActive(true);

            StartCoroutine(WaitForRespawn(5f));
        }
    }

    IEnumerator WaitForRespawn(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        FindObjectOfType<MrCrossyDistortion>().mask.SetActive(false);
        FindObjectOfType<MrCrossyDistortion>().ReduceInsanity();
        FindObjectOfType<MrCrossyDistortion>().DecreaseVignette();
        FindObjectOfType<MrCrossyDistortion>().ResetDamage();
        respawningText.SetActive(false);

        player.gameObject.SetActive(true);
        player.enabled = true;
        player.EnableController();
        seer.emitter.Target.SetParameter(seer.deadParamName, 1f);
        seer.emitter.Target.SetParameter(seer.distanceParamName, 100f);
        seer.emitter.Target.SetParameter(seer.chaseParamName, 1f);
        exSwitch.WalkIn();
        journal.EnableJournal();
        FindObjectOfType<OverseerController>().deady = false;
    }
}
