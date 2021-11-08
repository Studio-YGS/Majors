using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class AudioEvents : MonoBehaviour
{
    EventInstance eventInstance;

    bool incorrectWordVoiceLinePlayed = false, correctWordVoiceLinePlayed = false;

    public void WordSpeltCorrectly()
    {
        eventInstance = RuntimeManager.CreateInstance("event:/2D/Puzzle/Correct_Word");

        eventInstance.start();

        //if (!correctWordVoiceLinePlayed)
        //{
        //    eventInstance = RuntimeManager.CreateInstance(""); //tutorial line 0.5.1.2 here

        //    eventInstance.start();
        //}
    }

    public void WordSpeltIncorrectly()
    {
        eventInstance = RuntimeManager.CreateInstance("event:/2D/Puzzle/Incorrect_Word");

        eventInstance.start();

        if (!incorrectWordVoiceLinePlayed)
        {
            //eventInstance = RuntimeManager.CreateInstance(""); //tutorial line 0.5.1.1 here

            //eventInstance.start();
        }
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

        Debug.Log("random voice line");
    }

    public void BellToll()
    {
        eventInstance = RuntimeManager.CreateInstance("event:/2D/Bell/Bell_Tolls");

        eventInstance.start();
    }
}
