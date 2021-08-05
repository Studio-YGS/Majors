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
    Vector3 posLastFrame;
    Player_Controller controller;

    public float scaleFactor;
    public float pickupRange = 3;
    public Vector3 placementOffset;
    public Quaternion rotationalSet;

    void Start()
    {
        hand = GameObject.FindGameObjectWithTag("Hand").transform;
        objectInspectPoint = hand.GetComponentInChildren<Transform>();
        cam = FindObjectOfType<Camera>().transform;
        controller = FindObjectOfType<Player_Controller>();
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
                    transform.rotation = new Quaternion (0,0,0,0);
                    gameObject.layer = 6;
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
                gameObject.layer = 0;
                objectHeld = false;
                thisObjectHeld = false;
            }
        }

        if (thisObjectHeld)
        {
            if (Input.GetMouseButtonDown(0))
            {
                posLastFrame = Input.mousePosition;
            }
            if (Input.GetMouseButtonDown(1))
            {
                transform.position = cam.position + cam.forward * 2;
                controller.enabled = false;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else if (Input.GetMouseButtonUp(1))
            {
                controller.enabled = true;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                transform.position = hand.position;
                transform.rotation = new Quaternion(0, 0, 0, 0);
            }
            if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
            {
                var delta = Input.mousePosition - posLastFrame;
                posLastFrame = Input.mousePosition;

                var axis = Quaternion.AngleAxis(-90f, Vector3.forward) * delta;
                transform.rotation = Quaternion.AngleAxis(delta.magnitude * 0.3f, axis) * transform.rotation;
            }

        }

        

    }

    public void PutObjectDown()
    {
        objectHeld = false;
    }
}
