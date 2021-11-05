using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    public void OpenGate()
    {
        GetComponent<Animator>().SetBool("OpenGate", true);
    }

    public void TutorialAnimationsTrue(string conName)
    {
        Debug.Log("Trying to activate: " + conName);
        GetComponent<Animator>().SetBool(conName, true);
        //play sound here
    }

    public void TutorialAnimationsFalse(string conName)
    {
        Debug.Log("Turning off: " + conName);
        GetComponent<Animator>().SetBool(conName, false);
    }

    public void LowerWalls()
    {
        Debug.Log("Lowering Walls");
        GetComponent<Animator>().SetBool("WallDown", true);
    }
}
