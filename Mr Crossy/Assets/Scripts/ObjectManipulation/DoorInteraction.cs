using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public class DoorInteraction : MonoBehaviour
{
    [Header("Door Controls")]
    public bool xForward;
    public bool zForward;
    bool moveable = false;
    bool moved;
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
    public float closeDistance = 45;
    public bool locked;

    [Header("Cross-Key Settings")]
    public bool spawnLeft;
    public bool spawnRight;
    public bool spawnBehind;
    public GameObject mrCrossy;
    GameObject createdMrCrossy;
    Vector3 randomPos;
    Quaternion savedCamRot;
    bool puzzleOn;
    
    
    void Start()
    {
        cam = GameObject.Find("FirstPersonCharacter").transform;
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
                        FindObjectOfType<Player_Controller>().enabled = false;
                        FindObjectOfType<HeadBob>().enabled = false;
                        puzzleOn = true;
                        Time.timeScale = 0.1f;
                        Time.fixedDeltaTime = Time.timeScale * 0.02f;
                        if (zForward)
                        {
                            if (spawnBehind)
                            {
                                randomPos = new Vector3(15, 0, Random.Range(-5, 5));
                                createdMrCrossy = Instantiate(mrCrossy, transform.position - transform.forward * randomPos.x +transform.right * randomPos.z , Quaternion.LookRotation(player.position - (transform.position - transform.forward * randomPos.x + transform.right * randomPos.z)));

                            }
                            else if (spawnLeft)
                            {
                                randomPos = new Vector3(Random.Range(0, 5), 0, -15);
                                createdMrCrossy = Instantiate(mrCrossy, transform.position - transform.forward * randomPos.x + transform.right * randomPos.z, Quaternion.LookRotation(player.position - (transform.position - transform.forward * randomPos.x + transform.right * randomPos.z)));
                            }
                            else if (spawnRight)
                            {
                                randomPos = new Vector3(Random.Range(0, 5), 0, 15);
                                createdMrCrossy = Instantiate(mrCrossy, transform.position - transform.forward * randomPos.x + transform.right * randomPos.z, Quaternion.LookRotation(player.position - (transform.position - transform.forward * randomPos.x + transform.right * randomPos.z)));
                            }
                        }
                        else if (xForward)
                        {
                            if (spawnBehind)
                            {
                                randomPos = new Vector3(Random.Range(-5, 5), 0, 15);
                                createdMrCrossy = Instantiate(mrCrossy, transform.position + transform.forward * randomPos.x + transform.right * randomPos.z, Quaternion.LookRotation(player.position - (transform.position + transform.forward * randomPos.x + transform.right * randomPos.z)));

                            }
                            else if (spawnLeft)
                            {
                                randomPos = new Vector3(-15, 0, Random.Range(0, 5));
                                createdMrCrossy = Instantiate(mrCrossy, transform.position + transform.forward * randomPos.x + transform.right * randomPos.z, Quaternion.LookRotation(player.position - (transform.position + transform.forward * randomPos.x + transform.right * randomPos.z)));
                            }
                            else if (spawnRight)
                            {
                                randomPos = new Vector3(15, 0, Random.Range(0, 5));
                                createdMrCrossy = Instantiate(mrCrossy, transform.position + transform.forward * randomPos.x + transform.right * randomPos.z, Quaternion.LookRotation(player.position - (transform.position + transform.forward * randomPos.x + transform.right * randomPos.z)));
                            }
                        }
                        savedCamRot = cam.rotation;
                        StartCoroutine(RotateCamToNewPosition());
                        createdMrCrossy.GetComponent<NavMeshAgent>().SetDestination(player.position);
                    }
                }
            }
        }

        if (puzzleOn && moved)
        {
            if (zForward)
            {
                if (angleRelativeToPlayer > 180 * 0.5f)
                {
                    if (greaterThan)
                    {
                        if (rotationVal <= 0)
                        {
                            moveable = false;
                            Destroy(createdMrCrossy);
                            rotationVal = 0;
                            transform.localRotation = Quaternion.Euler(0, rotationVal, 0);
                            puzzleOn = false;
                            moved = false;
                        }
                    }
                    else if (lessThan)
                    {
                        if (rotationVal >= 0)
                        {
                            moveable = false;
                            Destroy(createdMrCrossy);
                            rotationVal = 0;
                            transform.localRotation = Quaternion.Euler(0, rotationVal, 0);
                            puzzleOn = false;
                            moved = false;
                        }
                    }
                }
            }
            else if (xForward)
            {
                if (angleRelativeToPlayer < 180 * 0.5f)
                {
                    if (greaterThan)
                    {
                        if (rotationVal <= 0)
                        {
                            moveable = false;
                            Destroy(createdMrCrossy);
                            rotationVal = 0;
                            transform.localRotation = Quaternion.Euler(0, rotationVal, 0);
                            puzzleOn = false;
                            moved = false;
                        }
                    }
                    else if (lessThan)
                    {
                        if (rotationVal >= 0)
                        {
                            moveable = false;
                            Destroy(createdMrCrossy);
                            rotationVal = 0;
                            transform.localRotation = Quaternion.Euler(0, rotationVal, 0);
                            puzzleOn = false;
                            moved = false;
                        }
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
                        if (!moved)
                        {
                            StartCoroutine(WaitToClose());
                            moved = true;
                        }
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

    IEnumerator WaitToClose()
    {
        while(Vector3.Distance(transform.position, player.position) < closeDistance )
        {
            yield return null;
        }
        while (transform.localRotation != Quaternion.identity)
        {

            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, Time.deltaTime * 2f);

            yield return null;
        }
        rotationVal = 0;
        greaterThan = false;
        lessThan = false;
        equalTo = true;
        moved = false;
    }

    IEnumerator RotateCamToNewPosition()
    {
        
        while (cam.rotation != Quaternion.Slerp(cam.rotation, Quaternion.LookRotation(createdMrCrossy.transform.position - cam.position), 0.005f))
        {
            cam.rotation = Quaternion.Slerp(cam.rotation, Quaternion.LookRotation(createdMrCrossy.transform.position - cam.position), 0.005f);

            yield return null;
        }
        FindObjectOfType<CrossKeyManager>().StartCrossKeyPuzzle(gameObject.GetComponent<DoorInteraction>());
    }

    public IEnumerator RotateCamToOldPosition()
    {
        while (cam.rotation != savedCamRot)
        {
            cam.rotation = Quaternion.Slerp(cam.rotation, savedCamRot, 0.025f);

            yield return null;
        }
        FindObjectOfType<CrossKeyManager>().headBob.enabled = true;
        FindObjectOfType<CrossKeyManager>().controller.enabled = true;
        StartCoroutine("ReturnTimeScale");
    }

    public IEnumerator ReturnTimeScale()
    {
        while (Time.timeScale <= 0.9f)
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, 1, 0.005f);
            Time.fixedDeltaTime = Time.timeScale * 0.02f;

            yield return null;
        }
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f;
    }
}
