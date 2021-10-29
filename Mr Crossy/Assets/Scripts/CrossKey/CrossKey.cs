using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class CrossKey : MonoBehaviour
{
    public TMP_InputField[] wordOne;
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

        if (numOfLetters == 4)
        {
            if (wordOne[0].text + wordOne[1].text + wordOne[2].text + wordOne[3].text == answer)
            {
                CompletePuzzle();
            }
        }
        if (numOfLetters == 5)
        {
            if (wordOne[0].text + wordOne[1].text + wordOne[2].text + wordOne[3].text + wordOne[4].text == answer)
            {
                CompletePuzzle();
            }
        }

        if (numOfLetters == 6)
        {
            if (wordOne[0].text + wordOne[1].text + wordOne[2].text + wordOne[3].text + wordOne[4].text + wordOne[5].text == answer)
            {
                CompletePuzzle();
            }
        }
        if (numOfLetters == 7)
        {
            if (wordOne[0].text + wordOne[1].text + wordOne[2].text + wordOne[3].text + wordOne[4].text + wordOne[5].text + wordOne[6].text == answer)
            {
                CompletePuzzle();
            }
        }

    }

    void CompletePuzzle()
    {
        //FindObjectOfType<CrossKeyManager>().controller.enabled = true;
        FindObjectOfType<CrossKeyManager>().hintArea.text = "";
        //FindObjectOfType<CrossKeyManager>().headBob.enabled = true;
        //Time.timeScale = 1;
        //Time.fixedDeltaTime = 0.02f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        door.locked = false;
        door.StartCoroutine("RotateCamToOldPosition");
        Destroy(gameObject);
        
    }

    public void valueChanged(TMP_InputField field)
    {
        if (field.text.Length == 1)
        {
            //field.MoveTextEnd(false);
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
