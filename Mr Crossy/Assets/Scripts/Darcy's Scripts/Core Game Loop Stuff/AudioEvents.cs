using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class AudioEvents : MonoBehaviour
{
    EventInstance eventInstance;

    public void WordSpeltCorrectly()
    {
        eventInstance = RuntimeManager.CreateInstance("event:/2D/Puzzle/Correct_Word");

        eventInstance.start();
    }

    public void WordSpeltIncorrectly()
    {
        eventInstance = RuntimeManager.CreateInstance("event:/2D/Puzzle/Incorrect_Word");

        eventInstance.start();
    }

    public void UIPrompt()
    {
        eventInstance = RuntimeManager.CreateInstance("event:/UI_Multimedia/Tutorial_Info");

        eventInstance.start();
    }

    public void Warning()
    {
        eventInstance = RuntimeManager.CreateInstance("event:/UI_Multimedia/Warning");

        eventInstance.start();
    }

    public void PlayerDie()
    {
        //random voiceline here
    }
}
