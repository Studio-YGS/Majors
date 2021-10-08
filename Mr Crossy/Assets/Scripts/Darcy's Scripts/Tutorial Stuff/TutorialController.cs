using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    JournalController journalController;

    Player_Controller playerController;

    GameObject controlsUI;

    bool reading = true;

    void Start()
    {
        journalController = FindObjectOfType<JournalController>();
        playerController = FindObjectOfType<Player_Controller>();
    }

    void ShowControls()
    {
        StartCoroutine(ShowingUI());
    }

    IEnumerator ShowingUI()
    {
        if (!reading)
        {

        }

        yield return null;
    }
}
