using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    JournalController journalController;

    JournalOnSwitch journalOnSwitch;

    Player_Controller playerController;

    [SerializeField]
    GameObject controlsUI, spaceToContinue;

    bool canContinue = false;

    void Start()
    {
        journalController = FindObjectOfType<JournalController>();
        playerController = FindObjectOfType<Player_Controller>();
        journalOnSwitch = FindObjectOfType<JournalOnSwitch>();

        ShowControls();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && canContinue)
        {
            canContinue = false;
            spaceToContinue.SetActive(false);

            StartTutorial();
        }
    }

    void ShowControls()
    {
        controlsUI.SetActive(true);

        playerController.DisableController();
        playerController.inJournal = false;

        journalController.DisableJournal();

        StartCoroutine(ReadingControls());
    }

    void StartTutorial()
    {
        controlsUI.SetActive(false);

        playerController.EnableController();

        journalController.EnableJournal();

        journalOnSwitch.journalClosed.SetActive(true);
    }

    IEnumerator ReadingControls()
    {
        yield return new WaitForSeconds(5f);

        canContinue = true;
        spaceToContinue.SetActive(true);
        StopCoroutine(ReadingControls());
    }
}
