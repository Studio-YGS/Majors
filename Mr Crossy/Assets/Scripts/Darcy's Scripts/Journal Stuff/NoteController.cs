using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FMOD.Studio;
using FMODUnity;
using UnityEngine.UI;

public class NoteController : MonoBehaviour
{
    Player_Controller player;

    JournalController journalController;

    JournalOnSwitch journalOnSwitch;

    Sprite noteImage;

    Color alphaOne;

    int currentNote;

    bool tutorialLine, found;

    EventInstance eventInstance;

    [SerializeField]
    TextMeshProUGUI prompt;

    void Start()
    {
        journalOnSwitch = FindObjectOfType<JournalOnSwitch>();
        journalController = FindObjectOfType<JournalController>();
        player = FindObjectOfType<Player_Controller>();

        alphaOne.a = 1f;
        alphaOne = Color.white;
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
                    noteImage = hit.transform.gameObject.GetComponent<NoteAssign>().assignedNote;

                    hit.transform.gameObject.SetActive(false);
                    GameObject.Find(hit.transform.gameObject.name).SetActive(false);

                    currentNote = hit.transform.gameObject.GetComponent<NoteAssign>().page;

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
        GameObject[] notes = journalController.notePages;
        for(int i = 0; i < notes.Length; i++)
        {
            Image[] images = notes[i].GetComponentsInChildren<Image>();

            for(int x = 0; x < images.Length; x++)
            {
                if(images[x].sprite == null)
                {
                    images[x].sprite = noteImage;
                    images[x].color = alphaOne;
                    found = true;
                    break;
                }
            }

            if (found)
            {
                break;
            }
        }

        found = false;

        journalController.whichNotesPage = currentNote;
        journalController.noteList.Add(currentNote);

        journalOnSwitch.OpenOrClose();

        journalController.OpenNotes();
    }
}
