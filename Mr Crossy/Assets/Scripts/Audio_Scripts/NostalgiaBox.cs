using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class NostalgiaBox : MonoBehaviour
{
    public StudioEventEmitter nostalgia;
    public StudioEventEmitter homeDrone;
    bool triggered;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "GameController")
        {
            if (!triggered)
            {
                nostalgia.Play();
                homeDrone.Stop();
                triggered = true;
            }

        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "GameController")
        {
            if (other.transform.position.y < transform.position.y)
            {
                nostalgia.Stop();
                homeDrone.Play();
                triggered = false;
            }
        }
    }
}
