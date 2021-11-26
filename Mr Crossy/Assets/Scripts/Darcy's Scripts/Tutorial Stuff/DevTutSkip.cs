using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DevTutSkip : MonoBehaviour
{
    public UnityEvent skipTutorial;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O) && Input.GetKeyDown(KeyCode.K) && Input.GetKeyDown(KeyCode.F))
        {
            skipTutorial.Invoke();
        }
    }
}
