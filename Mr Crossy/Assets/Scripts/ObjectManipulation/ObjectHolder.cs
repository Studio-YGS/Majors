using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Outline))]
public class ObjectHolder : MonoBehaviour
{
    public Sprite objectImage;
    public string objectName;
    [HideInInspector] public Image image;
    [HideInInspector] public TMP_Text textName;
    [HideInInspector] public Image imageTwo;
    [HideInInspector] public TMP_Text textNameTwo;
    TMP_Text hoverText;
    bool turnOffHoverText;
    Vector3 startPos;
    Quaternion startRot;
    Material mat;
    //float dissolveValue;
    [HideInInspector] public Transform hand;
    Transform objectInspectPoint;
    Transform cam;
    static GameObject heldObject;
    //static GameObject[] objectsInHands;
    static List<GameObject> objectsInHands = new List<GameObject>();
    bool dissolving;
    [HideInInspector] public bool thisObjectHeld;
    [HideInInspector] public bool isPlacedDown;
    [HideInInspector] public bool beingInspected;
    //Vector3 posLastFrame;
    [HideInInspector] public Player_Controller controller;

    [HideInInspector] public Vector3 ogScaleFactor;
    public float pickupRange = 5;
    [Header("In Hand")]
    public Vector3 handOffset;
    public Quaternion handRotation;
    public Vector3 scaleFactor = new Vector3 (1,1,1);
    [Header("On Pedestal")]
    public Vector3 placementOffset;
    public Quaternion rotationalSet;
    [Header("When Inspecting")]
    public float distanceFromFace = 1.2f;
    [Header("Testing Pos In Hand")]
    public bool updatePos;
    Vector3 newHandPosition = Vector3.zero;
    Quaternion newHandRotation = new Quaternion(0, 0, 0, 0);
    Vector3 newScaleFactor;

    void Start()
    {
        thisObjectHeld = false;
        mat = gameObject.GetComponent<MeshRenderer>().material;
        startPos = transform.position;
        startRot = transform.rotation;
        hand = GameObject.FindGameObjectWithTag("Hand").transform;
        objectInspectPoint = hand.GetComponentInChildren<Transform>();
        cam = FindObjectOfType<Camera>().transform;
        controller = FindObjectOfType<Player_Controller>();
        image = GameObject.Find("Canvas").transform.Find("Item Square 1").transform.Find("Object Image").GetComponent<Image>();
        textName = GameObject.Find("Canvas").transform.Find("Object Name").GetComponent<TMP_Text>();
        
        imageTwo = GameObject.Find("Canvas").transform.Find("Item Square 2").transform.Find("Object Image").GetComponent<Image>();
        textNameTwo = GameObject.Find("Canvas").transform.Find("Object Name 2").GetComponent<TMP_Text>();

        hoverText = GameObject.Find("Canvas").transform.Find("Hover Name").GetComponent<TMP_Text>();
        newHandPosition = handOffset;
        newHandRotation = handRotation;
        newScaleFactor = scaleFactor;
        ogScaleFactor = transform.localScale;
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
                if (Input.GetKeyDown(KeyCode.E) && objectsInHands.Count == 2)
                {
                    DropCurrentObject(heldObject);
                    heldObject = intereactedObject;
                    PickUpObject(intereactedObject);
                    //objectHeld = true;
                }
                else if (Input.GetKeyDown(KeyCode.E) && objectsInHands.Count == 1)
                {
                    PickUpObject(intereactedObject);
                    //objectHeld = true;
                }
                else if (Input.GetKeyDown(KeyCode.E) && objectsInHands.Count == 0)
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
            Drop();
        }

