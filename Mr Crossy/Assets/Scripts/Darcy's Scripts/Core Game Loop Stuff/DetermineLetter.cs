using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Written by Darcy Glover

public class DetermineLetter : MonoBehaviour
{
    [HideInInspector]
    public GameObject storedObject;

    PuzzleController puzzleController;

    string firstJLetter, wholeName;

    [HideInInspector]
    public string wordName;

    public void AssignPuzzleController()
    {
        PuzzleController[] puzzleControllers = FindObjectsOfType<PuzzleController>();

        string[] splitName = gameObject.name.Split('[', ']', ',', ' ');

        wordName  = "";

        int count = 0;

        for (int i = 0; i < splitName.Length; i++)
        {
            if (splitName[i] != null)
            {
                wordName += splitName[i];
                count++;
                if (count == 3)
                {
                    break;
                }
            }
        }

        Debug.Log("Combined the split array and have made: " + wordName + " from the altar: " + gameObject.name);

        Debug.Log("Puzzle Controllers array length: " + puzzleControllers.Length);

        for (int i = 0; i < puzzleControllers.Length; i++)
        {
            for (int x = 0; x < puzzleControllers[i].wordObjects.Count; x++)
            {
                if (wordName == puzzleControllers[i].wordObjects[x].name)
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
    }

    public void ObjectPickedUp()
    {
        firstJLetter = ""; //setting the letters back to empty

        SendLetterAndName(firstJLetter);
    }

    void SendLetterAndName(string letter) //sending the letter to the controller, as well as the name of the object it came from
    {
        if(puzzleController == null)
        {
            AssignPuzzleController();
        }

        puzzleController.ReceiveLetterAndName(letter, gameObject.name);
    }
}
