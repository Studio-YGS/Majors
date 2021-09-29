using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    bool moveable = false;
    bool greaterThan;
    bool lessThan;
    bool equalTo = true;
    Transform cam;
    public float rotationVal;
    Transform player;
    float angleRelativeToPlayer;
    public float openSpeed = 10;
    void Start()
    {
        cam = FindObjectOfType<Camera>().transform;
        rotationVal = 0;
        player = GameObject.Find("Fps Character").transform;
    }

    void Update()
    {
        //Debug.Log(transform.localRotation.y);
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
        {
            if(hit.collider == gameObject.GetComponent<Collider>())
            {
                
                if (Input.GetMouseButtonDown(0))
                {
                    moveable = true;
                    Vector3 direction = transform.parent.position - player.position;
                    angleRelativeToPlayer = Vector3.Angle(direction, transform.parent.forward);
                }
            }
        }
        if (Input.GetMouseButtonUp(0) && moveable)
        {
            if (transform.localRotation.y > 0.04)
            {
                greaterThan = true;
                lessThan = false;
                equalTo = false;
            }
            else if (transform.localRotation.y < -0.04)
            {
                greaterThan = false;
                lessThan = true;
                equalTo = false;
            }
            else if (transform.localRotation.y == 0)
            {
                greaterThan = false;
                lessThan = false;
                equalTo = true;
            }
            
            moveable = false;
        }
        

        if (moveable)
        {
            rotationVal = Mathf.Clamp(rotationVal, -90f, 90f);
            if (angleRelativeToPlayer > 180 * 0.5f)
            {
                //BEHIND
                if (greaterThan)
                {
                    if (Input.GetAxis("Mouse X") > 0)
                    {
                        rotationVal += Input.GetAxis("Mouse X") * openSpeed;

                    }
                    else if (Input.GetAxis("Mouse X") < 0)
                    {
                        rotationVal -= -Input.GetAxis("Mouse X") * openSpeed;
                    }
                }
                else if (lessThan)
                {
                    if (Input.GetAxis("Mouse X") < 0)
                    {
                        rotationVal += -Input.GetAxis("Mouse X") * openSpeed;

                    }
                    else if (Input.GetAxis("Mouse X") > 0)
                    {
                        rotationVal -= Input.GetAxis("Mouse X") * openSpeed;
                    }
                }
                else if (equalTo)
                {
                    if (Input.GetAxis("Mouse X") < 0)
                    {
                        rotationVal += -Input.GetAxis("Mouse X") * openSpeed;

                    }
                    else if (Input.GetAxis("Mouse X") > 0)
                    {
                        rotationVal -= Input.GetAxis("Mouse X") * openSpeed;
                    }
                }
            }
            else if (angleRelativeToPlayer < 180 * 0.5f)
            {
                //INFRONT
                if (greaterThan)
                {
                    if (Input.GetAxis("Mouse X") < 0)
                    {
                        rotationVal += -Input.GetAxis("Mouse X") * openSpeed;

                    }
                    else if (Input.GetAxis("Mouse X") > 0)
                    {
                        rotationVal -= Input.GetAxis("Mouse X") * openSpeed;
                    }
                }
                else if (lessThan)
                {
                    if (Input.GetAxis("Mouse X") > 0)
                    {
                        rotationVal += Input.GetAxis("Mouse X") * openSpeed;

                    }
                    else if (Input.GetAxis("Mouse X") < 0)
                    {
                        rotationVal -= -Input.GetAxis("Mouse X") * openSpeed;
                    }
                }
                else if (equalTo)
                {
                    if (Input.GetAxis("Mouse X") > 0)
                    {
                        rotationVal += Input.GetAxis("Mouse X") * openSpeed;

                    }
                    else if (Input.GetAxis("Mouse X") < 0)
                    {
                        rotationVal -= -Input.GetAxis("Mouse X") * openSpeed;
                    }
                }
            }
            if (Input.GetAxis("Mouse X") > 0)
            {
                

            }
            else if (Input.GetAxis("Mouse X") < 0)
            {
                
            }
            transform.localRotation = Quaternion.Euler(0,  rotationVal , 0);


        }
    }
}