        if(objectsInHands.Count == 2 && thisObjectHeld)
        {
            if(Input.GetAxis("Mouse ScrollWheel") != 0f )
            {
                if(objectsInHands[0].activeSelf == true)
                {
                    objectsInHands[0].SetActive(false);
                    objectsInHands[1].SetActive(true);
                    heldObject = objectsInHands[1];
                    image.GetComponentInParent<Transform>().localScale = new Vector3(1f, 1f, 1f);
                    imageTwo.GetComponentInParent<Transform>().localScale = new Vector3(1.3f, 1.3f, 1.3f);
                }
                else if (objectsInHands[1].activeSelf == true)
                {
                    objectsInHands[1].SetActive(false);
                    objectsInHands[0].SetActive(true);
                    heldObject = objectsInHands[0];
                    imageTwo.GetComponentInParent<Transform>().localScale = new Vector3(1f, 1f, 1f);
                    image.GetComponentInParent<Transform>().localScale = new Vector3(1.3f, 1.3f, 1.3f);
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                objectsInHands[1].SetActive(false);
                objectsInHands[0].SetActive(true);
                heldObject = objectsInHands[0];
                imageTwo.GetComponentInParent<Transform>().localScale = new Vector3(1f, 1f, 1f);
                image.GetComponentInParent<Transform>().localScale = new Vector3(1.3f, 1.3f, 1.3f);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                objectsInHands[0].SetActive(false);
                objectsInHands[1].SetActive(true);
                heldObject = objectsInHands[1];
                image.GetComponentInParent<Transform>().localScale = new Vector3(1f, 1f, 1f);
                imageTwo.GetComponentInParent<Transform>().localScale = new Vector3(1.3f, 1.3f, 1.3f);
            }
        }

        if (thisObjectHeld && !FindObjectOfType<CrossKeyManager>().puzzleOn)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Vector3 posOffset = transform.position - transform.GetComponent<Renderer>().bounds.center;
                transform.position = cam.position + cam.forward * distanceFromFace + posOffset;
                controller.enabled = false;
                beingInspected = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else if (Input.GetMouseButtonUp(1))
            {
                controller.enabled = true;
                beingInspected = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                transform.position = hand.TransformPoint(handOffset);
                transform.localRotation = handRotation;
            }
            if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
            {
                float rotX = Input.GetAxis("Mouse X") * 200 * Mathf.Deg2Rad;
                float rotY = Input.GetAxis("Mouse Y") * 200 * Mathf.Deg2Rad;
                transform.RotateAround(transform.GetComponent<Renderer>().bounds.center, cam.up, -rotX);
                transform.RotateAround(transform.GetComponent<Renderer>().bounds.center, cam.right, rotY);
            }

        }

        //if(dissolving == true)
        //{
        //    dissolveValue += Time.deltaTime;
        //    mat.SetFloat("Vector1_1bfaaeffe0534a91a219fc6f2e1eae9e", dissolveValue);
        //}
        //else if(dissolving == false)
        //{
        //    if (mat.GetFloat("Vector1_1bfaaeffe0534a91a219fc6f2e1eae9e") != 0)
        //    {
        //        dissolveValue -= Time.deltaTime;
        //        mat.SetFloat("Vector1_1bfaaeffe0534a91a219fc6f2e1eae9e", dissolveValue);
        //    }
        //}
        
