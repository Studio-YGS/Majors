using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    Player_Controller player;

    JournalController journal;

    AudioEvents audio;

    [SerializeField]
    Transform respawnPosition, crossyPosition;

    Vector3 originalposition;

    [SerializeField]
    GameObject respawningText;

    bool hasMoved = false;
    OverseerController seer;

    void Start()
    {
        Debug.Log("starting");
        originalposition = crossyPosition.position;
        Debug.Log("set position" + CrossyController.crossyTree.gameObject.name);
        seer = FindObjectOfType<OverseerController>();
    }

    public void Register()
    {
        TreeMalarkey.RegisterEventOnTree(CrossyController.crossyTree, "DeadNoises", PlayerDie);
        Debug.Log("Registered event");
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
            Debug.Log("position is different");
        }
    }

    public void PlayerDie()
    {
        FindObjectOfType<CrossKeyManager>().doorsLocked = false;
        player = FindObjectOfType<Player_Controller>();
        journal = FindObjectOfType<JournalController>();
        audio = FindObjectOfType<AudioEvents>();

        audio.PlayerDie();
        FindObjectOfType<OverseerController>().deady = true;

        player.gameObject.SetActive(false);
        player.DisableController();
        player.transform.position = respawnPosition.position;

        journal.OpenMap();
        journal.DisableJournal();

        respawningText.SetActive(true);

        StartCoroutine(WaitForRespawn(5f));
    }

    IEnumerator WaitForRespawn(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        FindObjectOfType<MrCrossyDistortion>().mask.SetActive(false);
        respawningText.SetActive(false);

        player.gameObject.SetActive(true);
        player.enabled = true;
        player.EnableController();
        seer.emitter.Target.SetParameter(seer.deadParamName, 1f);
        seer.emitter.Target.SetParameter(seer.distanceParamName, 100f);
        seer.emitter.Target.SetParameter(seer.chaseParamName, 1f);
        journal.EnableJournal();
        FindObjectOfType<OverseerController>().deady = false;
    }
}
