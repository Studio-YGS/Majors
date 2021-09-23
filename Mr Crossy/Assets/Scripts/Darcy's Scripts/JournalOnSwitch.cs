using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalOnSwitch : MonoBehaviour
{
    bool open = false;

    [SerializeField]
    GameObject journalOpen, journalClosed;

    FPController fpc;

    void Awake()
    {
        fpc = FindObjectOfType<FPController>();
    }

    public bool OpenOrClosed()
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
