using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Written by Darcy Glover

public class DetermineLetter : MonoBehaviour
{
    [Tooltip("Any altars that this letter overlaps")]
    public GameObject[] overlappedAltars;

    [HideInInspector]
    public GameObject storedObject;

    PuzzleController puzzleController;

    string firstJLetter, wholeName; //the first letter of the the name of the object placed on the altar, and the entire name

    void OnEnable()
    {
        AssignPuzzleController();
    }

    public void AssignPuzzleController()
    {
        PuzzleController[] puzzleControllers = FindObjectsOfType<PuzzleController>();

        string[] splitName = gameObject.name.Split('[', ']', ' ');

        string word = "";

        int count = 0;

        for (int i = 0; i < splitName.Length; i++)
        {
            if (splitName[i] != null)
            {
                word += splitName[i];
                count++;
                if (count == 3)
                {
                    break;
                }
            }
        }

        Debug.Log("Combined the split array and have made: " + word + " from the altar: " + gameObject.name);

        Debug.Log("Puzzle Controllers array length: " + puzzleControllers.Length);

        for (int i = 0; i < puzzleControllers.Length; i++)
        {
            for (int x = 0; x < puzzleControllers[i].wordsInSection.Length; x++)
            {
                if (word == puzzleControllers[i].wordsInSection[x])
                {
                    puzzleController = puzzleControllers[i];
                    Debug.Log("Puzzle Controller for: " + gameObject.name + " is: " + puzzleController.gameObject.name);
                }
            }
        }
    }

    public void ObjectPlaced(GameObject placedObject)
    {
        storedObject = placedObject;
        wholeName = placedObject.name;
        firstJLetter = wholeName.Substring(1, 1);
        SendLetterAndName(firstJLetter);

        if(overlappedAltars.Length > 0)
        {
            for(int i = 0; i < overlappedAltars.Length; i++)
            {
                overlappedAltars[i].GetComponent<OverlappedAltar>().ReceiveLetter(firstJLetter);
            }
        }
    }

    public void ObjectPickedUp()
    {
        firstJLetter = ""; //setting the letters back to empty

        SendLetterAndName(firstJLetter);

        if (overlappedAltars.Length > 0)
        {
            for (int i = 0; i < overlappedAltars.Length; i++)
            {
                overlappedAltars[i].GetComponent<OverlappedAltar>().ReceiveLetter(firstJLetter);
            }
        }
    }

    void SendLetterAndName(string letter) //sending the letter to the controller, as well as the name of the object it came from
    {
        puzzleController.ReceiveLetterAndName(letter, gameObject.name);
    }
}
