using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class AudioEvents : MonoBehaviour
{
    EventInstance eventInstance;

    bool incorrectWordVoiceLinePlayed, correctWordVoiceLinePlayed, firstBellToll = true;

    public void WordSpeltCorrectly()
    {
        eventInstance = RuntimeManager.CreateInstance("event:/2D/Puzzle/Correct_Word");

        eventInstance.start();

        if (!correctWordVoiceLinePlayed && !gameObject.name.Contains("NiNi"))
        {
            correctWordVoiceLinePlayed = true;
            eventInstance = RuntimeManager.CreateInstance("event:/MR_C_Tutorial/TUT.0.5.1.1");

            eventInstance.start();
        }
    }

    public void WordSpeltIncorrectly()
    {
        eventInstance = RuntimeManager.CreateInstance("event:/2D/Puzzle/Incorrect_Word");

        eventInstance.start();
    }

    public void TutorialWordIncorrect()
    {
        if (!incorrectWordVoiceLinePlayed)
        {
            incorrectWordVoiceLinePlayed = true;
            eventInstance = RuntimeManager.CreateInstance("event:/MR_C_Tutorial/TUT.0.5.1.2");

            eventInstance.start();
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

    public void BellToll()
    {
        eventInstance = RuntimeManager.CreateInstance("event:/2D/Bell/Bell_Tolls");

        eventInstance.start();

        if (firstBellToll)
        {
            firstBellToll = false;

            eventInstance = RuntimeManager.CreateInstance("event:/MR_C_SolvedPuzzles/I.SP.2_FirstNoticeboard");

            eventInstance.start();
        }
    }

    public void NotePickup()
    {
        eventInstance = RuntimeManager.CreateInstance("event:/MR_C_Tutorial/TUT.0.4.2");

        eventInstance.start();
    }

    public void HoverOver()
    {
        eventInstance = RuntimeManager.CreateInstance("event:/UI_Multimedia/Hover Menu UI");

        eventInstance.start();
    }

    public void UnlockDistrictTwo()
    {
        eventInstance = RuntimeManager.CreateInstance("event:/MR_C_SolvedPuzzles/Unlock_D2_Gate");

        eventInstance.start();
    }

    public void CleanTrophy()
    {
        eventInstance = RuntimeManager.CreateInstance("event:/MR_C_SolvedPuzzles/I.SP.3.1_Trophy");

        eventInstance.start();
    }

    public void SubmissionLine()
    {
        eventInstance = RuntimeManager.CreateInstance("event:/MR_C_SolvedPuzzles/Submission");

        eventInstance.start();
    }
}
