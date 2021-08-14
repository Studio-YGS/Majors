using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHolder : MonoBehaviour
{
    Vector3 startPos;
    Quaternion startRot;
    Material mat;
    float dissolveValue;
    Transform hand;
    Transform objectInspectPoint;
    Transform cam;
    static bool objectHeld = false;
    bool dissolving;
    [HideInInspector] public bool thisObjectHeld;
    [HideInInspector] public bool isPlacedDown;
    Vector3 posLastFrame;
    Player_Controller controller;

    public float scaleFactor;
    public float pickupRange = 3;
    public Vector3 placementOffset;
    public Quaternion rotationalSet;
    public float distanceFromFace = 1.2f;

    void Start()
    {
        mat = gameObject.GetComponent<MeshRenderer>().material;
        startPos = transform.position;
        startRot = transform.rotation;
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
                    StopCoroutine("Dissolve");
                    dissolveValue = 0;
                    mat.SetFloat("Vector1_1bfaaeffe0534a91a219fc6f2e1eae9e", dissolveValue);
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
                RaycastHit detectWall;
                if (Physics.Raycast(cam.position, cam.forward, out detectWall, 0.5f))
                {
                    if (detectWall.collider == null)
                    {
                        transform.position = cam.position + cam.forward;
                    }
                    else
                    {
                        transform.position = cam.position - cam.forward;
                    }
                }
                gameObject.GetComponent<Collider>().enabled = true;
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
                gameObject.GetComponent<Rigidbody>().useGravity = true;
                gameObject.layer = 0;
                objectHeld = false;
                thisObjectHeld = false;
                StartCoroutine("Dissolve");
                if(controller.enabled == false)
                {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    controller.enabled = true;
                }
                
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
                transform.position = cam.position + cam.forward * distanceFromFace;
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

        if(dissolving == true)
        {
            dissolveValue += Time.deltaTime;
            mat.SetFloat("Vector1_1bfaaeffe0534a91a219fc6f2e1eae9e", dissolveValue);
        }
        else if(dissolving == false)
        {
            if (mat.GetFloat("Vector1_1bfaaeffe0534a91a219fc6f2e1eae9e") != 0)
            {
                dissolveValue -= Time.deltaTime;
                mat.SetFloat("Vector1_1bfaaeffe0534a91a219fc6f2e1eae9e", dissolveValue);
            }
        }

    }

    public void PutObjectDown()
    {
        objectHeld = false;
    }

    IEnumerator Dissolve()
    {
        yield return new WaitForSeconds(5);
        dissolving = true;
        while(mat.GetFloat("Vector1_1bfaaeffe0534a91a219fc6f2e1eae9e") < 1)
        {
            yield return null;
        }
        transform.position = startPos;
        transform.rotation = startRot;
        dissolving = false;
        while (mat.GetFloat("Vector1_1bfaaeffe0534a91a219fc6f2e1eae9e") > 0)
        {
            yield return null;
        }
        dissolveValue = 0;
        mat.SetFloat("Vector1_1bfaaeffe0534a91a219fc6f2e1eae9e", dissolveValue);
    }
}
