using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalOnSwitch : MonoBehaviour
{
    bool open = false;

    public GameObject journalOpen, journalClosed;

    Player_Controller fpc;

    JournalController journalController;

    void Start()
    {
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
