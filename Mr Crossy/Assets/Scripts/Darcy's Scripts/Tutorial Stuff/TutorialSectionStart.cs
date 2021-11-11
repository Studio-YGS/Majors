using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialSectionStart : MonoBehaviour
{
    Player_Controller playerController;

    public UnityEvent sectionStart, steppingAway, raycastEvent;

    public bool needsRaycast = false, needsSecondRaycast = false, needsCheck = false;

    bool first = true;

    [SerializeField]
    GameObject[] tutorialClues;

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
                needsRaycast = false;
                raycastEvent.Invoke();
            }

            if(hit.transform.gameObject.CompareTag("Holdable") && needsRaycast)
            {
                needsRaycast = false;
                raycastEvent.Invoke();
            }
            else if (hit.transform.gameObject.CompareTag("Holdable") && Input.GetKeyDown(KeyCode.E) && needsSecondRaycast)
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

            if(hit.transform.gameObject.CompareTag("Crosskey") && needsRaycast && Input.GetKeyDown(KeyCode.E))
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

    public void CheckClues()
    {
        int count = 0;

        if (needsCheck)
        {
            for (int i = 0; i < tutorialClues.Length; i++)
            {
                if (!tutorialClues[i].activeInHierarchy)
                {
                    count++;
                }

                if(count == 4 && first)
                {
                    first = false;
                    sectionStart.Invoke();
                    break;
                }

                if (count == 6 && !first)
                {
                    steppingAway.Invoke();
                    break;
                }
            }
        }
    }

    public void ReadHowTo()
    {
        sectionStart.Invoke();
    }

    public void TutorialOver()
    {
        Debug.Log("Tutorial is completed");
    }
}
