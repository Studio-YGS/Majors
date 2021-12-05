using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class JournalController : MonoBehaviour
{
    [SerializeField]
    GameObject[] notePages, logPages;

    JournalOnSwitch journalOnSwitch;

    [HideInInspector]
    public List<int> noteList = new List<int>(0); 

    [SerializeField]
    GameObject log, tutMap, gameMap, notes, howTo, rightArrow, leftArrow;

    GameObject mapPage;

    int whichTab = 1; //,whichLogPage = 0;

    [HideInInspector]
    public int whichNotesPage = 0;

    public bool disabled = false, tutorial = true;
    [HideInInspector]
    public bool readingHowTo = false, waitForCrossy = false, logTab, notesTab;

    EventInstance eventInstance;

    void Start()
    {
        mapPage = tutMap;

        if (!tutorial)
        {
            SetToGameMap();
        }
        journalOnSwitch = FindObjectOfType<JournalOnSwitch>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D) && rightArrow.activeInHierarchy)
        {
            Arrows(true);
        }
        else if (Input.GetKeyDown(KeyCode.A) && leftArrow.activeInHierarchy)
        {
            Arrows(false);
        }

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if (!FindObjectOfType<CrossKeyManager>().puzzleOn)
            {
                if (readingHowTo)
                {
                    readingHowTo = false;

                    GetComponent<TutorialSectionStart>().ReadHowTo();

                    GetComponent<JournalTimer>().StartTimer();

                    OpenMap();
                }

                if (waitForCrossy)
                {
                    waitForCrossy = false;

                    GetComponent<TutorialSectionStart>().WaitForCrossy();

                    OpenMap();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            if(!FindObjectOfType<CrossKeyManager>().puzzleOn)
            {
                if (!journalOnSwitch.open)
                {
                    journalOnSwitch.OpenOrClose();
                }
                OpenMap();
            }
        }

        if (Input.GetKeyDown(KeyCode.N) && notesTab)
        {
            if (!FindObjectOfType<CrossKeyManager>().puzzleOn)
            {
                if (!journalOnSwitch.open)
                {
                    journalOnSwitch.OpenOrClose();
                }
                OpenNotes();
            }
        }

        if (Input.GetKeyDown(KeyCode.L) && logTab)
        {
            if (!FindObjectOfType<CrossKeyManager>().puzzleOn)
            {
                if (!journalOnSwitch.open)
                {
                    journalOnSwitch.OpenOrClose();
                }
                OpenLog();
            }
        }
    }

    public void SetToGameMap()
    {
        mapPage = gameMap;
    }

    public void OpenLog()
    {
        if (!disabled)
        {
            log.SetActive(true);
            mapPage.SetActive(false);
            notes.SetActive(false);
            howTo.SetActive(false);

            leftArrow.SetActive(false);
            rightArrow.SetActive(false);

            whichTab = 1;

            for (int i = 0; i < logPages.Length; i++)
            {
                logPages[i].SetActive(true);
            }
        }
    }

    public void OpenMap()
    {
        if (!disabled && mapPage != null)
        {
            log.SetActive(false);
            mapPage.SetActive(true);
            notes.SetActive(false);
            howTo.SetActive(false);

            rightArrow.SetActive(false);
            leftArrow.SetActive(false);

            whichTab = 2;
        }
    }

    public void OpenNotes()
    {
        if (!disabled)
        {
            if (noteList.Count >= 1)
            {
                log.SetActive(false);
                mapPage.SetActive(false);
                notes.SetActive(true);
                howTo.SetActive(false);

                leftArrow.SetActive(false);
                rightArrow.SetActive(false);

                whichTab = 3;

                for (int i = 0; i < notePages.Length; i++)
                {
                    notePages[i].SetActive(false);
                }

                notePages[whichNotesPage].SetActive(true);

                if (whichNotesPage == 0 && noteList.Count > 1)
                {
                    leftArrow.SetActive(false);
                    rightArrow.SetActive(true);
                }
                else if (whichNotesPage == noteList.Count && noteList.Count > 1 || whichNotesPage == noteList.Count - 1 && noteList.Count > 1)
                {
                    leftArrow.SetActive(true);
                    rightArrow.SetActive(false);
                }
                else if (whichNotesPage > 0)
                {
                    leftArrow.SetActive(true);
                    rightArrow.SetActive(true);
                }
            }
        }
    }

    public void OpenHowTo()
    {
        if (!disabled && !tutorial)
        {
            log.SetActive(false);
            mapPage.SetActive(false);
            notes.SetActive(false);
            howTo.SetActive(true);

            leftArrow.SetActive(false);
            rightArrow.SetActive(false);

            whichTab = 4;
        }
    }

    public void Arrows(bool right)
    {
        if (right)
        {
            switch (whichTab)
            {
                case 1:
                    {
                        //whichLogPage++;
                        OpenLog();
                        break;
                    }
                case 3:
                    {
                        whichNotesPage++;
                        OpenNotes();
                        break;
                    }
            }

            eventInstance = RuntimeManager.CreateInstance("event:/2D/Paper/Paper Turn");

            eventInstance.start();
        }
        else
        {
            switch (whichTab)
            {
                case 1:
                    {
                        //whichLogPage--;
                        OpenLog();
                        break;
                    }
                case 3:
                    {
                        whichNotesPage--;
                        OpenNotes();
                        break;
                    }
            }

            eventInstance = RuntimeManager.CreateInstance("event:/2D/Paper/Paper Turn");

            eventInstance.start();
        }
    }

    public void DisableJournal()
    {
        disabled = true;
    }

    public void EnableJournal()
    {
        disabled = false;
    }

    public void SetTutorialBool(bool set)
    {
        tutorial = set;
    }

    public void SetCrossyBool(bool set)
    {
        waitForCrossy = set;
    }

    public void SetLogTabBool(bool set)
    {
        logTab = set;
    }

    public void SetNotesTabBool(bool set)
    {
        notesTab = set;
    }
}
