using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using FMODUnity;

[RequireComponent(typeof(Rigidbody))]
public class DoorInteraction : MonoBehaviour
{
    [Header("Canvas")]
    public GameObject reticle;
    public GameObject hand;
    public GameObject lockCheck;
    JournalOnSwitch journal;
    MenuManager menu;
    
    [Header("Door Controls")]
    public bool xForward;
    public bool zForward;
    bool moveable = false;
    [HideInInspector] public bool moved;
    [HideInInspector] public static bool beingMoved;
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
    float openSpeed = 3;
    public float closeDistance = 45;
    //public bool locked;
    bool handon;
    public StudioEventEmitter doorclose;
    Player_Controller controller;
    float startMouseSens;
    public KeyLockDoor keyDoor;

    [Header("Cross-Key Settings")]
    public bool isSafeHouse;
    public bool spawnLeft;
    public bool spawnRight;
    public bool spawnBehind;
    public bool spawnInfront;
    public GameObject mrCrossy;
    [HideInInspector] public GameObject createdMrCrossy;
    Vector3 randomPos;
    Quaternion savedCamRot;
    [HideInInspector] public bool puzzleOn;
    bool puzzleTimer;
    float countdownTimer;
    MrCrossyDistortion distortion;
    CrossKeyManager keyMan;
    
    
    void Start()
    {
        keyMan = FindObjectOfType<CrossKeyManager>();
        cam = GameObject.Find("FirstPersonCharacter").transform;
        rotationVal = 0;
        player = GameObject.Find("Fps Character").transform;
        distortion = FindObjectOfType<MrCrossyDistortion>();
        controller = FindObjectOfType<Player_Controller>();
        startMouseSens = controller.mouseSensitivity;
        journal = FindObjectOfType<JournalOnSwitch>();
        menu = FindObjectOfType<MenuManager>();

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.centerOfMass = Vector3.zero;
        rb.inertiaTensorRotation = Quaternion.identity;
    }

