using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using FMOD.Studio;
using FMODUnity;

//Written by Darcy Glover

public class PuzzleController : MonoBehaviour
{
    public GameObject wordObject;

    [SerializeField]
    GameObject[] assignedAltars;

    public string word, currentStreet;

    [SerializeField]
    TextMeshProUGUI mistakeText, streetText;

    EventInstance eventInstance;

    int wordLength, mistakeCount = 3, completedWords = 0;

    public int wordsInPuzzle;

    //[HideInInspector]
    public List<TextMeshProUGUI> canvasLetters = new List<TextMeshProUGUI>();

    public List<GameObject> storedObjects = new List<GameObject>();

    string playersWord, letter, altarName, uiWord;

    public UnityEvent winEvent, loseEvent, tutorialEvent, tutorialMistakeEvent;

    public bool tutorial;

    public WordCollision wordCollision;

    void Start()
    {
        uiWord = " _ _ _ _";

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

        WriteToUI();
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
    public void PlayerWordControl() //this method forms the players word as they place objects, and also controls the win condition
    {
        playersWord = "";
        int playersWordLength;

        for(int i = 0; i < wordLength; i++) //the player's word becomes equal to all the texts within the canvas letters combined
        {
            playersWord += canvasLetters[i].text;
        }

        playersWordLength = playersWord.ToIntArray().Length;
        Debug.Log("Players word: " + playersWord + " is " + playersWordLength + " letters long.");

        storedObjects.Add(GameObject.Find(altarName));

        WriteToUI();

        if (tutorial)
        {
            TutorialController tutorialController = FindObjectOfType<TutorialController>();

            tutorialController.ChangeConLetter(letter);

            eventInstance = RuntimeManager.CreateInstance("event:/MR_C_Tutorial/TUT.0.5");

            eventInstance.start();

            tutorialEvent.Invoke();
            tutorial = false;
        }

        if(playersWord == word) //the script then checks to see if the players formed word is the same as the puzzle's answer
        {
            CompletionCheck();
            wordCollision.puzzleComplete = true;
        }

        if (playersWordLength == wordLength && playersWord != word) //if the player has put all the letters on the altar but hasnt gotten the word right, it counts down a mistake.
        {
            mistakeCount--;
            mistakeText.text = "Mistakes remaining: " + mistakeCount;

            AudioEvents audio = FindObjectOfType<AudioEvents>();

            audio.WordSpeltIncorrectly();

            storedObjects.Clear();

            if (gameObject.name.Contains("Tutorial"))
            {      
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

    void WriteToUI()
    {
        string currentAltarWord = "";

        for(int i = 0; i < canvasLetters.Count; i++)
        {
            if(canvasLetters[i].text.ToIntArray().Length < 1)
            {
                currentAltarWord += " _ ";
            }
            else
            {
                currentAltarWord += canvasLetters[i].text;
            }
        }

        uiWord = currentStreet + ": " + currentAltarWord;

        streetText.text = uiWord;
    }

    void CompletionCheck()
    {
        completedWords++;

        AudioEvents audio = FindObjectOfType<AudioEvents>();

        audio.WordSpeltCorrectly();

        if(storedObjects.Count > 0)
        {
            for (int i = 0; i < storedObjects.Count; i++)
            {
                if (storedObjects[i] != null)
                {
                    if (storedObjects[i].GetComponent<Outline>())
                    {
                        storedObjects[i].GetComponent<Outline>().enabled = false;
                    }
                    if (storedObjects[i].GetComponentInChildren<ObjectPlacement>())
                    {
                        storedObjects[i].GetComponentInChildren<ObjectPlacement>().enabled = false;
                    }
                    if (storedObjects[i].GetComponent<DetermineLetter>().storedObject.GetComponent<ObjectHolder>())
                    {
                        storedObjects[i].GetComponent<DetermineLetter>().storedObject.GetComponent<ObjectHolder>().enabled = false;
                    }
                    if (storedObjects[i].GetComponent<DetermineLetter>().storedObject.GetComponent<Outline>())
                    {
                        storedObjects[i].GetComponent<DetermineLetter>().storedObject.GetComponent<Outline>().enabled = false;
                    }
                }
                    
               
                
            }
        }

        

        storedObjects.Clear();

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
