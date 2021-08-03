using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHolder : MonoBehaviour
{
    Transform hand;
    Transform objectInspectPoint;
    Transform cam;
    static bool objectHeld = false;

    public float scaleFactor;
    public float pickupRange = 3;
    public Vector3 placementOffset;

    void Start()
    {
        hand = GameObject.FindGameObjectWithTag("Hand").transform;
        objectInspectPoint = hand.GetComponentInChildren<Transform>();
        cam = FindObjectOfType<Camera>().transform;
    }

    
    void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(cam.position, cam.forward, out hit, pickupRange))
        {
            if(hit.collider == gameObject.GetComponent<Collider>())
            {
                if (Input.GetKeyDown(KeyCode.E) && !objectHeld)
                {
                    transform.parent = hand;
                    transform.position = hand.position;
                    gameObject.GetComponent<Collider>().enabled = false;
                    objectHeld = true;
                }
            }
        }
    }

    public void PutObjectDown()
    {
        objectHeld = false;
    }
}
