using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacement : MonoBehaviour
{
    Transform hand;
    Transform cam;

    public float placementRange = 3;
    bool objectPlaced;
    GameObject PlacedObject;
    ObjectHolder heldObject;

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
                if (Input.GetKeyDown(KeyCode.E) && !objectPlaced)
                {
                    foreach(Transform child in hand)
                    {
                        if (child.gameObject.activeSelf)
                        {
                            heldObject = child.GetComponentInChildren<ObjectHolder>();
                            break;
                        }
                    }
                    PlacedObject = heldObject.gameObject;
                    heldObject.transform.position = transform.position + heldObject.placementOffset;
                    FMODUnity.RuntimeManager.PlayOneShot("event:/2D/Puzzle/Altar_Placement");
                    heldObject.isPlacedDown = true;
                    heldObject.thisObjectHeld = false;
                    heldObject.transform.parent = null;
                    foreach(Transform PO in PlacedObject.transform)
                    {
                        PO.gameObject.layer = 0;
                    }
                    PlacedObject.layer = 0;
                    PlacedObject.transform.rotation = heldObject.rotationalSet;
                    objectPlaced = true;
                    gameObject.layer = 2;
                    heldObject.transform.localScale = heldObject.ogScaleFactor;
                    heldObject.PlacedOnPedestal(PlacedObject);

                    if (GetComponentInParent<DetermineLetter>())
                    {
                        DetermineLetter letter = GetComponentInParent<DetermineLetter>();
                        letter.ObjectPlaced(PlacedObject); //sending to darcy's script
                    }
                    else if (GetComponentInParent<OverlappedAltar>())
                    {
                        GetComponentInParent<OverlappedAltar>().ReceiveObject(PlacedObject); //overlapping altar stuff
                    }
                    
                    StartCoroutine(ColliderOn());
                }
            }
        }

        if(PlacedObject != null)
        {
            if(PlacedObject.GetComponent<ObjectHolder>().isPlacedDown == false)
            {
                if (GetComponentInParent<DetermineLetter>())
                {
                    DetermineLetter letter = GetComponentInParent<DetermineLetter>();
                    letter.ObjectPickedUp(); //sending to darcy's script
                }
                else if (GetComponentInParent<OverlappedAltar>())
                {
                    GetComponentInParent<OverlappedAltar>().ObjectPickedUp(); //overlapping altar stuff
                }

                objectPlaced = false;
                gameObject.layer = 0;
                PlacedObject = null;
            }
        }
    }

    IEnumerator ColliderOn()
    {
        yield return new WaitForSeconds(1);
        PlacedObject.GetComponent<Collider>().enabled = true;
    }
}
