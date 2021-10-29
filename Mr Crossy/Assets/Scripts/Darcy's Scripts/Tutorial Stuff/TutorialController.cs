using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialController : MonoBehaviour
{
    JournalController journalController;

    JournalOnSwitch journalOnSwitch;

    JournalTimer journalTimer;

    Player_Controller playerController;

    [SerializeField]
    GameObject controlsUI, spaceToContinue;

    [SerializeField]
    GameObject[] objectsToSwitchOn;

    [SerializeField]
    TextMeshProUGUI conLetter;

    bool canContinue = false;

    void Start()
    {
        journalController = FindObjectOfType<JournalController>();
        playerController = FindObjectOfType<Player_Controller>();
        journalOnSwitch = FindObjectOfType<JournalOnSwitch>();
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

        for(int i = 0; i < objectsToSwitchOn.Length; i++)
        {
            objectsToSwitchOn[i].SetActive(true);
        }
    }

    public void ChangeConLetter(string letter)
    {
        conLetter.text = "[" + letter + "]";
    }

    public void CrossyWait(float waitTime)
    {
        StartCoroutine(WaitForCrossy(waitTime));
    }

    IEnumerator ReadingControls()
    {
        yield return new WaitForSeconds(5f);

        canContinue = true;
        spaceToContinue.SetActive(true);
        StopCoroutine(ReadingControls());
    }

    IEnumerator PlayerNeedsToWait(float waitTime)
    {
        playerController.DisableController();
        playerController.inJournal = false;

        journalController.DisableJournal();

        yield return new WaitForSeconds(waitTime);

        playerController.EnableController();

        journalController.EnableJournal();

        StopCoroutine(PlayerNeedsToWait(5f));
    }

    IEnumerator WaitForCrossy(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        journalController.OpenHowTo();
        journalController.readingHowTo = true;

        journalOnSwitch.OpenOrClose();
    }
}
