using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UI_Street_Events : MonoBehaviour
{

    public UnityEvent Animevent;

    public Animator streets;

    public void AnimEvent()
    {
        streets.SetBool("threeLetter", true);
    }
}
