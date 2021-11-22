using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialSectionStart : MonoBehaviour
{
    Player_Controller playerController;

    public UnityEvent sectionStart, steppingAway, raycastEvent;

    public bool needsRaycast = false, needsSecondRaycast = false, needsCheck = false, atGate;

    bool first = true;

    [SerializeField]
    GameObject[] tutorialClues;

    [SerializeField]
    GameObject locked;

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

            if (hit.transform.gameObject.CompareTag("Clue") && needsRaycast)
            {
                raycastEvent.Invoke();
            }

            if(hit.transform.gameObject.CompareTag("Crosskey") && needsRaycast && Input.GetKeyDown(KeyCode.E))
            {
                raycastEvent.Invoke();
            }

            if(hit.transform.gameObject.CompareTag("Gate") && atGate)
            {
                if (locked != null)
                {
                    locked.SetActive(true);
                }
            }
            else if(!hit.transform.gameObject.CompareTag("Gate") && atGate)
            {
                if(locked != null)
                {
                    locked.SetActive(false);
                }
            }
        }
        else if(!Physics.Raycast(playerController.cam.position, playerController.cam.TransformDirection(Vector3.forward), 2f))
        {
            if (locked != null)
            {
                locked.SetActive(false);
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

    public void AtGate(bool at)
    {
        atGate = at;
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

    public void WaitForCrossy()
    {
        steppingAway.Invoke();
    }

    public void NoteTutorialLine()
    {
        sectionStart.Invoke();
    }

    public void ObjectsTeach() 
    {
        sectionStart.Invoke();
    }

    public void CrosskeyPickedUp()
    {
        sectionStart.Invoke();
    }

    public void TutorialOver()
    {
        Debug.Log("Tutorial is completed");
    }
}
