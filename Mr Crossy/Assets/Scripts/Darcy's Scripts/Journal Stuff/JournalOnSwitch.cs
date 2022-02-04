using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class JournalOnSwitch : MonoBehaviour //this script just handles checking whether or not the journal is on or off, and switches it accordingly.
{
    [HideInInspector]
    public bool open = false;

    public GameObject journalOpen, journalClosed;

    Player_Controller fpc;

    JournalController journalController;

    MenuManager menuManager;

    EventInstance eventInstance;

    HeadBob headBob;

    void Start()
    {
        menuManager = FindObjectOfType<MenuManager>();
        fpc = FindObjectOfType<Player_Controller>();
        journalController = FindObjectOfType<JournalController>();
        headBob = FindObjectOfType<HeadBob>();
    }

    public bool OpenOrClose() //this method checks to see if the journal is on or off, and returns true or false respectively.
    {
        if (open)
        {
            CloseJournal();
            return false;
        }
        else
        {
            OpenJournal();
            return true;
        }
    }

    public void ForceOpenOrClose() //a force that the unity events use
    {
        OpenOrClose();
    }

    void OpenJournal() //this method opens the journal if it isn't disabled
    {
        if (!journalController.disabled)
        {
            fpc.DisableController(); //diables the player controller, and the head bob
            headBob.enabled = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None; //unlocks the cursor and makes it visible

            journalController.OpenMap(); //by default when the player opens the journal, it will open to the map

            eventInstance = RuntimeManager.CreateInstance("event:/2D/Paper/Paper Up");

            eventInstance.start();

            open = true;
            journalClosed.SetActive(false);
            journalOpen.SetActive(true);
        }
    }

    void CloseJournal() //this method closes the journal if it isn't disabled
    {
        if (!journalController.disabled)
        {
            fpc.EnableController(); //reenalbes player controller, head bob, relocks cursor and makes it invisible
            headBob.enabled = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            eventInstance = RuntimeManager.CreateInstance("event:/2D/Paper/Paper Up");

            eventInstance.start();

            open = false;
            journalClosed.SetActive(true);
            journalOpen.SetActive(false);
        }
    }

    //these methods just show or hide the tab if needed, for things like pausing the game
    public void HideTab()
    {
        journalClosed.SetActive(false);
    }

    public void ShowTab()
    {
        journalClosed.SetActive(true);
    }
}
