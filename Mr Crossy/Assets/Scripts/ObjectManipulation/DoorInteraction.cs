using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DoorInteraction : MonoBehaviour
{
    public bool xForward;
    public bool zForward;
    bool moveable = false;
    bool greaterThan;
    bool lessThan;
    bool equalTo = true;
    bool touchingPlayerRight;
    bool touchingPlayerLeft;
    Transform cam;
    public float rotationVal;
    Transform player;
    float angleRelativeToPlayer;
    float relativeAngle;
    public float openSpeed = 10;
    public bool locked;
    void Start()
    {
        cam = FindObjectOfType<Camera>().transform;
        rotationVal = 0;
        player = GameObject.Find("Fps Character").transform;
    }

    void Update()
    {
        if (locked)
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
            {
                if (hit.collider == gameObject.GetComponent<Collider>())
                {

                    if (Input.GetMouseButtonDown(0))
                    {
                        FindObjectOfType<CrossKeyManager>().StartCrossKeyPuzzle(gameObject.GetComponent<DoorInteraction>());

                    }
                }
            }
        }
        if (!locked)
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit))
            {
                if (hit.collider == gameObject.GetComponent<Collider>())
                {

                    if (Input.GetMouseButtonDown(0))
                    {
                        moveable = true;
                        Vector3 direction = transform.parent.position - player.position;
                        if (xForward)
                        {
                            angleRelativeToPlayer = Vector3.Angle(direction, transform.parent.right);
                        }
                        else if (zForward)
                        {
                            angleRelativeToPlayer = Vector3.Angle(direction, transform.parent.forward);
                        }

                    }
                }
            }
            if (Input.GetMouseButtonUp(0) && moveable)
            {
                if (transform.localRotation.y > 0.02)
                {
                    greaterThan = true;
                    lessThan = false;
                    equalTo = false;
                }
                else if (transform.localRotation.y < -0.02)
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
                        if (Input.GetAxis("Mouse X") > 0 && !touchingPlayerLeft)
                        {
                            rotationVal += Input.GetAxis("Mouse X") * openSpeed;

                        }
                        else if (Input.GetAxis("Mouse X") < 0 && !touchingPlayerRight)
                        {
                            rotationVal -= -Input.GetAxis("Mouse X") * openSpeed;
                        }
                    }
                    else if (lessThan)
                    {
                        if (Input.GetAxis("Mouse X") < 0 && !touchingPlayerLeft)
                        {
                            rotationVal += -Input.GetAxis("Mouse X") * openSpeed;

                        }
                        else if (Input.GetAxis("Mouse X") > 0 && !touchingPlayerRight)
                        {
                            rotationVal -= Input.GetAxis("Mouse X") * openSpeed;
                        }
                    }
                    else if (equalTo)
                    {
                        if (zForward)
                        {
                            if (Input.GetAxis("Mouse X") < 0 && !touchingPlayerRight)
                            {
                                rotationVal += -Input.GetAxis("Mouse X") * openSpeed;

                            }
                            else if (Input.GetAxis("Mouse X") > 0 && !touchingPlayerLeft)
                            {
                                rotationVal -= Input.GetAxis("Mouse X") * openSpeed;
                            }
                        }
                        else if (xForward)
                        {
                            if (Input.GetAxis("Mouse X") < 0 && !touchingPlayerLeft)
                            {
                                rotationVal += -Input.GetAxis("Mouse X") * openSpeed;

                            }
                            else if (Input.GetAxis("Mouse X") > 0 && !touchingPlayerRight)
                            {
                                rotationVal -= Input.GetAxis("Mouse X") * openSpeed;
                            }
                        }
                    }
                }
                else if (angleRelativeToPlayer < 180 * 0.5f)
                {
                    //INFRONT
                    if (greaterThan)
                    {
                        if (Input.GetAxis("Mouse X") < 0 && !touchingPlayerLeft)
                        {
                            rotationVal += -Input.GetAxis("Mouse X") * openSpeed;

                        }
                        else if (Input.GetAxis("Mouse X") > 0 && !touchingPlayerRight)
                        {
                            rotationVal -= Input.GetAxis("Mouse X") * openSpeed;
                        }
                    }
                    else if (lessThan)
                    {
                        if (Input.GetAxis("Mouse X") > 0 && !touchingPlayerLeft)
                        {
                            rotationVal += Input.GetAxis("Mouse X") * openSpeed;

                        }
                        else if (Input.GetAxis("Mouse X") < 0 && !touchingPlayerRight)
                        {
                            rotationVal -= -Input.GetAxis("Mouse X") * openSpeed;
                        }
                    }
                    else if (equalTo)
                    {
                        if (zForward)
                        {
                            if (Input.GetAxis("Mouse X") > 0 && !touchingPlayerLeft)
                            {
                                rotationVal += Input.GetAxis("Mouse X") * openSpeed;

                            }
                            else if (Input.GetAxis("Mouse X") < 0 && !touchingPlayerRight)
                            {
                                rotationVal -= -Input.GetAxis("Mouse X") * openSpeed;
                            }
                        }
                        else if (xForward)
                        {
                            if (Input.GetAxis("Mouse X") > 0 && !touchingPlayerRight)
                            {
                                rotationVal += Input.GetAxis("Mouse X") * openSpeed;

                            }
                            else if (Input.GetAxis("Mouse X") < 0 && !touchingPlayerLeft)
                            {
                                rotationVal -= -Input.GetAxis("Mouse X") * openSpeed;
                            }
                        }

                    }
                }
                transform.localRotation = Quaternion.Euler(0, rotationVal, 0);


            }
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == player.gameObject)
        {
            Debug.Log("oof");
            Vector3 direction = transform.position - player.position;
            if (xForward)
            {
                relativeAngle = Vector3.Angle(direction, transform.right);
            }
            else if (zForward)
            {
                relativeAngle = Vector3.Angle(direction, transform.forward);
            }

            if (relativeAngle > 180 * 0.5f)
            {
                touchingPlayerRight = true;
            }
            else if (relativeAngle < 180 * 0.5f)
            {
                touchingPlayerLeft = true;
            }

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == player.gameObject)
        {
            Debug.Log("oof2");
            touchingPlayerRight = false;
            touchingPlayerLeft = false;
        }
    }
}
