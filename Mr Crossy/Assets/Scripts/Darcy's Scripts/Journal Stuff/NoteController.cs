using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    Player_Controller player;

    JournalController journalController;

    JournalOnSwitch journalOnSwitch;

    int currentNote = 0;

    void Awake()
    {
        journalOnSwitch = FindObjectOfType<JournalOnSwitch>();
        journalController = FindObjectOfType<JournalController>();
        player = FindObjectOfType<Player_Controller>();
    }

    void Update()
    {
        RaycastHit hit;

        if(Physics.Raycast(player.cam.position, player.cam.TransformDirection(Vector3.forward), out hit, 2f))
        {
            if(hit.transform.gameObject.CompareTag("Note"))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    hit.transform.gameObject.SetActive(false);
                    PickUpNote();
                }
            }
        }
    }

    void PickUpNote()
    {
        journalController.whichNotesPage = currentNote;
        journalController.noteList.Add(currentNote);
        journalController.OpenNotes();

        journalOnSwitch.OpenOrClose();

        currentNote++;
    }
}
