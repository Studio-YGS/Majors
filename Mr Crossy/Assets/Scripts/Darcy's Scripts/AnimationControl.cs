using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    Animator[] animations;

    public void OpenGate()
    {
        GetComponent<Animator>().SetBool("OpenGate", true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)) //dev tool
        {
            GetComponent<Animator>().SetBool("OpenGate", true);
        }
    }
}
