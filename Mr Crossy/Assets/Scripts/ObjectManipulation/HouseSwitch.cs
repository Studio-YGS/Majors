using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HouseSwitch : MonoBehaviour
{
    public GameObject internals;
    public ExternalInteralSwitch audioSwitch;
    public DoorInteraction door;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "GameController")
        {
            Vector3 direction = transform.position - other.transform.position;
            float angle = Vector3.Angle(direction, transform.forward);
            if (angle > 180 * 0.5f)
            {
                //forward - leaving building
                
            }
            else
            {
                //back - entering building 
                StopCoroutine("Left");
                internals.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "GameController")
        {
            Vector3 direction = transform.position - other.transform.position;
            float angle = Vector3.Angle( direction, transform.forward);
            if (angle > 180 * 0.5f)
            {
                //forward - exiting front (entering)
                audioSwitch.WalkIn();
            }
            else
            {
                //back - exiting back (leaving)
                audioSwitch.WalkOut();
                StartCoroutine("Left");
            }
        }
    }

    IEnumerator Left()
    {
        while (door.moved)
        {
            yield return null;
        }
        internals.SetActive(false);
    }
}
