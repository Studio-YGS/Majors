using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class ObjectOpener : MonoBehaviour
{
    ObjectHolder holder;
    GameObject pressE;
    Transform player;
    Vector3 direction;
    Vector3 rotDirection;
    float angleXRelativeToPlayer;
    float angleYRelativeToPlayer;
    bool turnOff;
    bool InteractionOne;
    bool InteractionTwo;

    public UnityEvent FirstObjectInteractionActions;
    public GameObject key;
    [Header("First Interaction")]
    public float xAngle;
    public float yAngle;
    //public Vector3 rotationOffset;
    void Start()
    {
        holder = GetComponent<ObjectHolder>();
        player = FindObjectOfType<Camera>().transform;
        pressE = GameObject.Find("Canvas").transform.Find("PressE").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(player.position, key.transform.position - player.position, Color.green);
        //Debug.DrawRay(transform.position, player.position - transform.position, Color.red);
        //(player.position) - (transform.position + transform.up * rotationOffset.y + transform.right * rotationOffset.x + transform.forward * rotationOffset.z), Color.red);
        //Debug.Log(angleXRelativeToPlayer);
        //Debug.Log(UnityEditor.TransformUtils.GetInspectorRotation(gameObject.transform).x);
        //Debug.Log(Quaternion.Euler(0, 0, 30).z);
        //Debug.Log(Quaternion.LookRotation(player.position));
        if (holder.beingInspected)
        {
            direction = player.position - transform.position;
            //rotDirection = player.position  - transform.position + (player.forward * rotationOffset.z) + (player.up * rotationOffset.y) + (player.right * rotationOffset.x);
            angleXRelativeToPlayer = Vector3.Angle(direction, -transform.right);
            angleYRelativeToPlayer = Vector3.Angle(direction, transform.up);
            if (angleXRelativeToPlayer < xAngle * 0.5f /*&& angleYRelativeToPlayer < yAngle * 0.5f*/ &&
                transform.localRotation.x  < Quaternion.Euler(30,0,0).x && transform.localRotation.x  > Quaternion.Euler(-30, 0, 0).x &&
                transform.localRotation.z < Quaternion.Euler(0, 0, 30).z && transform.localRotation.z > Quaternion.Euler(0, 0, -30).z && !InteractionOne)
            {
                if (!turnOff )
                {
                    pressE.SetActive(true);
                    turnOff = true;
                }
                if (Input.GetKeyDown(KeyCode.E) )
                {
                    FirstObjectInteractionActions.Invoke();
                    InteractionOne = true;
                    pressE.SetActive(false);
                    //holder.enabled = false;
                    //StartCoroutine(RotateToNewPosition());
                }
            }
            else if (turnOff && !InteractionOne)
            {
                pressE.SetActive(false);
                turnOff = false;
            }
            //transform.RotateAround(transform.GetComponent<Renderer>().bounds.center, player.up, 1);
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rotationOffset), 0.01f);

            if (InteractionOne && !InteractionTwo)
            {
                RaycastHit hit;
                if (Physics.Raycast(player.position, key.transform.position - player.position, out hit, 5))
                {
                    if (hit.collider == key.GetComponent<Collider>())
                    {
                        Debug.Log("hit");
                        if (!turnOff)
                        {
                            pressE.SetActive(true);
                            turnOff = true;
                        }
                        if (Input.GetKeyUp(KeyCode.E))
                        {
                            //other stuff when key is picked up
                            holder.Drop();
                            key.SetActive(false);
                            holder.controller.enabled = true;
                            
                            Cursor.visible = false;
                            Cursor.lockState = CursorLockMode.Locked;

                            InteractionTwo = true;
                            pressE.SetActive(false);
                            holder.beingInspected = false;
                        }
                    }
                    else if (turnOff)
                    {
                        pressE.SetActive(false);
                        turnOff = false;
                    }
                }
                
            }
            else if (turnOff && InteractionOne)
            {
                pressE.SetActive(false);
                turnOff = false;
            }

        }
        else if (turnOff)
        {
            pressE.SetActive(false);
            turnOff = false;
        }

        //IEnumerator RotateToNewPosition()
        //{
        //    rotationOffset = transform.InverseTransformVector(rotationOffset);
        //    while (transform.rotation != Quaternion.Slerp(transform.rotation, Quaternion.Euler(19.5249825f, 228.726837f, 16.6993694f), 0.01f))
        //    {
        //        //Transform offset = ;
                
        //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(19.5249825f, 228.726837f, 16.6993694f), 0.01f);
        //        //Vector3 posOffset = transform.position - transform.GetComponent<Renderer>().bounds.center;
        //        //transform.position = player.position + player.forward * holder.distanceFromFace + posOffset;
        //        //transform.position = Vector3.Lerp(transform.position, player.position + player.forward * holder.distanceFromFace + posOffset, 0.01f);

        //        yield return null;
        //    }
        //    holder.enabled = true;
        //}
    }
}
