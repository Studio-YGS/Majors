using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

//Written by Darcy Glover

public class PuzzleController : MonoBehaviour
{
    public GameObject wordObject;

    [SerializeField]
    GameObject[] assignedAltars;

    public string word;

    [SerializeField]
    TextMeshProUGUI mistakeText;

    int wordLength, mistakeCount = 3, completedWords = 0;

    public int wordsInPuzzle;

    //[HideInInspector]
    public List<TextMeshProUGUI> canvasLetters = new List<TextMeshProUGUI>();

    public List<GameObject> storedObjects = new List<GameObject>();

    string playersWord, letter, altarName;

    public UnityEvent winEvent, loseEvent, tutorialEvent, tutorialMistakeEvent;

    public bool tutorial;

    void Start()
    {
        if (tutorial)
        {
            SetUpLetters();
        }
    }

    public void SetUpLetters()
    {
        canvasLetters.Clear();

        foreach (TextMeshProUGUI tmp in wordObject.GetComponentsInChildren<TextMeshProUGUI>())
        {
            canvasLetters.Add(tmp);
        }

        wordLength = canvasLetters.Count;
    }

    public void ReceiveLetterAndName(string firstLetter, string altarOrigName) //receiving the letter of the object, and the name of the altar it came from
    {
        letter = firstLetter;
        altarName = altarOrigName;
        DisplayLetter();
        PlayerWordControl();
    }

    void DisplayLetter() 
    {
        DetermineLetter letterSend = GameObject.Find(altarName).GetComponent<DetermineLetter>();

        letterSend.assignedLetter.text = letter;

        if(letterSend.overlappedAltars.Length > 0)
        {
            for(int i = 0; i < letterSend.overlappedAltars.Length; i++)
            {
                letterSend.overlappedAltars[i].GetComponent<OverlappedAltar>().assignedLetter.text = letter;
            }
        }
    }

    void PlayerWordControl() //this method forms the players word as they place objects, and also controls the win condition
    {
        playersWord = "";
        int playersWordLength;

        for(int i = 0; i < wordLength; i++) //the player's word becomes equal to all the texts within the canvas letters combined
        {
            playersWord += canvasLetters[i].text;
        }

        playersWordLength = playersWord.ToIntArray().Length;
        Debug.Log("Players word: " + playersWord + " is " + playersWordLength + " long.");

        storedObjects.Add(GameObject.Find(altarName));

        if (tutorial)
        {
            TutorialController tutorialController = FindObjectOfType<TutorialController>();

            tutorialController.ChangeConLetter(letter);

            tutorialEvent.Invoke();
            tutorial = false;
        }

        if(playersWord == word) //the script then checks to see if the players formed word is the same as the puzzle's answer
        {
            CompletionCheck();
        }

        if (playersWordLength == wordLength && playersWord != word) //if the player has put all the letters on the altar but hasnt gotten the word right, it counts down a mistake.
        {
            mistakeCount--;
            mistakeText.text = "Mistakes remaining: " + mistakeCount;

            storedObjects.Clear();

            if (gameObject.name.Contains("Tutorial"))
            {
                Debug.Log("TUTORIAL MISTAKE");
                tutorialMistakeEvent.Invoke();
            }

            if(mistakeCount == 0)
            {
                GameOver();
            }
        }
    }

    public void DisableAltars()
    {
        for(int i = 0; i < assignedAltars.Length; i++)
        {
            assignedAltars[i].GetComponentInChildren<ObjectPlacement>().enabled = false;
        }
    }

    void CompletionCheck()
    {
        completedWords++;

        for(int i = 0; i < storedObjects.Count; i++)
        {
            storedObjects[i].GetComponent<Outline>().enabled = false;
            storedObjects[i].GetComponentInChildren<ObjectPlacement>().enabled = false;
            storedObjects[i].GetComponent<DetermineLetter>().storedObject.GetComponent<ObjectHolder>().enabled = false;
            storedObjects[i].GetComponent<DetermineLetter>().storedObject.GetComponent<Outline>().enabled = false;
        }

        storedObjects.Clear();

        if (completedWords == 3)
        {
            TutorialSectionStart tutorialSectionStart = GetComponent<TutorialSectionStart>();

            tutorialSectionStart.TutorialComplete();
        }

        if(completedWords == wordsInPuzzle)
        {
            winEvent.Invoke();
        }
    }

    void GameOver()
    {
        loseEvent.Invoke();
    }
}
