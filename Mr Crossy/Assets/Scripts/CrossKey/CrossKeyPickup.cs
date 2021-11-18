using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CrossKeyPickup : MonoBehaviour
{
    CrossKeyManager manager;
    Transform cam;
    TMP_Text hoverText;
    bool turnOffHoverText;
    void Start()
    {
        manager = FindObjectOfType<CrossKeyManager>();
        cam = FindObjectOfType<Camera>().transform;
        hoverText = GameObject.Find("Canvas").transform.Find("Hover Name").GetComponent<TMP_Text>();
    }

    
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, 5))
        {
            if (hit.collider == gameObject.GetComponent<Collider>())
            {
                hoverText.text = "Pick Up Cross Key";
                hoverText.gameObject.SetActive(true);
                turnOffHoverText = true;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    //for things the happen when key is picked up
                    manager.numOfKeys += 1;
                    if (!manager.firstKeyPickedUp)
                    {
                        manager.gameObject.GetComponent<TutorialSectionStart>().CrosskeyPickedUp(); //tutorial ui prompt
                        manager.firstKeyPickedUp = true;
                    }
                    hoverText.gameObject.SetActive(false);
                    Destroy(gameObject);
                }
            }
            else if (hit.collider != gameObject.GetComponent<Collider>() && turnOffHoverText)
            {
                turnOffHoverText = false;
                hoverText.gameObject.SetActive(false);
            }
        }
        else if (turnOffHoverText)
        {
            turnOffHoverText = false;
            hoverText.gameObject.SetActive(false);
        }
    }
}
