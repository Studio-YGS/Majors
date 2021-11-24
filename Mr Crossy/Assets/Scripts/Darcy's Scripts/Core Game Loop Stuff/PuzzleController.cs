using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using FMOD.Studio;
using FMODUnity;

//Written by Darcy Glover

public class PuzzleController : MonoBehaviour
{
    public string word, currentStreet;
    string playersWord, letter, altarName, uiWord;

    [SerializeField]
    TextMeshProUGUI mistakeText; 
    [HideInInspector]
    public TextMeshProUGUI streetText;

    EventInstance eventInstance;

    int wordLength, mistakeCount, completedWords, letterPoint, objectPoint;
    public int wordsInPuzzle, section;

    //[HideInInspector]
    public List<TextMeshProUGUI> canvasLetters = new List<TextMeshProUGUI>();

    //[HideInInspector]
    public List<GameObject> storedObjects = new List<GameObject>();
    public List<GameObject> wordObjects = new List<GameObject>();

    public bool tutorial;
    bool tenPlayed;

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
        objectPoint = whichObject;

        canvasLetters.Clear();

        foreach (TextMeshProUGUI tmp in wordObjects[objectPoint].GetComponentsInChildren<TextMeshProUGUI>())
        {
            canvasLetters.Add(tmp);
        }

        wordLength = canvasLetters.Count;

        if (word == wordObjects[objectPoint].name)
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

        if(word == wordObjects[objectPoint].name)
        {
            WriteToUI();
        }

        if (tutorial)
        {
            TutorialController tutorialController = FindObjectOfType<TutorialController>();

            tutorialController.ChangeConLetter(letter);

            eventInstance = RuntimeManager.CreateInstance("event:/MR_C_Tutorial/TUT.0.5");

            eventInstance.start();

            tutorialEvent.Invoke();
            tutorial = false;
        }

        if(playersWord == "TEN" && !tenPlayed)
        {
            tenPlayed = true;

            eventInstance = RuntimeManager.CreateInstance("event:/MR_C_SolvedPuzzles/I.SP.1_Ten");

            eventInstance.start();
        }

        if(playersWord == word) //the script then checks to see if the players formed word is the same as the puzzle's answer
        {
            if (wordCollision != null)
            {
                wordCollision.puzzleComplete = true;
            }
            CompletionCheck();
        }

        if (playersWordLength == wordLength && playersWord != word) //if the player has put all the letters on the altar but hasnt gotten the word right, it counts down a mistake.
        {
            AudioEvents audio = FindObjectOfType<AudioEvents>();

            audio.WordSpeltIncorrectly();

            GameOverCheck();
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

        for(int i = 0; i < wordObjects.Count; i++)
        {
            if(word == wordObjects[i].name)
            {
                Debug.Log(wordObjects[i].name);
                wordObjects[i].GetComponentInChildren<Image>().enabled = true;
                break;
            }
        }

        if(wordCollision != null)
        {
            wordCollision.DisableAltars();
        }

        if(completedWords == wordsInPuzzle)
        {
            winEvent.Invoke();
        }
    }

    public bool GameOverCheck()
    {
        if (!gameObject.name.Contains("Tutorial"))
        {
            mistakeCount++;
            mistakeText.text = mistakeCount.ToString();
        }
        else if(gameObject.name.Contains("Tutorial"))
        {
            tutorialMistakeEvent.Invoke();
        }

        if (mistakeCount == 3)
        {
            GameOver();
            return true;
        }
        else
        {
            return false;
        }
    }

    void GameOver()
    {
        loseEvent.Invoke();
        StartCoroutine(StartAgain());
    }

    IEnumerator StartAgain()
    {
        yield return new WaitForSeconds(3f);

        FindObjectOfType<MenuManager>().RestartGame();
    }
}
