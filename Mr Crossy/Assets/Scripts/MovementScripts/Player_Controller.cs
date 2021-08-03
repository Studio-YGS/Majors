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
    float z;
    float x;

    public Transform playerBody;

    Vector3 move;
    [HideInInspector]
    public bool isGrounded;

    public float mouseSensitivity = 100f;
    public Transform cam;
    private Vector2 rotation = Vector2.zero;

    void Start()
    {
        baseSpeed = speed;

        //locking cursor and making it invisible
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }


    void Update()
    {
        

        rotation.y += Input.GetAxis("Mouse X");
        rotation.x += -Input.GetAxis("Mouse Y");

        rotation.x = Mathf.Clamp(rotation.x, -30f, 30f);
        playerBody.transform.eulerAngles = new Vector2(0, rotation.y * mouseSensitivity);
        cam.transform.localRotation = Quaternion.Euler(rotation.x * mouseSensitivity, 0, 0);

        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        move = transform.right * x + transform.forward * z;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = sprintSpeed;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = baseSpeed;
        }

        move.Normalize();

        controller.Move(move * speed * Time.deltaTime);

        

        

    }

    
    
}
