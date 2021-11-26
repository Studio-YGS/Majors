using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FMOD.Studio;
using FMODUnity;

public class TutorialController : MonoBehaviour
{
    JournalController journalController;

    JournalOnSwitch journalOnSwitch;

    JournalTimer journalTimer;

    Player_Controller playerController;

    OverseerController overseerController;

    EventInstance eventInstance;

    public GameObject[] objectsToSwitchOn;

    [SerializeField]
    TextMeshProUGUI conLetter;

    bool directed = false, doneTalk = false, spotted = false;

    void Start()
    {
        journalController = FindObjectOfType<JournalController>();
        playerController = FindObjectOfType<Player_Controller>();
        journalOnSwitch = FindObjectOfType<JournalOnSwitch>();
        journalTimer = FindObjectOfType<JournalTimer>();
        overseerController = FindObjectOfType<OverseerController>();

        StartTutorial();

        StartCoroutine(DoneTalk());
    }

    void Update()
    {
        if (overseerController.State == 3 && !spotted)
        {
            spotted = true;
            GetComponent<TutorialSectionStart>().sectionStart.Invoke();
        }
    }

    void StartTutorial()
    {
        playerController.EnableController();

        journalController.EnableJournal();
        journalController.OpenMap();
        journalController.DisableJournal();

        eventInstance = RuntimeManager.CreateInstance("event:/MR_C_Tutorial/TUT.0.1");

        eventInstance.start();

        for (int i = 0; i < objectsToSwitchOn.Length; i++)
        {
            objectsToSwitchOn[i].SetActive(true);
        }
    }

    public void DirectToNote()
    {
        if (!directed && doneTalk)
        {
            eventInstance = RuntimeManager.CreateInstance("event:/MR_C_Tutorial/TUT.0.2");

            eventInstance.start();

            directed = true;

            StartCoroutine(DirectionTimer());
        }
    }

    public void ChangeConLetter(string letter)
    {
        conLetter.text = "[" + letter + "]";
    }

    public void StreetNameBlank()
    {
        PuzzleController puzzleController = FindObjectOfType<PuzzleController>();

        puzzleController.streetText.text = "";
    }

    public void CrossyWait()
    {
        StartCoroutine(WaitForCrossy());
    }

    IEnumerator WaitForCrossy()
    {
        yield return new WaitForSeconds(5f);

        eventInstance = RuntimeManager.CreateInstance("event:/MR_C_Tutorial/TUT.0.6");

        eventInstance.start();

        yield return new WaitForSeconds(60f);

        journalController.EnableJournal();
        journalController.readingHowTo = true;

        journalOnSwitch.OpenOrClose();
        journalController.OpenHowTo();
    }
    
    IEnumerator DoneTalk()
    {
        yield return new WaitForSeconds(11f);

        doneTalk = true;

        StopCoroutine(DoneTalk());
    }

    IEnumerator DirectionTimer()
    {
        yield return new WaitForSeconds(15f);

        directed = false;

        StopCoroutine(DirectionTimer());
    }
}
