using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Written by Darcy Glover

public class DetermineLetter : MonoBehaviour
{
    [Tooltip("The UI letter that this altar corresponds to")]
    public TextMeshProUGUI assignedLetter;

    [Tooltip("Any altars that this letter overlaps")]
    public GameObject[] overlappedAltars;

    PuzzleController puzzleController;

    string firstLetter, wholeName; //the first letter of the the name of the object placed on the altar, and the entire name

    public void ObjectPlaced(GameObject placedObject)
    {
        wholeName = placedObject.name;
        firstLetter = wholeName.Substring(0, 1);
        SendLetterAndName(firstLetter);

        if(overlappedAltars.Length > 0)
        {
            for(int i = 0; i < overlappedAltars.Length; i++)
            {
                overlappedAltars[i].GetComponent<OverlappedAltar>().ReceiveLetter(firstLetter);
            }
        }
    }

    public void ObjectPickedUp()
    {
        firstLetter = ""; //setting it back to empty
        SendLetterAndName(firstLetter);

        if (overlappedAltars.Length > 0)
        {
            for (int i = 0; i < overlappedAltars.Length; i++)
            {

            }
        }
    }

    void SendLetterAndName(string letter) //sending the letter to the controller, as well as the name of the object it came from
    {
        puzzleController = FindObjectOfType<PuzzleController>();

        puzzleController.ReceiveLetterAndName(letter, gameObject.name);
    }
}
