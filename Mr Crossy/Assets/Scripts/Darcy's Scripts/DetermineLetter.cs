using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Written by Darcy Glover

public class DetermineLetter : MonoBehaviour
{
    [Tooltip("The UI letter that this altar corresponds to")]
    public TextMeshProUGUI assignedLetter;

    [SerializeField, Tooltip("The 'Puzzle Controller' object")]
    GameObject puzzleControl;

    string firstLetter, wholeName; //the first letter of the the name of the object placed on the altar, and the entire name

    public void ObjectPlaced(GameObject placedObject)
    {
        wholeName = placedObject.name;
        firstLetter = wholeName.Substring(0, 1);
        SendLetterAndName(firstLetter);
    }

    public void ObjectPickedUp()
    {
        firstLetter = ""; //setting it back to empty
        SendLetterAndName(firstLetter);
    }

    void SendLetterAndName(string letter) //sending the letter to the controller, as well as the name of the object it came from
    {
        puzzleControl.GetComponent<PuzzleController>().ReceiveLetterAndName(letter, gameObject.name);
    }
}
