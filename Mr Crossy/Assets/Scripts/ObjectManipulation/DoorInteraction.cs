using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public bool moveable;
    Transform cam;
    public float rotationVal;
    Vector3 posLastFrame;
    public Quaternion hitRotation;
    void Start()
    {
        cam = FindObjectOfType<Camera>().transform;
        rotationVal = 0;
        hitRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
        {
            if(hit.collider == gameObject.GetComponent<Collider>())
            {
                moveable = true;
                
            }
            else
            {
                moveable = false;
                //rotationVal = 0;
                hitRotation = transform.rotation;
            }
        }
        else
        {
            moveable = false;
            //rotationVal = 0;
            hitRotation = transform.rotation;
        }

        if (moveable)
        {
            //if (Input.GetMouseButtonDown(0))
            //{
            //    hitRotation = transform.localRotation;
            //}
            if (Input.GetMouseButton(0))
            {
                //rotationVal += Input.GetAxis("Mouse X");
                //rotationVal = Mathf.Clamp(rotationVal, -30f, 30f);
                if(Input.GetAxis("Mouse X") > 0)
                {
                    rotationVal += 1;
                    //rotationVal += Input.GetAxis("Mouse X");

                }
                else if(Input.GetAxis("Mouse X") < 0)
                {
                    rotationVal -= 1;
                    //rotationVal -= Input.GetAxis("Mouse X");
                }
                //transform.rotation = Quaternion.Lerp(hitRotation, Quaternion.Euler(0, hitRotation.y + rotationVal * 2, 0) , Time.deltaTime);
                transform.localRotation = Quaternion.Euler(0, hitRotation.y + rotationVal, 0);
            }
            if (Input.GetMouseButtonUp(0))
            {
                hitRotation = transform.rotation;
            }
            

        }
    }
}