    void Update()
    {
        if (keyMan.doorsLocked && !isSafeHouse && !journal.open && !menu.menuOpen)
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 2, controller.raycastLayerMask))
            {
                if (hit.collider == gameObject.GetComponent<Collider>())
                {
                    if (FindObjectOfType<CrossKeyManager>().numOfKeys > 0)
                    {
                        if (!handon)
                        {
                            handon = true;
                            reticle.SetActive(false);
                            hand.SetActive(true);
                            lockCheck.SetActive(true);
                            lockCheck.GetComponentInChildren<TMP_Text>().text = "Use Crosskey to Unlock";
                        }

                        if (Input.GetMouseButtonDown(0) && !puzzleOn)
                        {
                            FindObjectOfType<Player_Controller>().enabled = false;
                            FindObjectOfType<HeadBob>().enabled = false;
                            reticle.SetActive(false);
                            hand.SetActive(false);
                            lockCheck.SetActive(false);
                            if (CrossyController.crossyTree)
                            {
                                TreeMalarkey.SendEventToTree(CrossyController.crossyTree, "SuperDespawn");
                            }
                            Time.timeScale = 0.05f;
                            Time.fixedDeltaTime = Time.timeScale * 0.02f;

                            

                            if (zForward)
                            {
                                if (spawnBehind)
                                {
                                    randomPos = new Vector3(15, 0, Random.Range(-5, 5));
                                    createdMrCrossy = Instantiate(mrCrossy, transform.position - transform.forward * randomPos.x + transform.right * randomPos.z, Quaternion.LookRotation(player.position - (transform.position - transform.forward * randomPos.x + transform.right * randomPos.z)));

                                }
                                else if (spawnInfront)
                                {
                                    randomPos = new Vector3(15, 0, Random.Range(-5, 5));
                                    createdMrCrossy = Instantiate(mrCrossy, transform.position + transform.forward * randomPos.x + transform.right * randomPos.z, Quaternion.LookRotation(player.position - (transform.position - transform.forward * randomPos.x + transform.right * randomPos.z)));

                                }
                                else if (spawnLeft)
                                {
                                    randomPos = new Vector3(Random.Range(3, 6), 0, -15);
                                    createdMrCrossy = Instantiate(mrCrossy, transform.position - transform.forward * randomPos.x + transform.right * randomPos.z, Quaternion.LookRotation(player.position - (transform.position - transform.forward * randomPos.x + transform.right * randomPos.z)));
                                }
                                else if (spawnRight)
                                {
                                    randomPos = new Vector3(Random.Range(3, 6), 0, 15);
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
                                else if (spawnInfront)
                                {
                                    randomPos = new Vector3(Random.Range(-5, 5), 0, 15);
                                    createdMrCrossy = Instantiate(mrCrossy, transform.position + transform.forward * randomPos.x - transform.right * randomPos.z, Quaternion.LookRotation(player.position - (transform.position + transform.forward * randomPos.x + transform.right * randomPos.z)));

                                }
                                else if (spawnLeft)
                                {
                                    randomPos = new Vector3(-15, 0, Random.Range(3, 6));
                                    createdMrCrossy = Instantiate(mrCrossy, transform.position + transform.forward * randomPos.x + transform.right * randomPos.z, Quaternion.LookRotation(player.position - (transform.position + transform.forward * randomPos.x + transform.right * randomPos.z)));
                                }
                                else if (spawnRight)
                                {
                                    randomPos = new Vector3(15, 0, Random.Range(3, 6));
                                    createdMrCrossy = Instantiate(mrCrossy, transform.position + transform.forward * randomPos.x + transform.right * randomPos.z, Quaternion.LookRotation(player.position - (transform.position + transform.forward * randomPos.x + transform.right * randomPos.z)));
                                }
                            }
                            puzzleOn = true;
                            savedCamRot = cam.rotation;
                            StartCoroutine(RotateCamToNewPosition());
                            createdMrCrossy.GetComponent<NavMeshAgent>().SetDestination(player.position);
                            createdMrCrossy.GetComponent<CrossyCrossKeyVariant>().door = gameObject.GetComponent<DoorInteraction>();
                            distortion.ReduceInsanity();
                        }
                    }
                    else
                    {
                        if (!handon)
                        {
                            handon = true;
                            lockCheck.SetActive(true);
                            lockCheck.GetComponentInChildren<TMP_Text>().text = "It's locked";
                        }
                    }


                }
                else if (handon)
                {
                    reticle.SetActive(true);
                    hand.SetActive(false);
                    handon = false;
                    lockCheck.SetActive(false);
                    lockCheck.GetComponentInChildren<TMP_Text>().text = "";
                }
            }
            else if (handon)
            {
                reticle.SetActive(true);
                hand.SetActive(false);
                handon = false;
                lockCheck.SetActive(false);
                lockCheck.GetComponentInChildren<TMP_Text>().text = "";
            }

        }

        

        if (puzzleOn)
        {
            if (createdMrCrossy)
            {
                createdMrCrossy.GetComponent<NavMeshAgent>().SetDestination(player.position);
            }
            
            if (moved)
            {
                if (zForward)
                {
                    if (angleRelativeToPlayer > 180 * 0.5f)
                    {
                        if (greaterThan)
                        {
                            if (rotationVal <= 5)
                            {
                                moveable = false;
                                //Destroy(createdMrCrossy);
                                rotationVal = 0;
                                transform.localRotation = Quaternion.Euler(0, rotationVal, 0);
                                puzzleOn = false;
                                doorclose.Play();
                                StartCoroutine("WaitForPuzzleEnd");
                                moved = false;
                            }
                        }
                        else if (lessThan)
                        {
                            if (rotationVal >= 5)
                            {
                                moveable = false;
                                //Destroy(createdMrCrossy);
                                rotationVal = 0;
                                transform.localRotation = Quaternion.Euler(0, rotationVal, 0);
                                puzzleOn = false;
                                doorclose.Play();
                                StartCoroutine("WaitForPuzzleEnd");
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
                            if (rotationVal <= 5)
                            {
                                moveable = false;
                                //Destroy(createdMrCrossy);
                                rotationVal = 0;
                                transform.localRotation = Quaternion.Euler(0, rotationVal, 0);
                                puzzleOn = false;
                                StartCoroutine("WaitForPuzzleEnd");
                                moved = false;
                            }
                        }
                        else if (lessThan)
                        {
                            if (rotationVal >= -5)
                            {
                                moveable = false;
                                //Destroy(createdMrCrossy);
                                rotationVal = 0;
                                transform.localRotation = Quaternion.Euler(0, rotationVal, 0);
                                puzzleOn = false;
                                StartCoroutine("WaitForPuzzleEnd");
                                moved = false;
                            }
                        }
                    }
                    
                }
            }
            
            if(Vector3.Distance(transform.position, player.position) > 10)
            {
                if (createdMrCrossy)
                {
                    FindObjectOfType<CrossKeyManager>().PuzzleDeath(createdMrCrossy);
                }
                
                puzzleTimer = false;
            }
            
        }
        if (puzzleTimer /*&& createdMrCrossy*/)
        {
            if (createdMrCrossy)
            {
                if (zForward)
                {
                    createdMrCrossy.GetComponent<NavMeshAgent>().SetDestination(transform.position - transform.forward + transform.right * 0.5f);
                }
                else if (xForward)
                {
                    createdMrCrossy.GetComponent<NavMeshAgent>().SetDestination(transform.position + transform.right);
                }
                FindObjectOfType<CrossyCrossKeyVariant>().lookAtTransform = transform;
            }
            
            
            if (moved)
            {
                if (rotationVal > 5 && rotationVal < 20 || rotationVal > -20 && rotationVal < -5)
                {
                    
                    //countdownTimer += Time.deltaTime;
                    if(distortion.colorAdjustments[0].colorFilter.value == Color.black)
                    {
                        Debug.Log("death-intensity");
                        FindObjectOfType<CrossKeyManager>().PuzzleDeath(createdMrCrossy);
                        
                        StopCoroutine("WaitForPuzzleEnd");
                        puzzleTimer = false;
                    }
                }
                //else
                //{
                //    countdownTimer = 0;
                //}
                
                if(rotationVal > 20 || rotationVal < -20)
                {
                    FindObjectOfType<CrossKeyManager>().PuzzleDeath(createdMrCrossy);
                    
                    StopCoroutine("WaitForPuzzleEnd");
                    puzzleTimer = false;
                }
            }
        }

        if (!keyMan.doorsLocked && keyDoor == null && !journal.open && !menu.menuOpen || isSafeHouse && keyDoor == null && !journal.open && !menu.menuOpen)
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 2, controller.raycastLayerMask))
            {
                if (hit.collider == gameObject.GetComponent<Collider>())
                {
                    handon = true;
                    reticle.SetActive(false);
                    hand.SetActive(true);
                    if (Input.GetMouseButtonDown(0) && !ObjectHolder.objectBeingInspected)
                    {
                        moveable = true;
                        beingMoved = true;
                        controller.mouseSensitivity = controller.mouseSensitivity / 2;
                        if (moved)
                        {
                            StopCoroutine("WaitToClose");
                            StartCoroutine("WaitToClose");
                        }
                        if (!moved)
                        {
                            StartCoroutine("WaitToClose");
                            if (isSafeHouse)
                            {
                                FMODUnity.RuntimeManager.PlayOneShot("event:/3D/Doors/Surburban/Door Open");
                            }
                            else
                            {
                                FMODUnity.RuntimeManager.PlayOneShot("event:/3D/Doors/Calekirk Doors/Door Open");
                            }

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
                else if (handon)
                {
                    reticle.SetActive(true);
                    hand.SetActive(false);
                    handon = false;
                }
            }
            else if (handon)
            {
                reticle.SetActive(true);
                hand.SetActive(false);
                handon = false;
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
                controller.mouseSensitivity = startMouseSens;
                beingMoved = false;
                moveable = false;
            }


            if (moveable && !journal.open && !menu.menuOpen)
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

    //private void OnTriggerstay(Collider other)
    //{
    //    if(other.tag == "GameController")
    //    {
    //        Debug.Log("triggers");
    //        Vector3 direction = transform.position - player.position;
    //        if (xForward)
    //        {
    //            relativeAngle = Vector3.Angle(direction, transform.right);
    //        }
    //        else if (zForward)
    //        {
    //            relativeAngle = Vector3.Angle(direction, transform.forward);
    //        }

    //        if (relativeAngle > 180 * 0.5f)
    //        {
                
    //            if (gameObject.GetComponent<Collider>().bounds.Contains(other.transform.position))
    //            {
    //                rotationVal += (Vector3.Distance(player.GetComponent<Collider>().bounds.extents, transform.position) + 0.5f);
    //            }
                
    //        }
    //        else if (relativeAngle < 180 * 0.5f)
    //        {
                
    //            if (gameObject.GetComponent<Collider>().bounds.Contains(other.transform.position))
    //            {
    //                rotationVal -= (Vector3.Distance(player.GetComponent<Collider>().bounds.extents, transform.position) + 0.5f);
    //            }
                    
    //        }

            
    //    }
    //}
    //private void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    if(hit.gameObject == player.gameObject)
    //    {
    //        Debug.Log("player");
    //    }
    //}
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
                //rotationVal += 7;
            }
            else if (relativeAngle < 180 * 0.5f)
            {
                touchingPlayerLeft = true;
                //rotationVal -= 7;
            }

            

        }
    }

    public void PlayerContact()
    {
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

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == player.gameObject)
        {
            Debug.Log("oof2");
            touchingPlayerRight = false;
            touchingPlayerLeft = false;
        }
    }

    public void LostPlayerContact()
    {
        touchingPlayerRight = false;
        touchingPlayerLeft = false;
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
        if (doorclose)
        {
            doorclose.Play();
            //jack the poopoohead
        }

        rotationVal = 0;
        greaterThan = false;
        lessThan = false;
        equalTo = true;
        moved = false;
    }

    IEnumerator RotateCamToNewPosition()
    {
        
        while (cam.rotation != Quaternion.Slerp(cam.rotation, Quaternion.LookRotation(createdMrCrossy.transform.position - cam.position), 0.04f))
        {
            cam.rotation = Quaternion.Slerp(cam.rotation, Quaternion.LookRotation(createdMrCrossy.transform.position - cam.position), 0.04f);

            yield return null;
        }
        FindObjectOfType<CrossKeyManager>().StartCrossKeyPuzzle(gameObject.GetComponent<DoorInteraction>());
    }

    public IEnumerator RotateCamToOldPosition()
    {
        while (cam.rotation != savedCamRot)
        {
            cam.rotation = Quaternion.Slerp(cam.rotation, savedCamRot, 0.3f);

            yield return null;
        }
        FindObjectOfType<CrossKeyManager>().headBob.enabled = true;
        FindObjectOfType<CrossKeyManager>().controller.enabled = true;
        StartCoroutine("ReturnTimeScale");
    }

    public IEnumerator ReturnTimeScale()
    {
        while (Time.timeScale <= 0.99f)
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, 1, 0.005f);
            Time.fixedDeltaTime = Time.timeScale * 0.02f;

            yield return null;
        }
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f;
    }

    IEnumerator WaitForPuzzleEnd()
    {
        puzzleTimer = true;
        float timer = 0;
        while (timer <= Random.Range(10, 20))
        {
            
            if (rotationVal < 5 && rotationVal > -5)
            {
                timer += Time.deltaTime;
                
                distortion.DecreaseVignette();
            }
            else
            {
                timer = 0;
                
                distortion.IncreaseVignette();
            }
            yield return null;
        }
        
        
        
        puzzleTimer = false;
        FindObjectOfType<CrossKeyManager>().puzzleOn = false;
        distortion.ReduceInsanity();
        if (createdMrCrossy)
        {
            Destroy(createdMrCrossy);
        }
        
        keyMan.doorsLocked = false;
        Debug.Log("safe");
    }
}
