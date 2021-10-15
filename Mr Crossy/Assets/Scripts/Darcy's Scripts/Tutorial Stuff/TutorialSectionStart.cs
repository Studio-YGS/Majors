using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialSectionStart : MonoBehaviour
{
    public UnityEvent sectionStart, steppingAway;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GameController"))
        {
            sectionStart.Invoke();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("GameController"))
        {
            steppingAway.Invoke();
        }
    }
}
