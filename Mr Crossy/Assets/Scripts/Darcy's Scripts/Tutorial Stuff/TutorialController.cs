using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    JournalController journalController;

    Player_Controller playerController;

    GameObject controlsUI;

    void Start()
    {
        journalController = FindObjectOfType<JournalController>();
        playerController = FindObjectOfType<Player_Controller>();
    }

    void ShowControls()
    {
        //StartCoroutine(Loading());
    }
}
