using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    [SerializeField]
    GameObject prompt;

    [SerializeField]
    Transform player;

    void Update()
    {
        RaycastHit hit;

        if(Physics.Raycast(player.position, player.TransformDirection(Vector3.forward), out hit, 2f))
        {
            if(hit.transform.gameObject.CompareTag("Note"))
            {
                prompt.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    hit.transform.gameObject.SetActive(false);
                    PickUpNote();
                }
            }
            else if (!hit.transform.gameObject.CompareTag("Note"))
            {
                prompt.SetActive(false);
            }
        }
        else if (!Physics.Raycast(player.position, player.TransformDirection(Vector3.forward), 2f))
        {
            prompt.SetActive(false);
        }
    }

    void PickUpNote()
    {
        
    }
}
