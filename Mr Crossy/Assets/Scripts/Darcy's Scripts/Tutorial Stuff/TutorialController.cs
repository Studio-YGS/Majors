using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FMOD.Studio;
using FMODUnity;
using UnityEngine.Events;

public class TutorialController : MonoBehaviour
{
    JournalController journalController;

    JournalOnSwitch journalOnSwitch;

    Player_Controller playerController;

    OverseerController overseerController;

    EventInstance eventInstance;

    public GameObject[] objectsToSwitchOn;

    public UnityEvent showTabs;

    [SerializeField]
    TextMeshProUGUI conLetter;

    bool directed, doneTalk, spotted, fiveMinute, canPlayFive = true;

    void Start()
    {
        journalController = FindObjectOfType<JournalController>();
        playerController = FindObjectOfType<Player_Controller>();
        journalOnSwitch = FindObjectOfType<JournalOnSwitch>();
        overseerController = FindObjectOfType<OverseerController>();

        StartTutorial();

        StartCoroutine(DoneTalk());
    }

    void Update()
    {
        if(overseerController != null)
        {
            if (overseerController.State == 3 && !spotted)
            {
                spotted = true;
                GetComponent<TutorialSectionStart>().sectionStart.Invoke();
            }
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

    public void StartFiveMinuteTimer()
    {
        StartCoroutine(FiveMinuteTimer());
    }

    public void SetCanPlayFiveBool(bool set)
    {
        canPlayFive = set;
    }

    IEnumerator WaitForCrossy()
    {
        yield return new WaitForSeconds(5f);

        eventInstance = RuntimeManager.CreateInstance("event:/MR_C_Tutorial/TUT.0.6");

        eventInstance.start();

        yield return new WaitForSeconds(31.5f);

        journalController.EnableJournal();
        journalController.readingHowTo = true;

        if (!journalOnSwitch.open)
        {
            journalOnSwitch.OpenOrClose();
        }
        journalController.OpenHowTo();
        showTabs.Invoke();
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

    IEnumerator FiveMinuteTimer()
    {
        yield return new WaitForSeconds(300f);

        if (!fiveMinute && canPlayFive)
        {
            eventInstance = RuntimeManager.CreateInstance("event:/MR_C_Tutorial/TUT.0.5.1.3");

            eventInstance.start();
        }
    }
}
