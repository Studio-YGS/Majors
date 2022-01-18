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
    public ObjectVariableHolder vHolder;
    //[HideInInspector] public Image image;
    //[HideInInspector] public TMP_Text textName;
    //[HideInInspector] public Image imageTwo;
    //[HideInInspector] public TMP_Text textNameTwo;
    //TMP_Text hoverText;
    bool turnOffHoverText;
    public static bool forcedRaycast;
    Vector3 startPos;
    Quaternion startRot;
    //Material mat;
    //float dissolveValue;

    //TOGGLE THE BELOW WHEN DONE EDITING
    [HideInInspector] public Transform hand;
    Transform objectInspectPoint;
    //Transform cam;
    static GameObject heldObject;
    //static GameObject[] objectsInHands;
    static List<GameObject> objectsInHands = new List<GameObject>();
    bool dissolving;
    [HideInInspector] public bool thisObjectHeld;
    [HideInInspector] public bool isPlacedDown;
    [HideInInspector] public bool beingInspected;
    [HideInInspector] public static bool objectBeingInspected;
    //Vector3 posLastFrame;
    //[HideInInspector] public Player_Controller controller;

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

    // updatePos is for editing the position in hand when in playmode
    [Header("Testing Pos In Hand")]
    public bool updatePos;
    Vector3 newHandPosition = Vector3.zero;
    Quaternion newHandRotation = new Quaternion(0, 0, 0, 0);
    Vector3 newScaleFactor;

    void Start()
    {
        thisObjectHeld = false;
        //vHolder = FindObjectOfType<ObjectVariableHolder>();
        //mat = gameObject.GetComponent<MeshRenderer>().material;
        startPos = transform.position;
        startRot = transform.rotation;
        
        //TOGGLE THE BELOW WHEN DONE EDITING
        hand = vHolder.hand;
        
        objectInspectPoint = vHolder.hand.GetComponentInChildren<Transform>();
        //cam = vHolder.cam; ;
        //controller = vHolder.controller;
        //image = vHolder.image;
        //textName = vHolder.textName;

        //imageTwo = vHolder.imageTwo;
        //textNameTwo = vHolder.textNameTwo;

        //hoverText = vHolder.hoverText;

        //TOGGLE THE BELOW 3 LINES WHEN DONE EDITING
        newHandPosition = handOffset;
        newHandRotation = handRotation;
        newScaleFactor = scaleFactor;
        ogScaleFactor = transform.localScale;
    }


    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(vHolder.cam.position, vHolder.cam.forward, out hit, pickupRange, vHolder.controller.raycastLayerMask))
        {
            if (hit.collider == gameObject.GetComponent<Collider>())
            {
                vHolder.hoverText.text = "Pick Up " + objectName;
                vHolder.hoverText.gameObject.SetActive(true);
                if (!forcedRaycast && GameObject.Find("Game Manager"))
                {
                    forcedRaycast = true;
                    GameObject.Find("Game Manager").GetComponent<TutorialSectionStart>().ForceRaycast();
                }
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
                vHolder.hoverText.gameObject.SetActive(false);
            }
        }
        else if (turnOffHoverText)
        {
            turnOffHoverText = false;
            vHolder.hoverText.gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Drop();
        }



        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            if (objectsInHands.Count == 2 && thisObjectHeld && !FindObjectOfType<MenuManager>().menuOpen)
            {
                if (objectsInHands[0].activeSelf == true)
                {
                    objectsInHands[0].SetActive(false);
                    objectsInHands[1].SetActive(true);
                    heldObject = objectsInHands[1];
                    vHolder.image.GetComponentInParent<Transform>().localScale = new Vector3(1f, 1f, 1f);
                    vHolder.imageTwo.GetComponentInParent<Transform>().localScale = new Vector3(1.3f, 1.3f, 1.3f);
                    Debug.Log("Change");
                }
                else if (objectsInHands[1].activeSelf == true)
                {
                    objectsInHands[1].SetActive(false);
                    objectsInHands[0].SetActive(true);
                    heldObject = objectsInHands[0];
                    vHolder.imageTwo.GetComponentInParent<Transform>().localScale = new Vector3(1f, 1f, 1f);
                    vHolder.image.GetComponentInParent<Transform>().localScale = new Vector3(1.3f, 1.3f, 1.3f);
                }
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (objectsInHands.Count == 2 && thisObjectHeld && !FindObjectOfType<MenuManager>().menuOpen)
            {
                objectsInHands[1].SetActive(false);
                objectsInHands[0].SetActive(true);
                heldObject = objectsInHands[0];
                vHolder.imageTwo.GetComponentInParent<Transform>().localScale = new Vector3(1f, 1f, 1f);
                vHolder.image.GetComponentInParent<Transform>().localScale = new Vector3(1.3f, 1.3f, 1.3f);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (objectsInHands.Count == 2 && thisObjectHeld && !FindObjectOfType<MenuManager>().menuOpen)
            {
                objectsInHands[0].SetActive(false);
                objectsInHands[1].SetActive(true);
                heldObject = objectsInHands[1];
                vHolder.image.GetComponentInParent<Transform>().localScale = new Vector3(1f, 1f, 1f);
                vHolder.imageTwo.GetComponentInParent<Transform>().localScale = new Vector3(1.3f, 1.3f, 1.3f);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (thisObjectHeld && !FindObjectOfType<CrossKeyManager>().puzzleOn && !DoorInteraction.beingMoved)
            {
                Vector3 posOffset = transform.position - transform.GetComponent<Renderer>().bounds.center;
                transform.position = vHolder.cam.position + vHolder.cam.forward * distanceFromFace + posOffset;
                vHolder.controller.enabled = false;
                beingInspected = true;
                objectBeingInspected = true;
                vHolder.headbob.enabled = false;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
        else if (Input.GetMouseButtonUp(1))
        {
            if (thisObjectHeld && !FindObjectOfType<CrossKeyManager>().puzzleOn && !DoorInteraction.beingMoved)
            {
                vHolder.controller.enabled = true;
                beingInspected = false;
                objectBeingInspected = false;
                vHolder.headbob.enabled = true;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                transform.position = vHolder.hand.TransformPoint(handOffset);
                transform.localRotation = handRotation;
            }
        }
        if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
        {
            if (thisObjectHeld && !FindObjectOfType<CrossKeyManager>().puzzleOn && !DoorInteraction.beingMoved)
            {
                float rotX = Input.GetAxis("Mouse X") * 200 * Mathf.Deg2Rad;
                float rotY = Input.GetAxis("Mouse Y") * 200 * Mathf.Deg2Rad;
                transform.RotateAround(transform.GetComponent<Renderer>().bounds.center, vHolder.cam.up, -rotX);
                transform.RotateAround(transform.GetComponent<Renderer>().bounds.center, vHolder.cam.right, rotY);
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

        if (updatePos && thisObjectHeld)
        {

            if (handOffset != newHandPosition)
            {
                transform.position = hand.TransformPoint(handOffset);
                newHandPosition = handOffset;
            }
            if (handRotation != newHandRotation)
            {
                transform.localRotation = handRotation;
                newHandRotation = handRotation;
            }
            if (scaleFactor != newScaleFactor)
            {
                transform.localScale = scaleFactor;
                newScaleFactor = scaleFactor;
            }

        }

    }

    public void ObjectDroppedWhileInspecting()
    {
        objectBeingInspected = false;
    }

    public void DeathDrop()
    {
        if(objectsInHands.Count == 1)
        {
            DropCurrentObject(heldObject);
        }
        else if(objectsInHands.Count == 2)
        {
            DropCurrentObject(heldObject);
            objectsInHands[0].SetActive(true);
            heldObject = objectsInHands[0];
            DropCurrentObject(heldObject);
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
                vHolder.image.sprite = objectsInHands[1].GetComponent<ObjectHolder>().objectImage;
                vHolder.textName.text = objectsInHands[1].GetComponent<ObjectHolder>().objectName;
                vHolder.imageTwo.gameObject.SetActive(false);
                vHolder.textNameTwo.gameObject.SetActive(false);
            }
            else
            {
                vHolder.image.gameObject.SetActive(false);
                vHolder.textName.gameObject.SetActive(false);
            }
            objectsInHands.Remove(objectsInHands[0]);
            
        }
        else if (item == objectsInHands[1])
        {
            objectsInHands.Remove(objectsInHands[1]);
            vHolder.imageTwo.gameObject.SetActive(false);
            vHolder.textNameTwo.gameObject.SetActive(false);
            
        }
        vHolder.image.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);

        heldObject.transform.parent = null;
        
        RaycastHit detectWall;
        if (Physics.Raycast(vHolder.cam.position, vHolder.cam.forward, out detectWall, 0.5f, vHolder.controller.raycastLayerMask))
        {
            if (detectWall.collider == null)
            {
                heldObject.transform.position = vHolder.cam.position + vHolder.cam.forward;
            }
            else
            {
                heldObject.transform.position = vHolder.cam.position - vHolder.cam.forward;
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
        if (vHolder.controller.enabled == false)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            vHolder.controller.enabled = true;
        }

    }

    void PickUpObject(GameObject item)
    {
        ObjectHolder itemObjectHolder = item.GetComponent<ObjectHolder>();
        if (objectsInHands.Count == 0)
        {
            objectsInHands.Add(item);
            heldObject = item;
            vHolder.image.gameObject.SetActive(true);
            vHolder.textName.gameObject.SetActive(true);
            itemObjectHolder.vHolder.image.sprite = objectImage;
            vHolder.textName.text = itemObjectHolder.objectName;
            vHolder.image.GetComponentInParent<Transform>().localScale = new Vector3(1.3f, 1.3f, 1.3f);
        }
        else if (objectsInHands.Count == 1)
        {
            objectsInHands.Add(item);
            objectsInHands[0].SetActive(false);
            heldObject = item;
            vHolder.imageTwo.gameObject.SetActive(true);
            vHolder.textNameTwo.gameObject.SetActive(true);
            itemObjectHolder.vHolder.imageTwo.sprite = objectImage;
            vHolder.textNameTwo.text = itemObjectHolder.objectName;
            vHolder.image.GetComponentInParent<Transform>().localScale = new Vector3(1f, 1f, 1f);
            vHolder.imageTwo.GetComponentInParent<Transform>().localScale = new Vector3(1.3f, 1.3f, 1.3f);
        }

        FMODUnity.RuntimeManager.PlayOneShot("event:/2D/Object Interaction/Object Pickup");
        itemObjectHolder.StopCoroutine("Dissolve");
        itemObjectHolder.dissolving = false;
        //itemObjectHolder.dissolveValue = 0;
        //itemObjectHolder.mat.SetFloat("Vector1_1bfaaeffe0534a91a219fc6f2e1eae9e", dissolveValue);
        itemObjectHolder.transform.localScale = scaleFactor;
        //itemObjectHolder.transform.position = hand.position + handOffset;
        itemObjectHolder.transform.position = vHolder.hand.TransformPoint(handOffset);
        itemObjectHolder.transform.parent = vHolder.hand;
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
                vHolder.image.sprite = objectsInHands[1].GetComponent<ObjectHolder>().objectImage;
                vHolder.textName.text = objectsInHands[1].GetComponent<ObjectHolder>().objectName;
                vHolder.imageTwo.gameObject.SetActive(false);
                vHolder.textNameTwo.gameObject.SetActive(false);
            }
            else
            {
                vHolder.image.gameObject.SetActive(false);
                vHolder.textName.gameObject.SetActive(false);
            }
            objectsInHands.Remove(objectsInHands[0]);

        }
        else if (item == objectsInHands[1])
        {
            objectsInHands.Remove(objectsInHands[1]);
            vHolder.imageTwo.gameObject.SetActive(false);
            vHolder.textNameTwo.gameObject.SetActive(false);

        }
        if (objectsInHands.Count == 1)
        {
            objectsInHands[0].SetActive(true);
            heldObject = objectsInHands[0];
        }
        vHolder.image.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
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
