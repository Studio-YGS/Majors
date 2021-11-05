using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowPromptUI : MonoBehaviour
{
    public TextMeshProUGUI pickupPrompt, generalPrompt;

    public bool canShow = true;

    Player_Controller playerController;

    void Start()
    {
        playerController = FindObjectOfType<Player_Controller>();
    }

    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(playerController.cam.position, playerController.transform.TransformDirection(Vector3.forward), out hit, 2f) && canShow)
        {
            string tag = hit.transform.gameObject.tag;

            switch (tag)
            {
                case "Holdable":
                    {
                        pickupPrompt.text = "Press E to pick up: " + hit.transform.gameObject.name;
                        pickupPrompt.gameObject.SetActive(true);
                        break;
                    }
                case "Clue":
                    {
                        pickupPrompt.text = "Press E to read clue";
                        pickupPrompt.gameObject.SetActive(true);
                        break;
                    }
                case "Note":
                    {
                        pickupPrompt.text = "Press E to read note";
                        pickupPrompt.gameObject.SetActive(true);
                        break;
                    }
            }

            if (!hit.transform.gameObject.CompareTag("Note") && !hit.transform.gameObject.CompareTag("Clue") && !hit.transform.gameObject.CompareTag("Holdable"))
            {
                pickupPrompt.gameObject.SetActive(false);
            }
        }

        if (!Physics.Raycast(playerController.cam.position, playerController.transform.TransformDirection(Vector3.forward), 2f))
        {
            pickupPrompt.gameObject.SetActive(false);
        }
    }
}
