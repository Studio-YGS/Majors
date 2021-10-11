using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CrossKey : MonoBehaviour
{
    public TMP_InputField wordOne;
    public TMP_Text[] toothOneFront;
    public TMP_Text[] toothTwoFront;
    public TMP_Text[] toothOneBack;
    public TMP_Text[] toothTwoBack;
    [HideInInspector] public string answer;
    public int numOfLetters;
    Transform cam;
    [HideInInspector] public DoorInteraction door;
    void Start()
    {
        cam = FindObjectOfType<Camera>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            float rotX = Input.GetAxis("Mouse X") * 200 * Mathf.Deg2Rad;
            float rotY = Input.GetAxis("Mouse Y") * 200 * Mathf.Deg2Rad;
            transform.RotateAround(transform.GetComponent<Renderer>().bounds.center, cam.up, -rotX);
            transform.RotateAround(transform.GetComponent<Renderer>().bounds.center, cam.right, rotY);
        }

        //if(numOfLetters == 6)
        //{
            if(wordOne.text == answer)
            {
                Debug.Log("unlock");
                FindObjectOfType<CrossKeyManager>().controller.enabled = true;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                door.locked = false;
                Destroy(gameObject);
            }
        //}

    }
}
