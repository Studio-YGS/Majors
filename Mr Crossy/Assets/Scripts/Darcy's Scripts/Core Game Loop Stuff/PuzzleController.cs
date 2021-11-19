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
    public string word, currentStreet;

    [SerializeField]
    TextMeshProUGUI mistakeText;
    
    [HideInInspector]
    public TextMeshProUGUI streetText;

    EventInstance eventInstance;

    int wordLength, mistakeCount = 3, completedWords = 0, letterPoint;

    public int wordsInPuzzle, section;

    //[HideInInspector]
    public List<TextMeshProUGUI> canvasLetters = new List<TextMeshProUGUI>();

    //[HideInInspector]
    public List<GameObject> storedObjects = new List<GameObject>();
    public List<GameObject> wordObjects = new List<GameObject>();

    string playersWord, letter, altarName, uiWord;

    public bool tutorial;

    public UnityEvent winEvent, loseEvent, tutorialEvent, tutorialMistakeEvent;

    [HideInInspector]
    public WordCollision wordCollision;

    void Start()
    {
        uiWord = " _ _ _ _";

        streetText = GameObject.Find("Street Name With Word").GetComponent<TextMeshProUGUI>();

        if (tutorial)
        {
            SetUpLetters(0);
        }
    }

    public void SetUpLetters(int whichObject)
    {
        canvasLetters.Clear();

        foreach (TextMeshProUGUI tmp in wordObjects[whichObject].GetComponentsInChildren<TextMeshProUGUI>())
        {
            canvasLetters.Add(tmp);
        }

        wordLength = canvasLetters.Count;

        if (word != wordObjects[whichObject].name)
        {
            WriteToUI();
        }
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

        for (int i = 0; i < wordObjects.Count; i++)
        {
            if (wordObjects[i].name == GameObject.Find(altarName).GetComponent<DetermineLetter>().wordName)
            {
                SetUpLetters(i);
                break;
            }
        }

        letterPoint = firstLength.ToIntArray().Length;

        canvasLetters[letterPoint].text = letter;
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
                if (storedObjects[i].GetComponent<Outline>())
                {
                    storedObjects[i].GetComponent<Outline>().enabled = false;
                    storedObjects[i].GetComponentInChildren<ObjectPlacement>().enabled = false;
                    storedObjects[i].GetComponent<DetermineLetter>().storedObject.GetComponent<ObjectHolder>().enabled = false;
                    storedObjects[i].GetComponent<DetermineLetter>().storedObject.GetComponent<Outline>().enabled = false;
                }
                else if (storedObjects[i].GetComponentInParent<OverlappedAltar>())
                {
                    storedObjects[i].GetComponentInParent<Outline>().enabled = false;
                    storedObjects[i].GetComponentInParent<OverlappedAltar>().GetComponentInChildren<ObjectPlacement>().enabled = false;
                    storedObjects[i].GetComponentInParent<DetermineLetter>().storedObject.GetComponent<ObjectHolder>().enabled = false;
                    storedObjects[i].GetComponentInParent<DetermineLetter>().storedObject.GetComponent<Outline>().enabled = false;
                }
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
