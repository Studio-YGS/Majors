using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OverlappedAltar : MonoBehaviour
{
    public TextMeshProUGUI assignedLetter;

    PuzzleController puzzleController;

    string letter;
 
    public void ReceiveLetter(string firstLetter)
    {
        letter = firstLetter;
        //SendLetterAndName();
    }

    void SendLetterAndName()
    {
        puzzleController = FindObjectOfType<PuzzleController>();

        puzzleController.ReceiveLetterAndName(letter, gameObject.name);
    }
}
