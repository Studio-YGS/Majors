using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyLockDoor : MonoBehaviour
{
    public bool hasKey;
    public DoorInteraction door;
    public GameObject displayMessage;
    Transform cam;
    Player_Controller controller;
    bool displayedTheMessage;
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
                    displayMessage.SetActive(true);
                }
                else if(Input.GetKeyDown(KeyCode.E) && hasKey)
                {
                    door.keyDoor = null;
                }
            }
            else if (displayedTheMessage)
            {
                displayedTheMessage = false;
                displayMessage.SetActive(false);
            }
        }
        else if (displayedTheMessage)
        {
            displayedTheMessage = false;
            displayMessage.SetActive(false);
        }
    }
}
