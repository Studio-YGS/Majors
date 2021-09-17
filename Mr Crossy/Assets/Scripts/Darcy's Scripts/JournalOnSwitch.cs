using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalOnSwitch : MonoBehaviour
{
    bool open = false;

    [SerializeField]
    GameObject journalOpen, journalClosed;

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
        open = true;
        journalClosed.SetActive(false);
        journalOpen.SetActive(true);
    }

    void CloseJournal()
    {
        open = false;
        journalClosed.SetActive(true);
        journalOpen.SetActive(false);
    }
}
