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
                    
                    ObjectHolder heldObject = hand.GetComponentInChildren<ObjectHolder>();
                    PlacedObject = heldObject.gameObject;
                    heldObject.transform.position = transform.position + heldObject.placementOffset;
                    heldObject.PutObjectDown();
                    heldObject.isPlacedDown = true;
                    heldObject.thisObjectHeld = false;
                    heldObject.transform.parent = null;
                    PlacedObject.layer = 0;
                    PlacedObject.transform.rotation = heldObject.rotationalSet;
                    objectPlaced = true;
                    heldObject.transform.localScale = heldObject.ogScaleFactor;
                    heldObject.image.gameObject.SetActive(false);
                    heldObject.textName.gameObject.SetActive(false);
                    DetermineLetter letter = GetComponentInParent<DetermineLetter>();
                    letter.ObjectPlaced(PlacedObject); //sending to darcy's script
                    StartCoroutine(ColliderOn());
                }
            }
        }

        if(PlacedObject != null)
        {
            if(PlacedObject.GetComponent<ObjectHolder>().isPlacedDown == false)
            {
                DetermineLetter letter = GetComponentInParent<DetermineLetter>();
                letter.ObjectPickedUp(); //sending to darcy's script
                objectPlaced = false;
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
