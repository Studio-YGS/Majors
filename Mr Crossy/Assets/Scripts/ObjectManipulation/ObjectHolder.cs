using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.UI;
using UnityEngine.UI;

public class ObjectHolder : MonoBehaviour
{
    public Sprite objectImage;
    public string objectName;
    [HideInInspector] public Image image;
    [HideInInspector] public TMP_Text textName;
    TMP_Text hoverText;
    bool turnOffHoverText;
    Vector3 startPos;
    Quaternion startRot;
    Material mat;
    float dissolveValue;
    Transform hand;
    Transform objectInspectPoint;
    Transform cam;
    static bool objectHeld = false;
    static GameObject heldObject;
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
        image = GameObject.Find("Canvas").transform.Find("Object Image").GetComponent<Image>();
        textName = GameObject.Find("Canvas").transform.Find("Object Name").GetComponent<TMP_Text>();
        hoverText = GameObject.Find("Canvas").transform.Find("Hover Name").GetComponent<TMP_Text>();
    }

    
    void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(cam.position, cam.forward, out hit, pickupRange))
        {
            if(hit.collider == gameObject.GetComponent<Collider>())
            {
                hoverText.text = "Pick Up " + objectName;
                hoverText.gameObject.SetActive(true);
                turnOffHoverText = true;
                GameObject intereactedObject = hit.collider.gameObject;
                if (Input.GetKeyDown(KeyCode.E) && objectHeld)
                {
                    DropCurrentObject(heldObject);
                    PickUpObject(intereactedObject);
                    objectHeld = true;
                }
                else if (Input.GetKeyDown(KeyCode.E) && !objectHeld)
                {
                    
                    PickUpObject(intereactedObject);
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

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (thisObjectHeld)
            {
                DropCurrentObject(heldObject);
                
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

    void DropCurrentObject(GameObject item)
    {
        ObjectHolder itemObjectHolder = item.GetComponent<ObjectHolder>();
        heldObject.transform.parent = null;
        RaycastHit detectWall;
        if (Physics.Raycast(cam.position, cam.forward, out detectWall, 0.5f))
        {
            if (detectWall.collider == null)
            {
                heldObject.transform.position = cam.position + cam.forward;
            }
            else
            {
                heldObject.transform.position = cam.position - cam.forward;
            }
        }
        heldObject.gameObject.GetComponent<Collider>().enabled = true;
        heldObject.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        heldObject.gameObject.GetComponent<Rigidbody>().useGravity = true;
        heldObject.gameObject.layer = 0;
        objectHeld = false;
        itemObjectHolder.thisObjectHeld = false;
        itemObjectHolder.StartCoroutine("Dissolve");
        image.gameObject.SetActive(false);
        textName.gameObject.SetActive(false);
        if (controller.enabled == false)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            controller.enabled = true;
        }
    }

    void PickUpObject(GameObject item)
    {
        heldObject = item;
        ObjectHolder itemObjectHolder = item.GetComponent<ObjectHolder>();
        itemObjectHolder.StopCoroutine("Dissolve");
        itemObjectHolder.dissolving = false;
        itemObjectHolder.dissolveValue = 0;
        itemObjectHolder.mat.SetFloat("Vector1_1bfaaeffe0534a91a219fc6f2e1eae9e", dissolveValue);
        itemObjectHolder.transform.parent = hand;
        itemObjectHolder.transform.position = hand.position;
        itemObjectHolder.gameObject.GetComponent<Collider>().enabled = false;
        itemObjectHolder.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        itemObjectHolder.gameObject.GetComponent<Rigidbody>().useGravity = false;
        itemObjectHolder.transform.rotation = new Quaternion(0, 0, 0, 0);
        itemObjectHolder.gameObject.layer = 6;
        objectHeld = true;
        itemObjectHolder.isPlacedDown = false;
        itemObjectHolder.thisObjectHeld = true;
        image.gameObject.SetActive(true);
        textName.gameObject.SetActive(true);
        itemObjectHolder.image.sprite = objectImage;
        textName.text = itemObjectHolder.objectName;
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
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        dissolving = false;
        while (mat.GetFloat("Vector1_1bfaaeffe0534a91a219fc6f2e1eae9e") > 0)
        {
            yield return null;
        }
        dissolveValue = 0;
        mat.SetFloat("Vector1_1bfaaeffe0534a91a219fc6f2e1eae9e", dissolveValue);
    }
}
