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
    GameObject[] objectsToSwitchOn;

    [SerializeField]
    TextMeshProUGUI conLetter;

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

    IEnumerator WaitForCrossy(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        journalController.EnableJournal();
        journalController.OpenHowTo();
        journalController.readingHowTo = true;

        journalOnSwitch.OpenOrClose();
    }
}
