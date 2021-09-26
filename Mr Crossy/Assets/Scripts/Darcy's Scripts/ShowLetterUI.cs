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
            if (hit.collider == gameObject.GetComponent<Collider>())
            {
                uiLetter.text = hit.transform.gameObject.name;
                uiLetter.transform.gameObject.SetActive(true);
            }
        }

        if (Physics.Raycast(cam.position, cam.forward, out hit, pickupRange))
        {
            if (!hit.collider == gameObject.GetComponent<Collider>())
            {
                uiLetter.transform.gameObject.SetActive(false);
            }
        }

        if (!Physics.Raycast(cam.position, cam.forward, pickupRange))
        {
            uiLetter.transform.gameObject.SetActive(false);
        }
    }
}
