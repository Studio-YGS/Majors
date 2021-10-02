using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalOnSwitch : MonoBehaviour
{
    bool open = false;

    [SerializeField]
    GameObject journalOpen, journalClosed;

    Player_Controller fpc;

    void Awake()
    {
        fpc = FindObjectOfType<Player_Controller>();
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

    void OpenJournal()
    {
        fpc.DisableController();

        open = true;
        journalClosed.SetActive(false);
        journalOpen.SetActive(true);
    }

    void CloseJournal()
    {
        fpc.EnableController();

        open = false;
        journalClosed.SetActive(true);
        journalOpen.SetActive(false);
    }
}
