using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CluePickUp : MonoBehaviour
{
    Player_Controller playerController;

    public UnityEvent pickUp;

    bool first = true;

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

                int number;

                if (first)
                {
                    number = 4;
                }
                else
                {
                    number = 2;
                }

                if (GetComponentInParent<TutorialSectionStart>())
                {
                    GetComponentInParent<TutorialSectionStart>().CheckClues(number);
                }
            }
        }
    }
}
