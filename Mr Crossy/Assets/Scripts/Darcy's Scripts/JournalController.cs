using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalController : MonoBehaviour
{
    [SerializeField]
    GameObject[] notePages, logPages;

    [SerializeField]
    GameObject log, map, notes, rightArrow, leftArrow;

    int whichTab = 1, whichLogPage = 0;
    [HideInInspector]
    public int whichNotesPage = 0; 

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
    }

    public void OpenLog()
    {
        log.SetActive(true);
        map.SetActive(false);
        notes.SetActive(false);
        whichTab = 1;

        for (int i = 0; i < logPages.Length; i++)
        {
            logPages[i].SetActive(false);
        }

        logPages[whichLogPage].SetActive(true);

        if (whichLogPage == 0)
        {
            leftArrow.SetActive(false);
            rightArrow.SetActive(true);
        }
        else if (whichLogPage == logPages.Length - 1)
        {
            leftArrow.SetActive(true);
            rightArrow.SetActive(false);
        }
        else
        {
            leftArrow.SetActive(true);
            rightArrow.SetActive(true);
        }
    }

    public void OpenMap()
    {
        log.SetActive(false);
        map.SetActive(true);
        notes.SetActive(false);
        rightArrow.SetActive(false);
        leftArrow.SetActive(false);
        whichTab = 2;
    }

    public void OpenNotes()
    {
        log.SetActive(false);
        map.SetActive(false);
        notes.SetActive(true);
        whichTab = 3;

        for (int i = 0; i < notePages.Length; i++)
        {
            notePages[i].SetActive(false);
        }

        notePages[whichNotesPage].SetActive(true);

        if (whichNotesPage == 0)
        {
            leftArrow.SetActive(false);
            rightArrow.SetActive(true);
        }
        else if (whichNotesPage == notePages.Length - 1)
        {
            leftArrow.SetActive(true);
            rightArrow.SetActive(false);
        }
        else
        {
            leftArrow.SetActive(true);
            rightArrow.SetActive(true);
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
                        whichLogPage++;
                        OpenLog();
                        break;
                    }
                case 2:
                    {
                        OpenMap();
                        break;
                    }
                case 3:
                    {
                        whichNotesPage++;
                        OpenNotes();
                        break;
                    }
            }
        }
        else
        {
            switch (whichTab)
            {
                case 1:
                    {
                        whichLogPage--;
                        OpenLog();
                        break;
                    }
                case 2:
                    {
                        OpenMap();
                        break;
                    }
                case 3:
                    {
                        whichNotesPage--;
                        OpenNotes();
                        break;
                    }
            }
        }
    }
}
