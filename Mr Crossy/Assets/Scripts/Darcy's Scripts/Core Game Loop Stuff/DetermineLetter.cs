using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Written by Darcy Glover

public class DetermineLetter : MonoBehaviour
{
    public TextMeshProUGUI assignedLetter;

    [Tooltip("Any altars that this letter overlaps")]
    public GameObject[] overlappedAltars;

    [HideInInspector]
    public GameObject storedObject;

    [SerializeField]
    PuzzleController puzzleController;

    string firstJLetter, wholeName; //the first letter of the the name of the object placed on the altar, and the entire name

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
