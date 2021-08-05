using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Written by Darcy Glover

public class DetermineLetter : MonoBehaviour
{
    [Tooltip("The UI letter that this altar corresponds to")]
    public Text assignedLetter;

    [SerializeField, Tooltip("The 'Puzzle Controller' object")]
    GameObject puzzleControl;

    string firstLetter, wholeName; //the first letter of the the name of the object placed on the altar, and the entire name

    bool hasAnObject = false; //this is to make sure there is only one object being detected at once, the first one placed on the altar

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Placeable Object" && !hasAnObject)
        {
            hasAnObject = true;
            wholeName = other.name;
            firstLetter = wholeName.Substring(0, 1);
            SendLetterAndName(firstLetter);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.name == wholeName) //if the object that is leaving has the same name as the first object placed, it will revert the bool
        {
            hasAnObject = false;
            firstLetter = "_"; //setting it back to 'empty'
            SendLetterAndName(firstLetter); 
        }
    }

    void SendLetterAndName(string letter) //sending the letter to the controller, as well as the name of the object it came from
    {
        puzzleControl.GetComponent<WordControl>().ReceiveLetterAndName(letter, gameObject.name);
    }
}
