using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class ObjectOpener : MonoBehaviour
{
    ObjectHolder holder;
    GameObject pressE;
    Transform player;
    Vector3 direction;
    float angleXRelativeToPlayer;
    float angleYRelativeToPlayer;
    bool turnOff;
    bool pressed;

    public UnityEvent FirstObjectInteractionActions;
    public float xAngle;
    public float yAngle;
    public Vector3 rotationOffset;
    void Start()
    {
        holder = GetComponent<ObjectHolder>();
        player = FindObjectOfType<Camera>().transform;
        pressE = GameObject.Find("Canvas").transform.Find("PressE").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.DrawRay(transform.position, -transform.right, Color.green);
        //Debug.DrawRay(transform.position, player.position - transform.position, Color.red);
        //Debug.Log(angleXRelativeToPlayer);
        //Debug.Log(UnityEditor.TransformUtils.GetInspectorRotation(gameObject.transform).x);
        //Debug.Log(Quaternion.Euler(0, 0, 30).z);
        //Debug.Log(transform.localRotation.z);

        if (holder.beingInspected)
        {
            direction = player.position - transform.position;
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
                    holder.enabled = false;
                    StartCoroutine(RotateToNewPosition());
                    
                }
            }
            else if (turnOff)
            {
                pressE.SetActive(false);
                turnOff = false;
            }
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction.normalized + rotationOffset), 0.01f);
        }
        else if (turnOff)
        {
            pressE.SetActive(false);
            turnOff = false;
        }

        IEnumerator RotateToNewPosition()
        {
            while (transform.rotation != Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction.normalized /*+ rotationOffset*/), 0.01f))
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction.normalized)/* + rotationOffset*/, 0.01f);
                //Vector3 posOffset = transform.position - transform.GetComponent<Renderer>().bounds.center;
                //transform.position = Vector3.Lerp(transform.position, player.position + player.forward * holder.distanceFromFace + posOffset, 0.01f);
                yield return null;
            }
            holder.enabled = true;
        }
    }
}
