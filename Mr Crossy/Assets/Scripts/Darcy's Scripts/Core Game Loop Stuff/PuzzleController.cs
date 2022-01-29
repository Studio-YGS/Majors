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
    string playersWord, letter, altarName;

    UIController uiController;

    [SerializeField]
    TextMeshProUGUI mistakeText;
    [HideInInspector]
    public TextMeshProUGUI streetText;

    EventInstance eventInstance;

    int wordLength, mistakeCount, completedWords, letterPoint, objectPoint;
    public int wordsInPuzzle;

    //[HideInInspector]
    public List<TextMeshProUGUI> canvasLetters = new List<TextMeshProUGUI>();

    [HideInInspector]
    public List<GameObject> storedObjects = new List<GameObject>();
    public List<GameObject> wordObjects = new List<GameObject>();

    public bool tutorial;
    bool tenPlayed, petPlayed, trophyPlayed, cheating;

    public UnityEvent winEvent, loseEvent, tutorialEvent, tutorialMistakeEvent;

    [HideInInspector]
    public WordCollision wordCollision;

    void Start()
    {
        uiController = FindObjectOfType<UIController>();

        if (tutorial)
        {
            uiController.currentWordDisplay = uiController.wordDisplayObjects[1];

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
            if (wordCollision == null)
            {
                WriteToUI();
            }
            else if (!wordCollision.dontWrite)
            {
                WriteToUI();
            }
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
        int playersWordLength;

        if (!cheating)
        {
            playersWord = "";

            for (int i = 0; i < wordLength; i++) //the player's word becomes equal to all the texts within the canvas letters combined
            {
                playersWord += canvasLetters[i].text;
            }
        }

        playersWordLength = playersWord.ToIntArray().Length;

        if (GameObject.Find(altarName) != null)
        {
            storedObjects.Add(GameObject.Find(altarName));
        }

        if (word == wordObjects[objectPoint].name)
        {
            if (wordCollision == null)
            {
                WriteToUI();
            }
            else if (!wordCollision.dontWrite)
            {
                WriteToUI();
            }
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

        AudioChecks();

        if (playersWord == word && wordCollision == null) //the script then checks to see if the players formed word is the same as the puzzle's answer
        {
            CompletionCheck();
        }

        if (wordCollision != null)
        {
            if (playersWord == word)
            {
                wordCollision.puzzleComplete = true;
                wordCollision.DisableAltars();
                CompletionCheck();
            }

            Debug.Log("Word Collision's overlapped streets length is: " + wordCollision.overlappedStreets.Length);

            if (wordCollision.overlappedStreets.Length > 0)
            {
                Debug.Log("Starting the overlapped streets loop.");
                for (int i = 0; i < wordCollision.overlappedStreets.Length; i++)
                {
                    if (!GameObject.Find(wordCollision.overlappedStreets[i]).GetComponent<WordCollision>().altarsDisabled)
                    {
                        WordCollision temp = wordCollision;
                        Debug.Log("Altars disabled if statement has been accessed for: " + wordCollision.overlappedStreets[i] + ". Temp is: " + temp.gameObject.name);
                        wordCollision = GameObject.Find(wordCollision.overlappedStreets[i]).GetComponent<WordCollision>();
                        wordCollision.dontWrite = true;
                        wordCollision.SetUpController();
                        wordCollision.dontWrite = false;
                        Debug.Log("Just completed SetUpController for: " + wordCollision.gameObject.name);
                        wordCollision = temp;
                        Debug.Log("Word collision has been reverted back to: " + wordCollision.gameObject.name + " from: " + temp.gameObject.name);
                        wordCollision.dontCheck = true;
                        wordCollision.SetUpController();
                        wordCollision.dontCheck = false;
                    }
                }
            }
        }

        if (playersWordLength == wordLength && playersWord != word) //if the player has put all the letters on the altar but hasnt gotten the word right, it counts down a mistake.
        {
            if (wordCollision != null)
            {
                if (!wordCollision.puzzleComplete)
                {
                    AudioEvents audio = FindObjectOfType<AudioEvents>();

                    audio.WordSpeltIncorrectly();

                    MistakeCounter();
                }
            }
            else if(wordCollision == null)
            {
                AudioEvents audio = FindObjectOfType<AudioEvents>();

                audio.WordSpeltIncorrectly();

                MistakeCounter();
            }
        }
    }

    public void WriteToUI()
    {
        string currentAltarWord = "";

        for (int i = 0; i < canvasLetters.Count; i++)
        {
            if (canvasLetters[i].text.ToIntArray().Length < 1)
            {
                currentAltarWord += " _ ";
            }
            else
            {
                currentAltarWord += canvasLetters[i].text;
            }
        }

        Debug.Log("current st: " + currentStreet + ",  current altar word: " + currentAltarWord);

        TextMeshProUGUI[] letters = uiController.currentWordDisplay.GetComponentsInChildren<TextMeshProUGUI>();

        for(int i = 0; i < letters.Length; i++)
        {
            if (canvasLetters[i].text.ToIntArray().Length < 1)
            {
                letters[i].text = "";
                letters[i].alpha = 0f;
            }
            else
            {
                letters[i].text = canvasLetters[i].text;
                letters[i].alpha = 1f;
            }
        }

        //uiWord = currentStreet + ": " + currentAltarWord;

        //if (currentStreet == "")
        //{
        //    uiWord = "";
        //}

        ////streetText.text = uiWord;
    }

    void CompletionCheck()
    {
        if (!cheating)
        {
            completedWords++;
        }

        if (gameObject.name.Contains("Tutorial"))
        {
            TutorialAltarDisable();
        }

        AudioEvents audio = FindObjectOfType<AudioEvents>();

        audio.WordSpeltCorrectly();

        for (int i = 0; i < wordObjects.Count; i++)
        {
            if (word == wordObjects[i].name)
            {
                Debug.Log(wordObjects[i].name);
                wordObjects[i].GetComponentInChildren<Image>().enabled = true;
                break;
            }
        }

        if (completedWords == wordsInPuzzle)
        {
            winEvent.Invoke();
        }
    }

    public void MistakeCounter()
    {
        if (!gameObject.name.Contains("Tutorial"))
        {
            Debug.Log("MISTAKE");
            mistakeCount++;
            mistakeText.text = mistakeCount.ToString();
        }
        else if (gameObject.name.Contains("Tutorial"))
        {
            tutorialMistakeEvent.Invoke();
        }
    }

    void TutorialAltarDisable()
    {
        for(int i = 0; i < storedObjects.Count; i++)
        {
            storedObjects[i].GetComponent<Outline>().enabled = false;
            storedObjects[i].GetComponentInChildren<ObjectPlacement>().enabled = false;
            storedObjects[i].GetComponent<DetermineLetter>().storedObject.GetComponent<Outline>().enabled = false;
            storedObjects[i].GetComponent<DetermineLetter>().storedObject.GetComponent<ObjectHolder>().enabled = false;
        }
    }

    void AudioChecks()
    {
        if (playersWord == "TEN" && !tenPlayed)
        {
            tenPlayed = true;

            eventInstance = RuntimeManager.CreateInstance("event:/MR_C_SolvedPuzzles/I.SP.1_Ten");

            eventInstance.start();
        }

        if (playersWord == "PET" && !petPlayed)
        {
            petPlayed = true;

            eventInstance = RuntimeManager.CreateInstance("event:/MR_C_SolvedPuzzles/I.SP.4.1_Pet");

            eventInstance.start();
        }

        if (playersWord == "TROPHY" && !trophyPlayed)
        {
            trophyPlayed = true;

            eventInstance = RuntimeManager.CreateInstance("event:/MR_C_SolvedPuzzles/I.SP.3_Trophy");

            eventInstance.start();

            GetComponent<TutorialSectionStart>().ObjectsTeach();
        }
    }

    public void DevSkip()
    {
        cheating = true;
        completedWords = wordsInPuzzle;
        CompletionCheck();
        cheating = false;
    }

    public void CompleteCurrentWord()
    {
        playersWord = word;
        cheating = true;
        PlayerWordControl();
        cheating = false;
    }
}
