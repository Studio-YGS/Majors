using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacement : MonoBehaviour
{
    Transform hand;
    Transform cam;

    public float placementRange = 3;


    void Start()
    {
        hand = GameObject.FindGameObjectWithTag("Hand").transform;
        cam = FindObjectOfType<Camera>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        

        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, placementRange))
        {
            if (hit.collider == gameObject.GetComponent<Collider>() && hand.GetComponentInChildren<ObjectHolder>())
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    ObjectHolder heldObject = hand.GetComponentInChildren<ObjectHolder>();
                    heldObject.transform.position = transform.position + heldObject.placementOffset;
                    heldObject.PutObjectDown();
                    heldObject.GetComponent<Collider>().enabled = true;
                    heldObject.transform.parent = null;
                    
                }
            }
        }
    }
}
