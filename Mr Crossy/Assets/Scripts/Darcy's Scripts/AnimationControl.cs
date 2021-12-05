using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    public void ControlGate(bool open)
    {
        if (open)
        {
            GetComponent<Animator>().SetBool("OpenGate", open);
        }
        else
        {
            GetComponent<Animator>().SetBool("OpenGate", open);
        }
    }

    public void TutorialAnimationsTrue(string conName)
    {
        Debug.Log("Trying to activate: " + conName);
        GetComponent<Animator>().SetBool(conName, true);
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

    public void PauseGame()
    {
        Time.timeScale = 0f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    public void PlayGame()
    {
        Time.timeScale = 0.05f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }
}
