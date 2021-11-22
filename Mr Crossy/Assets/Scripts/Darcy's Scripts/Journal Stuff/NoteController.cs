using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FMOD.Studio;
using FMODUnity;

public class NoteController : MonoBehaviour
{
    Player_Controller player;

    JournalController journalController;

    JournalOnSwitch journalOnSwitch;

    int currentNote = 0;

    bool tutorialLine = false;

    EventInstance eventInstance;

    [SerializeField]
    TextMeshProUGUI prompt;

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
                prompt.text = "Press E to read note.";
                prompt.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    hit.transform.gameObject.GetComponent<NoteAssign>().assignedNote.SetActive(true);
                    hit.transform.gameObject.SetActive(false);

                    eventInstance = RuntimeManager.CreateInstance("event:/2D/Paper/Paper Up");

                    eventInstance.start();

                    PickUpNote();

                    if (!tutorialLine)
                    {
                        GetComponent<TutorialSectionStart>().NoteTutorialLine();
                        tutorialLine = true;
                    }
                }
            }
            else
            {
                prompt.gameObject.SetActive(false);
            }
        }
        else
        {
            prompt.gameObject.SetActive(false);
        }
    }

    void PickUpNote()
    {
        //if (!tutorialLine)
        //{

        //}
        journalController.whichNotesPage = currentNote;
        journalController.noteList.Add(currentNote);

        journalOnSwitch.OpenOrClose();
        journalController.OpenNotes();

        currentNote++;
    }
}
