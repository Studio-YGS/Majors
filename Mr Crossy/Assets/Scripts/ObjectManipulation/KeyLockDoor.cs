using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyLockDoor : MonoBehaviour
{
    public bool hasKey;
    public DoorInteraction door;
    public GameObject displayMessageLocked;
    public GameObject displayMessageUnlocked;
    Transform cam;
    Player_Controller controller;
    bool displayedTheMessage;
    bool unlocked;
    public UnityEvent openActions;
    void Start()
    {
        cam = GameObject.Find("FirstPersonCharacter").transform;
        controller = FindObjectOfType<Player_Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 2, controller.raycastLayerMask))
        {
            if (hit.collider == gameObject.GetComponent<Collider>())
            {
                displayedTheMessage = true;
                if (!hasKey)
                {
                    displayMessageLocked.SetActive(true);
                }
                else if(hasKey && !unlocked)
                {
                    displayMessageUnlocked.SetActive(true);
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        displayMessageUnlocked.SetActive(false);
                        if (door)
                        {
                            door.keyDoor = null;
                        }
                        openActions.Invoke();
                        unlocked = true;

                    }
                    
                }
            }
            else if (displayedTheMessage)
            {
                displayedTheMessage = false;
                displayMessageLocked.SetActive(false);
                displayMessageUnlocked.SetActive(false);
            }
        }
        else if (displayedTheMessage)
        {
            displayedTheMessage = false;
            displayMessageLocked.SetActive(false);
            displayMessageUnlocked.SetActive(false);
        }
    }
}
