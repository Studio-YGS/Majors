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

    //public void PausingControl(bool pause)
    //{
    //    if (pause)
    //    {
    //        animations = FindObjectsOfType<Animator>();

    //        foreach (Animator ani in animations)
    //        {
    //            ani.StopPlayback();
    //        }
    //    }
    //    else
    //    {
    //        animations = FindObjectsOfType<Animator>();

    //        foreach (Animator ani in animations)
    //        {
    //            ani.StartPlayback();
    //        }
    //    }
    //}
}
