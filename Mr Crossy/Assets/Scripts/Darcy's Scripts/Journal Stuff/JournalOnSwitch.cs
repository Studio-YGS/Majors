using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalOnSwitch : MonoBehaviour
{
    bool open = false;

    public GameObject journalOpen, journalClosed;

    Player_Controller fpc;

    JournalController journalController;

    MenuManager pauseMenu;

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

            pauseMenu.streetName.SetActive(false);

            //paper sound

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

            pauseMenu.streetName.SetActive(true);

            //paper sound

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
