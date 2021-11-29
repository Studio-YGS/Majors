using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    public CharacterController controller;
    public LayerMask raycastLayerMask;
    [HideInInspector]
    public float baseSpeed;
    public float speed;
    public float sprintSpeed;
    public float stamina = 8;
    public float crouchSpeed;
    public float gravity;
    public float jumpHeight = 3f;
    float z;
    float x;

    //journal variables
    [SerializeField]
    GameObject cursorImage;
    [HideInInspector]
    public bool inJournal = false;
    bool canMove = true;

    public Transform groundCheck;
    private Vector3 groundCheckSpot;
    public float groundDistance = 0.2f;
    public Transform playerBody;

    Vector3 move;
    Vector3 velocity;
    [HideInInspector]
    public bool isGrounded;

    public float mouseSensitivity = 100f;
    public Transform cam;
    private Vector3 camStart;
    private Vector2 rotation = Vector2.zero;

    public float reducedHeight;
    public float originalHeight;

    bool playerHit;
    DoorInteraction contactDoor;
    float contactVal;
    //public bool zForward;
    //public bool zBackwards;
    //public bool xRight;
    //public bool xLeft;
    void Start()
    {
        baseSpeed = speed;

        groundCheckSpot = groundCheck.localPosition;
        camStart = cam.localPosition;

        //locking cursor and making it invisible
        //Cursor.visible = false; //cursor cant be invisible for journal to work :(
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance);

        if (canMove || !isGrounded)
        {


            rotation.y += Input.GetAxis("Mouse X") * mouseSensitivity;
            rotation.x += -Input.GetAxis("Mouse Y") * mouseSensitivity;

            rotation.x = Mathf.Clamp(rotation.x, -90f, 90f);
            playerBody.transform.eulerAngles = new Vector2(0, rotation.y );
            cam.transform.localRotation = Quaternion.Euler(rotation.x , 0, 0);

            x = Input.GetAxis("Horizontal");
            z = Input.GetAxis("Vertical");

            //if (xLeft)
            //{
            //    x = Mathf.Clamp(x, 0, 1);
            //}
            //else if (xRight)
            //{
            //    x = Mathf.Clamp(x, -1, 0);
            //}

            //if (zForward)
            //{
            //    z = Mathf.Clamp(z, -1, 0);
            //}
            //else if (zBackwards)
            //{
            //    z = Mathf.Clamp(z, 0, 1);
            //}

            //if (Input.GetButtonDown("Jump") && isGrounded && canMove)
            //{
            //    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            //}

            velocity.y += gravity * Time.unscaledDeltaTime;
            move = transform.right * x + transform.forward * z;
            controller.Move(velocity * Time.unscaledDeltaTime);

            if (Input.GetKeyDown(KeyCode.C) && isGrounded)
            {
                //makes smol
                controller.height = reducedHeight;
                speed = crouchSpeed;
                groundCheck.localPosition += new Vector3(0, (originalHeight - reducedHeight) / 2, 0);
                cam.localPosition -= new Vector3(0, (originalHeight - reducedHeight) / 2, 0);
            }
            else if (Input.GetKeyUp(KeyCode.C))
            {
                controller.height = originalHeight;
                groundCheck.localPosition = groundCheckSpot;
                cam.localPosition = camStart;
                speed = baseSpeed;
            }


            if (Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.C) && stamina > 0)
            {
                speed = sprintSpeed;
                stamina -= Time.unscaledDeltaTime * 2;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift) && !Input.GetKey(KeyCode.C))
            {
                speed = baseSpeed;
            }

            if(stamina < 8 && !Input.GetKey(KeyCode.LeftShift))
            {
                stamina += Time.unscaledDeltaTime;
            }
            if (stamina <= 0)
            {
                //when the player runs out of stamina
                //FMODUnity.RuntimeManager.PlayOneShot("event:/Character/Out of Stamina/Out of Breath");
                speed = baseSpeed;
            }

            move.Normalize();

            controller.Move(move * speed * Time.unscaledDeltaTime);

            if (isGrounded && velocity.y < 0)
            {

                velocity.y = -2f;
            }
        }

        //journal related stuff
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            JournalController journalController = FindObjectOfType<JournalController>();

            if (!journalController.disabled)
            {
                JournalOnSwitch journal = FindObjectOfType<JournalOnSwitch>();
                bool open = journal.OpenOrClose();

                if (open)
                {
                    DisableController();
                }
                else
                {
                    EnableController();
                }
            }
        }
        //if (Input.GetKeyDown(KeyCode.Escape) && inJournal)
        //{
        //    JournalController journalController = FindObjectOfType<JournalController>();

        //    if (!journalController.disabled)
        //    {
        //        JournalOnSwitch journal = FindObjectOfType<JournalOnSwitch>();

        //        journal.OpenOrClose();
        //        EnableController();
        //    }
        //}
    }
    
    //methods for journal
    public void LockCursor()
    {
        cursorImage.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void UnlockCursor()
    {
        cursorImage.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
    }

    public void EnableController()
    {
        inJournal = false;
        canMove = true;
        LockCursor();
    }

    public void DisableController()
    {
        inJournal = true;
        canMove = false;
        UnlockCursor();
    }

    
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.GetComponent<DoorInteraction>())
        {
            Debug.Log("oof45");
            playerHit = true;
            hit.gameObject.GetComponent<DoorInteraction>().PlayerContact();
            contactDoor = hit.gameObject.GetComponent<DoorInteraction>();
            contactVal = hit.gameObject.GetComponent<DoorInteraction>().rotationVal;
        }
        //else if (playerHit && !hit.gameObject.GetComponent<DoorInteraction>())
        //{
        //    if(z != 0 || x != 0 || contactDoor.rotationVal != contactVal)
        //    {
        //        contactDoor.GetComponent<DoorInteraction>().LostPlayerContact();
        //        playerHit = false;
        //    }
            
        //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<DoorInteraction>())
        {
            Debug.Log("oof45");
            //playerHit = true;
            collision.gameObject.GetComponent<DoorInteraction>().PlayerContact();
            contactDoor = collision.gameObject.GetComponent<DoorInteraction>();
            contactVal = collision.gameObject.GetComponent<DoorInteraction>().rotationVal;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<DoorInteraction>())
        {
            if (z != 0 || x != 0 || contactDoor.rotationVal != contactVal)
            {
                contactDoor.GetComponent<DoorInteraction>().LostPlayerContact();
               playerHit = false;
            }

        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.GetComponent<DoorInteraction>())
    //    {
    //        Debug.Log("door");
    //        if (x > 0)
    //        {
    //            xRight = true;

    //        }
    //        else if (x < 0)
    //        {
    //            xLeft = true;

    //        }

    //        if (z > 0)
    //        {
    //            zForward = true;

    //        }
    //        else if (z < 0)
    //        {
    //            zBackwards = true;

    //        }
    //    }
    //}

    //private void OnCollisionStay(Collision collision)
    //{
    //    if (collision.gameObject.GetComponent<DoorInteraction>())
    //    {
    //        Debug.Log("door2");
    //        if (x > 0)
    //        {
    //            xRight = true;

    //        }
    //        else if (x < 0)
    //        {
    //            xLeft = true;

    //        }

    //        if (z > 0)
    //        {
    //            zForward = true;

    //        }
    //        else if (z < 0)
    //        {
    //            zBackwards = true;

    //        }
    //    }
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.GetComponent<DoorInteraction>())
    //    {

    //        xRight = false;

    //        xLeft = false;

    //        zForward = false;

    //        zBackwards = false;


    //    }
    //}
}
