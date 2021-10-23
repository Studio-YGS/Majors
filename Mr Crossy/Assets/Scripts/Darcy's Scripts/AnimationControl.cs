using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    TutorialController tutorialController;

    public void OpenGate()
    {
        GetComponent<Animator>().SetBool("OpenGate", true);
    }

    public void TutorialAnimations(string triggerName)
    {
        Debug.Log("Trying to trigger: " + triggerName + " from: " + gameObject.name);
        GetComponent<Animator>().SetTrigger(triggerName);
    }
}
