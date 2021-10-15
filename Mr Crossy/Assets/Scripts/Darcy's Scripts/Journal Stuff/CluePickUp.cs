using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CluePickUp : MonoBehaviour
{
    Player_Controller playerController;

    public UnityEvent pickUp;

    void Start()
    {
        playerController = FindObjectOfType<Player_Controller>();
    }

    void Update()
    {
        RaycastHit hit;

        if(Input.GetKeyDown(KeyCode.E) && Physics.Raycast(playerController.cam.position, playerController.transform.TransformDirection(Vector3.forward), out hit, 2f))
        {
            if (hit.transform.gameObject.CompareTag("Clue"))
            {
                pickUp.Invoke();
            }
        }
    }
}
