using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowLetterUI : MonoBehaviour
{
    [SerializeField]
    Text uiLetter;

    float pickupRange = 3;

    Transform cam;

    void Start()
    {
        cam = FindObjectOfType<Camera>().transform;
    }

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, pickupRange))
        {
            if (hit.transform.gameObject.CompareTag("Holdable"))
            {
                uiLetter.text = "Press E to pick up: " + hit.transform.gameObject.name;
                uiLetter.gameObject.SetActive(true);
            }
            else if(!hit.transform.gameObject.CompareTag("Holdable"))
            {
                uiLetter.gameObject.SetActive(false);
            }
        }

        if (!Physics.Raycast(cam.position, cam.forward, pickupRange))
        {
            uiLetter.gameObject.SetActive(false);
        }
    }
}
