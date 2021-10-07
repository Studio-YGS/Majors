using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    public CharacterController controller;
    [HideInInspector]
    public float baseSpeed;
    public float speed;
    public float sprintSpeed;
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

    void Start()
    {
        baseSpeed = speed;

        groundCheckSpot = groundCheck.localPosition;
        camStart = cam.localPosition;

        //locking cursor and making it invisible
        //Cursor.visible = false; //cursor cant be invisible for journal to work :(
        Cursor.lockState = CursorLockMode.Locked;
    }


    void Update()
    {

        if (canMove)
        {

            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance);

            rotation.y += Input.GetAxis("Mouse X");
            rotation.x += -Input.GetAxis("Mouse Y");

            rotation.x = Mathf.Clamp(rotation.x, -30f, 30f);
            playerBody.transform.eulerAngles = new Vector2(0, rotation.y * mouseSensitivity);
            cam.transform.localRotation = Quaternion.Euler(rotation.x * mouseSensitivity, 0, 0);

            x = Input.GetAxis("Horizontal");
            z = Input.GetAxis("Vertical");



            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            }

            velocity.y += gravity * Time.deltaTime;
            move = transform.right * x + transform.forward * z;
            controller.Move(velocity * Time.deltaTime);


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


            if (Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.C))
            {
                speed = sprintSpeed;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift) && !Input.GetKey(KeyCode.C))
            {
                speed = baseSpeed;
            }

            move.Normalize();

            controller.Move(move * speed * Time.deltaTime);

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
        if (Input.GetKeyDown(KeyCode.Escape) && inJournal)
        {
            JournalController journalController = FindObjectOfType<JournalController>();

            if (!journalController.disabled)
            {
                JournalOnSwitch journal = FindObjectOfType<JournalOnSwitch>();

                journal.OpenOrClose();
                EnableController();
            }
        }
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
}
