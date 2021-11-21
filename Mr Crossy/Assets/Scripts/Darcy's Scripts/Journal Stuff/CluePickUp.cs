using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FMOD.Studio;
using FMODUnity;

public class CluePickUp : MonoBehaviour
{
    Player_Controller playerController;

    public UnityEvent pickUp;

    EventInstance eventInstance;

    void Start()
    {
        playerController = FindObjectOfType<Player_Controller>();
    }

    void Update()
    {
        RaycastHit hit;

        if(Input.GetKeyDown(KeyCode.E) && Physics.Raycast(playerController.cam.position, playerController.cam.TransformDirection(Vector3.forward), out hit, 2f))
        {
            if (hit.transform.gameObject.CompareTag("Clue") && hit.transform.gameObject.name == gameObject.name)
            {
                pickUp.Invoke();

                eventInstance = RuntimeManager.CreateInstance("event:/2D/Paper/Paper Up");

                eventInstance.start();

                if (GetComponentInParent<TutorialSectionStart>())
                {
                    GetComponentInParent<TutorialSectionStart>().CheckClues();
                }
            }
        }
    }
}
