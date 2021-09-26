using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Written by Darcy Glover

//Use this script to manage or control any of the animations that we need to play in the game. Just simply add a method that does what you need it to and reference it within your script, and then add your name to the top

public class AnimationControl : MonoBehaviour
{
    public void OpenGate()
    {
        GetComponent<Animator>().SetBool("OpenGate", true);
    }
}
