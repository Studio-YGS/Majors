using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class JournalOnSwitch : MonoBehaviour
{
    //[HideInInspector]
    public bool open = false;

    public GameObject journalOpen, journalClosed;

    Player_Controller fpc;

    JournalController journalController;

    MenuManager pauseMenu;

    EventInstance eventInstance;

    void Start()
    {
        pauseMenu = FindObjectOfType<MenuManager>();
        fpc = FindObjectOfType<Player_Controller>();
        journalController = FindObjectOfType<JournalController>();
    }

    public bool OpenOrClose()
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

    public void ForceOpenOrClose()
    {
        OpenOrClose();
    }

    void OpenJournal()
    {
        if (!journalController.disabled)
        {
            fpc.DisableController();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            pauseMenu.streetName.SetActive(false);

            journalController.OpenMap();

            eventInstance = RuntimeManager.CreateInstance("event:/2D/Paper/Paper Up");

            eventInstance.start();

            open = true;
            journalClosed.SetActive(false);
            journalOpen.SetActive(true);
        }
    }

    void CloseJournal()
    {
        if (!journalController.disabled)
        {
            fpc.EnableController();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            pauseMenu.streetName.SetActive(true);

            eventInstance = RuntimeManager.CreateInstance("event:/2D/Paper/Paper Up");

            eventInstance.start();

            open = false;
            journalClosed.SetActive(true);
            journalOpen.SetActive(false);
        }
    }

    public void HideTab()
    {
        journalClosed.SetActive(false);
    }

    public void ShowTab()
    {
        journalClosed.SetActive(true);
    }
}
