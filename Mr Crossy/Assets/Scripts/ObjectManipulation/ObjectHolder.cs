using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHolder : MonoBehaviour
{
    Transform hand;
    Transform objectInspectPoint;
    Transform cam;
    static bool objectHeld = false;
    [HideInInspector] public bool thisObjectHeld;
    [HideInInspector] public bool isPlacedDown;

    public float scaleFactor;
    public float pickupRange = 3;
    public Vector3 placementOffset;
    public Quaternion rotationalSet;

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
                    gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    gameObject.GetComponent<Rigidbody>().useGravity = false;
                    objectHeld = true;
                    isPlacedDown = false;
                    thisObjectHeld = true;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (thisObjectHeld)
            {
                transform.parent = null;
                transform.position = cam.position + cam.forward;
                gameObject.GetComponent<Collider>().enabled = true;
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
                gameObject.GetComponent<Rigidbody>().useGravity = true;
                objectHeld = false;
                thisObjectHeld = false;
            }
        }

    }

    public void PutObjectDown()
    {
        objectHeld = false;
    }
}
