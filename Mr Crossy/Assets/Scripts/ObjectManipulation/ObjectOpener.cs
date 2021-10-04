using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class ObjectOpener : MonoBehaviour
{
    ObjectHolder holder;
    GameObject pressE;
    Transform player;
    float angleXRelativeToPlayer;
    float angleYRelativeToPlayer;
    bool turnOff;
    bool pressed;

    public UnityEvent FirstObjectInteractionActions;
    public float xAngle;
    public float yAngle;
    void Start()
    {
        holder = GetComponent<ObjectHolder>();
        player = FindObjectOfType<Camera>().transform;
        pressE = GameObject.Find("Canvas").transform.Find("PressE").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, -transform.right, Color.green);
        Debug.DrawRay(transform.position, player.position - transform.position, Color.red);
        //Debug.Log(angleXRelativeToPlayer);
        //Debug.Log(UnityEditor.TransformUtils.GetInspectorRotation(gameObject.transform).x);
        //Debug.Log(Quaternion.Euler(0, 0, 30).z);
        //Debug.Log(transform.localRotation.z);
        if (holder.beingInspected)
        {
            Vector3 direction = player.position - transform.position;
            angleXRelativeToPlayer = Vector3.Angle(direction, -transform.right);
            angleYRelativeToPlayer = Vector3.Angle(direction, transform.up);
            if (angleXRelativeToPlayer < xAngle * 0.5f  /*&& angleYRelativeToPlayer < yAngle * 0.5f*/ &&
                transform.localRotation.x  < Quaternion.Euler(30,0,0).x && transform.localRotation.x  > Quaternion.Euler(-30, 0, 0).x &&
                transform.localRotation.z < Quaternion.Euler(0, 0, 30).z && transform.localRotation.z > Quaternion.Euler(0, 0, -30).z)
            {
                if (!turnOff && !pressed)
                {
                    pressE.SetActive(true);
                    turnOff = true;
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    FirstObjectInteractionActions.Invoke();
                    pressed = true;
                    pressE.SetActive(false);
                }
            }
            else if (turnOff)
            {
                pressE.SetActive(false);
                turnOff = false;
            }
        }
        else if (turnOff)
        {
            pressE.SetActive(false);
            turnOff = false;
        }
    }
}
