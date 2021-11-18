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

    public string word, currentStreet;

    public string[] wordsInSection;

    [SerializeField]
    TextMeshProUGUI mistakeText;
    
    [HideInInspector]
    public TextMeshProUGUI streetText;

    EventInstance eventInstance;

    int wordLength, mistakeCount = 3, completedWords = 0, letterPoint;

    public int wordsInPuzzle, section;

    [HideInInspector]
    public List<TextMeshProUGUI> canvasLetters = new List<TextMeshProUGUI>();

    [HideInInspector]
    public List<GameObject> storedObjects = new List<GameObject>();

    string playersWord, letter, altarName, uiWord;

    public bool tutorial;

    public GameObject test;

    public UnityEvent winEvent, loseEvent, tutorialEvent, tutorialMistakeEvent;

    [HideInInspector]
    public WordCollision wordCollision;

    void Start()
    {
        test = GameObject.Find("Journal Open");

        uiWord = " _ _ _ _";

        streetText = GameObject.Find("Street Name With Word").GetComponent<TextMeshProUGUI>();

        if (tutorial)
        {
            SetUpLetters();
        }

        Test();
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
        string[] splitName = altarName.Split('[', ']');

        string firstLength = splitName[0];

        letterPoint = firstLength.ToIntArray().Length;

        canvasLetters[letterPoint].text = letter;

        if(GameObject.Find(altarName).GetComponent<DetermineLetter>().overlappedWord != null)
        {
        }
    }

    void Test()
    {

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

        if (GameObject.Find(altarName) != null)
        {
            storedObjects.Add(GameObject.Find(altarName));
        }

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
            if (wordCollision != null)
            {
                wordCollision.puzzleComplete = true;
            }
        }

        if (playersWordLength == wordLength && playersWord != word) //if the player has put all the letters on the altar but hasnt gotten the word right, it counts down a mistake.
        {
            if (!tutorial)
            {
                mistakeCount--;
                mistakeText.text = "Mistakes remaining: " + mistakeCount;
            }

            AudioEvents audio = FindObjectOfType<AudioEvents>();

            audio.WordSpeltIncorrectly();

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
                storedObjects[i].GetComponent<Outline>().enabled = false;
                storedObjects[i].GetComponentInChildren<ObjectPlacement>().enabled = false;
                storedObjects[i].GetComponent<DetermineLetter>().storedObject.GetComponent<ObjectHolder>().enabled = false;
                storedObjects[i].GetComponent<DetermineLetter>().storedObject.GetComponent<Outline>().enabled = false;
            }
        }

        storedObjects.Clear();

        if(completedWords == wordsInPuzzle)
        {
            winEvent.Invoke();
            Destroy(gameObject); //deleting unused puzzle controllers
        }
    }

    void GameOver()
    {
        loseEvent.Invoke();
    }
}
