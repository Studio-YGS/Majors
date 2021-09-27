using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    [SerializeField]
    GameObject prompt;

    [SerializeField]
    Transform player;

    JournalController journalController;

    JournalOnSwitch journalOnSwitch;

    int currentNote = 0;

    void Awake()
    {
        journalOnSwitch = FindObjectOfType<JournalOnSwitch>();
        journalController = FindObjectOfType<JournalController>();
    }

    void Update()
    {
        RaycastHit hit;

        if(Physics.Raycast(player.position, player.TransformDirection(Vector3.forward), out hit, 5f))
        {
            if(hit.transform.gameObject.CompareTag("Note"))
            {
                prompt.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    hit.transform.gameObject.SetActive(false);
                    PickUpNote();
                }
            }
            else if (!hit.transform.gameObject.CompareTag("Note"))
            {
                prompt.SetActive(false);
            }
        }
        else if (!Physics.Raycast(player.position, player.TransformDirection(Vector3.forward), 5f))
        {
            prompt.SetActive(false);
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
