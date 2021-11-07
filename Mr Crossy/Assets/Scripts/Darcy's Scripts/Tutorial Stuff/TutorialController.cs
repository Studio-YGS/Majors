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

    EventInstance eventInstance;

    [SerializeField]
    GameObject[] objectsToSwitchOn;

    [SerializeField]
    TextMeshProUGUI conLetter;

    bool directed = false;

    void Start()
    {
        journalController = FindObjectOfType<JournalController>();
        playerController = FindObjectOfType<Player_Controller>();
        journalOnSwitch = FindObjectOfType<JournalOnSwitch>();
        journalTimer = FindObjectOfType<JournalTimer>();

        journalTimer.canCount = false;

        StartTutorial();
    }

    void StartTutorial()
    {
        playerController.EnableController();

        journalController.EnableJournal();
        journalController.OpenMap();

        journalOnSwitch.journalClosed.SetActive(true);

        //eventInstance = RuntimeManager.CreateInstance(""); //tutorial 0.1 line here

        //eventInstance.start();

        for(int i = 0; i < objectsToSwitchOn.Length; i++)
        {
            objectsToSwitchOn[i].SetActive(true);
        }
    }

    public void DirectToNote()
    {
        if (!directed)
        {
            //eventInstance = RuntimeManager.CreateInstance(""); //tutorial 0.2 line here

            //eventInstance.start();

            directed = true;
        }

        StartCoroutine(DirectionTimer());
    }

    public void ChangeConLetter(string letter)
    {
        conLetter.text = "[" + letter + "]";
    }

    public void CrossyWait(float waitTime)
    {
        StartCoroutine(WaitForCrossy(waitTime));
    }

    IEnumerator WaitForCrossy(float waitTime)
    {
        //eventInstance = RuntimeManager.CreateInstance(""); //tutorial line 0.6.1 here

        //eventInstance.start();

        yield return new WaitForSeconds(waitTime);

        journalController.EnableJournal();
        journalController.OpenHowTo();
        journalController.readingHowTo = true;

        journalOnSwitch.OpenOrClose();
    }

    IEnumerator DirectionTimer()
    {
        yield return new WaitForSeconds(15f);

        directed = false;

        StopCoroutine(DirectionTimer());
    }
}
