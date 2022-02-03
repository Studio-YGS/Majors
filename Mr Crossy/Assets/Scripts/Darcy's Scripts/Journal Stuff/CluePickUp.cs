using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FMOD.Studio;
using FMODUnity;
using TMPro;

public class CluePickUp : MonoBehaviour //this script just handles the raycasting for the clues
{
    Player_Controller playerController;

    public UnityEvent pickUp;

    EventInstance eventInstance;

    [SerializeField]
    TextMeshProUGUI prompt;

    void Start()
    {
        playerController = FindObjectOfType<Player_Controller>();
    }

    void Update()
    {
        RaycastHit hit;

        if(Physics.Raycast(playerController.cam.position, playerController.cam.TransformDirection(Vector3.forward), out hit, 2f))
        {
            if (hit.transform.gameObject.CompareTag("Clue") && hit.transform.gameObject.name == gameObject.name) //if the object that was hit is tagged as a clue and is the same object as the object that this component is attached to
            {
                prompt.text = "Press E to pick up clue(s)."; //popping the prompt up so the player knows they can pick it up
                prompt.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E)) //the player can then press e to pick up the note
                {
                    pickUp.Invoke();

                    eventInstance = RuntimeManager.CreateInstance("event:/2D/Paper/Paper Up");

                    eventInstance.start();
                }
            }
            else //if they are not hitting something tagged with Clue, or if they aren't hitting anything at all, it will disable the prompt.
            {
                prompt.gameObject.SetActive(false);
            }
        }
        else
        {
            prompt.gameObject.SetActive(false);
        }
    }

    public void PickUpOverride() //a force pickup for the devskip
    {
        pickUp.Invoke();
    }
}
