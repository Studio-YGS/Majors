using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialSectionStart : MonoBehaviour
{
    Player_Controller playerController;

    public UnityEvent sectionStart, steppingAway, raycastEvent;

    public bool needsRaycast = false, needsSecondRaycast = false;

    void Start()
    {
        playerController = FindObjectOfType<Player_Controller>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GameController"))
        {
            sectionStart.Invoke();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("GameController"))
        {
            steppingAway.Invoke();
        }
    }

    void Update()
    {
        RaycastHit hit;

        if(Physics.Raycast(playerController.cam.position, playerController.cam.TransformDirection(Vector3.forward), out hit, 2f))
        {
            if(hit.transform.gameObject.CompareTag("Front Door") && needsRaycast) //for the front door part of the tutorial
            {
                Debug.Log("hit front door");
                needsRaycast = false;
                raycastEvent.Invoke();
            }

            if(hit.transform.gameObject.CompareTag("Holdable") && needsRaycast)
            {
                Debug.Log("Hit tag: " + hit.transform.gameObject.tag);
                needsRaycast = false;
                raycastEvent.Invoke();
            }

            if (hit.transform.gameObject.CompareTag("Holdable") && Input.GetKeyDown(KeyCode.E) && needsSecondRaycast)
            {
                needsSecondRaycast = false;
                sectionStart.Invoke();
            }

            if (hit.transform.gameObject.CompareTag("Note") && Input.GetKeyDown(KeyCode.E) && needsRaycast)
            {
                needsRaycast = false;
                raycastEvent.Invoke();
            }

            if (hit.transform.gameObject.CompareTag("Clue") && needsRaycast)
            { 
                raycastEvent.Invoke();
            }
        }
    }

    public void NeedsRaycast(bool need)
    {
        needsRaycast = need;
    }

    public void NeedsSecondRaycast(bool need)
    {
        needsSecondRaycast = need;
    }
}
