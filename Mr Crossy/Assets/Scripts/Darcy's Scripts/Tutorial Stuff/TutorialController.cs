using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    JournalController journalController;

    JournalOnSwitch journalOnSwitch;

    JournalTimer journalTimer;

    Player_Controller playerController;

    [SerializeField]
    GameObject controlsUI, spaceToContinue;

    ShowPromptUI showPrompt;

    bool canContinue = false;

    void Start()
    {
        journalController = FindObjectOfType<JournalController>();
        playerController = FindObjectOfType<Player_Controller>();
        journalOnSwitch = FindObjectOfType<JournalOnSwitch>();
        showPrompt = FindObjectOfType<ShowPromptUI>();
        journalTimer = FindObjectOfType<JournalTimer>();

        ShowControls();

        journalTimer.canCount = false;
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

    public void LockedGate()
    {
        showPrompt.generalPrompt.text = "The gate appears to be locked. What's that piece of paper though?";
        showPrompt.generalPrompt.gameObject.SetActive(true);
        showPrompt.canShow = false;

        StartCoroutine(PlayerNeedsToWait());
    }

    public void CantOpenDoorYet()
    {
        showPrompt.generalPrompt.text = "There's something alluring about that gate.";
        showPrompt.generalPrompt.gameObject.SetActive(true);
        showPrompt.canShow = false;
    }

    IEnumerator ReadingControls()
    {
        yield return new WaitForSeconds(5f);

        canContinue = true;
        spaceToContinue.SetActive(true);
        StopCoroutine(ReadingControls());
    }

    IEnumerator PlayerNeedsToWait()
    {
        playerController.DisableController();
        playerController.inJournal = false;

        journalController.DisableJournal();

        yield return new WaitForSeconds(5f);

        playerController.EnableController();

        journalController.EnableJournal();

        showPrompt.generalPrompt.gameObject.SetActive(false);
        showPrompt.canShow = true;

        StopCoroutine(PlayerNeedsToWait());
    }
}