        if(updatePos && thisObjectHeld)
        {
            
            if(handOffset != newHandPosition)
            {
                transform.position = hand.TransformPoint(handOffset);
                newHandPosition = handOffset;
            }
            if(handRotation != newHandRotation)
            {
                transform.localRotation = handRotation;
                newHandRotation = handRotation;
            }
            if(scaleFactor != newScaleFactor)
            {
                transform.localScale = scaleFactor;
                newScaleFactor = scaleFactor;
            }
            
        }

    }


    public void Drop()
    {
        if (thisObjectHeld)
        {
            DropCurrentObject(heldObject);
            if (objectsInHands.Count == 1)
            {
                objectsInHands[0].SetActive(true);
                heldObject = objectsInHands[0];
            }
        }
    }
    void DropCurrentObject(GameObject item)
    {
        ObjectHolder itemObjectHolder = item.GetComponent<ObjectHolder>();
        if (item == objectsInHands[0])
        {
            if (objectsInHands.Count == 2)
            {
                image.sprite = objectsInHands[1].GetComponent<ObjectHolder>().objectImage;
                textName.text = objectsInHands[1].GetComponent<ObjectHolder>().objectName;
                imageTwo.gameObject.SetActive(false);
                textNameTwo.gameObject.SetActive(false);
            }
            else
            {
                image.gameObject.SetActive(false);
                textName.gameObject.SetActive(false);
            }
            objectsInHands.Remove(objectsInHands[0]);
            
        }
        else if (item == objectsInHands[1])
        {
            objectsInHands.Remove(objectsInHands[1]);
            imageTwo.gameObject.SetActive(false);
            textNameTwo.gameObject.SetActive(false);
            
        }
        image.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);

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
        heldObject.GetComponent<Outline>().enabled = true;
        foreach (Transform HO in heldObject.transform)
        {
            HO.gameObject.layer = 0;
            if (HO.GetComponent<Outline>())
            {
                HO.GetComponent<Outline>().enabled = true;
            }
            foreach (Transform t in HO.transform)
            {
                t.gameObject.layer = 0;
                foreach (Transform T in t.transform)
                {
                    T.gameObject.layer = 0;
                }
                if (t.GetComponent<Outline>())
                {
                    t.GetComponent<Outline>().enabled = true;
                }
            }
        }
        itemObjectHolder.thisObjectHeld = false;
        itemObjectHolder.StartCoroutine("Dissolve");
        
        heldObject.transform.localScale = itemObjectHolder.ogScaleFactor;
        if (controller.enabled == false)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            controller.enabled = true;
        }

    }

    void PickUpObject(GameObject item)
    {
        ObjectHolder itemObjectHolder = item.GetComponent<ObjectHolder>();
        if (objectsInHands.Count == 0)
        {
            objectsInHands.Add(item);
            heldObject = item;
            image.gameObject.SetActive(true);
            textName.gameObject.SetActive(true);
            itemObjectHolder.image.sprite = objectImage;
            textName.text = itemObjectHolder.objectName;
            image.GetComponentInParent<Transform>().localScale = new Vector3(1.3f, 1.3f, 1.3f);
        }
        else if (objectsInHands.Count == 1)
        {
            objectsInHands.Add(item);
            objectsInHands[0].SetActive(false);
            heldObject = item;
            imageTwo.gameObject.SetActive(true);
            textNameTwo.gameObject.SetActive(true);
            itemObjectHolder.imageTwo.sprite = objectImage;
            textNameTwo.text = itemObjectHolder.objectName;
            image.GetComponentInParent<Transform>().localScale = new Vector3(1f, 1f, 1f);
            imageTwo.GetComponentInParent<Transform>().localScale = new Vector3(1.3f, 1.3f, 1.3f);
        }

        
        itemObjectHolder.StopCoroutine("Dissolve");
        itemObjectHolder.dissolving = false;
        //itemObjectHolder.dissolveValue = 0;
        //itemObjectHolder.mat.SetFloat("Vector1_1bfaaeffe0534a91a219fc6f2e1eae9e", dissolveValue);
        itemObjectHolder.transform.localScale = scaleFactor;
        //itemObjectHolder.transform.position = hand.position + handOffset;
        itemObjectHolder.transform.position = hand.TransformPoint(handOffset);
        itemObjectHolder.transform.parent = hand;
        itemObjectHolder.gameObject.GetComponent<Collider>().enabled = false;
        itemObjectHolder.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        itemObjectHolder.gameObject.GetComponent<Rigidbody>().useGravity = false;
        itemObjectHolder.transform.localRotation = handRotation;
        itemObjectHolder.gameObject.layer = 6;
        itemObjectHolder.GetComponent<Outline>().enabled = false;
        foreach (Transform HO in itemObjectHolder.transform)
        {
            HO.gameObject.layer = 6;
            if (HO.GetComponent<Outline>())
            {
                HO.GetComponent<Outline>().enabled = false;
            }
            foreach(Transform t in HO.transform)
            {
                t.gameObject.layer = 6;
                foreach(Transform T in t.transform)
                {
                    T.gameObject.layer = 6;
                }
                if (t.GetComponent<Outline>())
                {
                    t.GetComponent<Outline>().enabled = false;
                }
            }
        }
        itemObjectHolder.isPlacedDown = false;
        itemObjectHolder.thisObjectHeld = true;
        
        

    }

    public void PlacedOnPedestal(GameObject item)
    {
        ObjectHolder itemObjectHolder = item.GetComponent<ObjectHolder>();
        if (item == objectsInHands[0])
        {
            if (objectsInHands.Count == 2)
            {
                image.sprite = objectsInHands[1].GetComponent<ObjectHolder>().objectImage;
                textName.text = objectsInHands[1].GetComponent<ObjectHolder>().objectName;
                imageTwo.gameObject.SetActive(false);
                textNameTwo.gameObject.SetActive(false);
            }
            else
            {
                image.gameObject.SetActive(false);
                textName.gameObject.SetActive(false);
            }
            objectsInHands.Remove(objectsInHands[0]);

        }
        else if (item == objectsInHands[1])
        {
            objectsInHands.Remove(objectsInHands[1]);
            imageTwo.gameObject.SetActive(false);
            textNameTwo.gameObject.SetActive(false);

        }
        if (objectsInHands.Count == 1)
        {
            objectsInHands[0].SetActive(true);
            heldObject = objectsInHands[0];
        }
        image.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
    }

    IEnumerator Dissolve()
    {
        yield return new WaitForSeconds(5);
        dissolving = true;
        //while(mat.GetFloat("Vector1_1bfaaeffe0534a91a219fc6f2e1eae9e") < 1)
        //{
        //    yield return null;
        //}
        transform.position = startPos;
        transform.rotation = startRot;
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        dissolving = false;
        //while (mat.GetFloat("Vector1_1bfaaeffe0534a91a219fc6f2e1eae9e") > 0)
        //{
        //    yield return null;
        //}
        //dissolveValue = 0;
        //mat.SetFloat("Vector1_1bfaaeffe0534a91a219fc6f2e1eae9e", dissolveValue);
    }
}
