using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialSectionStart : MonoBehaviour
{
    Player_Controller playerController;

    public UnityEvent sectionStart, steppingAway, raycastEvent;

    public bool needsRaycast = false;

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

        if(Physics.Raycast(playerController.cam.position, playerController.cam.TransformDirection(Vector3.forward), out hit, 2f) && needsRaycast)
        {
            Debug.Log("bonk " + hit.transform.gameObject.name);
            if(hit.transform.gameObject.CompareTag("Front Door")) //for the front door part of the tutorial
            {
                Debug.Log("hit front door");
                needsRaycast = false;
                raycastEvent.Invoke();
            }

            if(hit.transform.gameObject.CompareTag("Holdable") && Input.GetKeyDown(KeyCode.E))
            {
                needsRaycast = false;
                raycastEvent.Invoke();
            }
        }
    }
}
