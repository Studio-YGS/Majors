using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour //this script gets called by unity events to play animations
{
    public void ControlGate(bool open) //this can swing the home gate open or shut depending on the boolean that it gets called with
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

    public void TutorialAnimationsTrue(string conName) //this plays UI prompts for the tutorial, the event just sends the name when it calls it
    {
        Debug.Log("Trying to activate: " + conName);
        GetComponent<Animator>().SetBool(conName, true);
    }

    public void TutorialAnimationsFalse(string conName) //thsi turns the animations back off afterwards
    {
        Debug.Log("Turning off: " + conName);
        GetComponent<Animator>().SetBool(conName, false);
    }

    public void LowerWalls() //this lowers the walls after mr crossy talks to the player
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
