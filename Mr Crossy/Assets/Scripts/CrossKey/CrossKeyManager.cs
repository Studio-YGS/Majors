using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class PuzzleOptions
{
    public string hint;
    public string wordToSolve;
    [Header("ToothOne")]
    public string keyToothWordOne;
    public bool wordOneSetBack;
    [Header("ToothTwo")]
    public string keyToothWordTwo;
    public bool wordTwoSetBack;
}

[System.Serializable]
public class KeyPuzzles
{
    public int numOfLetters;
    public GameObject crossKey;
    public PuzzleOptions[] puzzleOptions;
}

public class CrossKeyManager : MonoBehaviour
{
    public int numOfKeys = 1;
    public TMP_Text hintArea;
    public KeyPuzzles[] keyPuzzles;
    Transform cam;
    [HideInInspector] public bool puzzleOn;
    [HideInInspector] public Player_Controller controller;
    [HideInInspector] public HeadBob headBob;

    void Start()
    {
        cam = FindObjectOfType<Camera>().transform;
        controller = FindObjectOfType<Player_Controller>();
        headBob = FindObjectOfType<HeadBob>();
    }

    
    void Update()
    {
        
    }
    public void StartCrossKeyPuzzle(DoorInteraction door)
    {
        
        if(numOfKeys > 0 && !puzzleOn)
        {
            numOfKeys -= 1;
            puzzleOn = true;
            controller.enabled = false;
            headBob.enabled = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            int randomKey = Random.Range(0, keyPuzzles.Length);
            KeyPuzzles key = keyPuzzles[randomKey];
            Vector3 posOffset = key.crossKey.transform.position - key.crossKey.transform.GetComponent<Renderer>().bounds.center;
            GameObject newCrossKey = Instantiate(key.crossKey, cam.position + cam.forward * 0.4f + posOffset, key.crossKey.transform.rotation);
            newCrossKey.layer = 6;
            foreach(Transform t in newCrossKey.transform)
            {
                t.gameObject.layer = 6;
                foreach (Transform T in t.transform)
                {
                    T.gameObject.layer = 6;
                    foreach (Transform Tr in T.transform)
                    {
                        Tr.gameObject.layer = 6;
                        foreach (Transform TR in Tr.transform)
                        {
                            TR.gameObject.layer = 6;
                        }
                    }
                }
            }
            if (keyPuzzles[randomKey].numOfLetters == 4)
            {
                FourLetterKey(key, newCrossKey, door);
            }
            if (keyPuzzles[randomKey].numOfLetters == 5)
            {
                FiveLetterKey(key, newCrossKey, door);
            }
            if (keyPuzzles[randomKey].numOfLetters == 6)
            {
                SixLetterKey(key, newCrossKey, door);
            }
            if(keyPuzzles[randomKey].numOfLetters == 7)
            {
                SevenLetterKey(key, newCrossKey, door);
            }
            
        }
    }

    void FourLetterKey(KeyPuzzles key, GameObject newCrossKey, DoorInteraction door)
    {
        int randomPuzzle = Random.Range(0, key.puzzleOptions.Length);

        PuzzleOptions puzzle = key.puzzleOptions[randomPuzzle];
        if (puzzle.wordTwoSetBack)
        {
            string[] characters = new string[puzzle.keyToothWordTwo.Length];
            for (int i = 0; i < 3; i++)
            {
                characters[i] = System.Convert.ToString(puzzle.keyToothWordTwo[i]);
                newCrossKey.GetComponent<CrossKey>().toothTwoBack[i].text = characters[i];
            }
        }
        else if (puzzle.wordTwoSetBack == false)
        {
            string[] characters = new string[puzzle.keyToothWordTwo.Length];
            for (int i = 0; i < 3; i++)
            {
                characters[i] = System.Convert.ToString(puzzle.keyToothWordTwo[i]);
                newCrossKey.GetComponent<CrossKey>().toothTwoFront[i].text = characters[i];
            }
        }

        //string[] answerChar = new string[puzzle.wordToSolve.Length];
        //for (int i = 0; i < 4; i++)
        //{
        //    answerChar[i] = System.Convert.ToString(puzzle.wordToSolve[i]);

        //}
        newCrossKey.GetComponent<CrossKey>().door = door;
        newCrossKey.GetComponent<CrossKey>().answer = puzzle.wordToSolve;
        hintArea.text = "[C]LUE: " + puzzle.hint;
    }

    void FiveLetterKey(KeyPuzzles key, GameObject newCrossKey, DoorInteraction door)
    {
        int randomPuzzle = Random.Range(0, key.puzzleOptions.Length);

        PuzzleOptions puzzle = key.puzzleOptions[randomPuzzle];
        if (puzzle.wordOneSetBack)
        {
            string[] characters = new string[puzzle.keyToothWordOne.Length];
            for (int i = 0; i < 4; i++)
            {
                characters[i] = System.Convert.ToString(puzzle.keyToothWordOne[i]);
                newCrossKey.GetComponent<CrossKey>().toothOneBack[i].text = characters[i];
            }
        }
        else if (puzzle.wordOneSetBack == false)
        {
            string[] characters = new string[puzzle.keyToothWordOne.Length];
            for (int i = 0; i < 4; i++)
            {
                characters[i] = System.Convert.ToString(puzzle.keyToothWordOne[i]);
                newCrossKey.GetComponent<CrossKey>().toothOneFront[i].text = characters[i];
            }
        }

        if (puzzle.wordTwoSetBack)
        {
            string[] characters = new string[puzzle.keyToothWordTwo.Length];
            for (int i = 0; i < 3; i++)
            {
                characters[i] = System.Convert.ToString(puzzle.keyToothWordTwo[i]);
                newCrossKey.GetComponent<CrossKey>().toothTwoBack[i].text = characters[i];
            }
        }
        else if (puzzle.wordTwoSetBack == false)
        {
            string[] characters = new string[puzzle.keyToothWordTwo.Length];
            for (int i = 0; i < 3; i++)
            {
                characters[i] = System.Convert.ToString(puzzle.keyToothWordTwo[i]);
                newCrossKey.GetComponent<CrossKey>().toothTwoFront[i].text = characters[i];
            }
        }

        //string[] answerChar = new string[puzzle.wordToSolve.Length];
        //for (int i = 0; i < 5; i++)
        //{
        //    answerChar[i] = System.Convert.ToString(puzzle.wordToSolve[i]);

        //}
        newCrossKey.GetComponent<CrossKey>().door = door;
        newCrossKey.GetComponent<CrossKey>().answer = puzzle.wordToSolve;
        hintArea.text = "[C]LUE: " + puzzle.hint;
    }

    void SixLetterKey(KeyPuzzles key, GameObject newCrossKey, DoorInteraction door)
    {
        int randomPuzzle = Random.Range(0, key.puzzleOptions.Length);

        PuzzleOptions puzzle = key.puzzleOptions[randomPuzzle];
        if (puzzle.wordOneSetBack)
        {
            string[] characters = new string[puzzle.keyToothWordOne.Length];
            for (int i = 0; i < 4; i++)
            {
                characters[i] = System.Convert.ToString(puzzle.keyToothWordOne[i]);
                newCrossKey.GetComponent<CrossKey>().toothOneBack[i].text = characters[i];
            }
        }
        else if (puzzle.wordOneSetBack == false)
        {
            string[] characters = new string[puzzle.keyToothWordOne.Length];
            for (int i = 0; i < 4; i++)
            {
                characters[i] = System.Convert.ToString(puzzle.keyToothWordOne[i]);
                newCrossKey.GetComponent<CrossKey>().toothOneFront[i].text = characters[i];
            }
        }

        if (puzzle.wordTwoSetBack)
        {
            string[] characters = new string[puzzle.keyToothWordTwo.Length];
            for (int i = 0; i < 3; i++)
            {
                characters[i] = System.Convert.ToString(puzzle.keyToothWordTwo[i]);
                newCrossKey.GetComponent<CrossKey>().toothTwoBack[i].text = characters[i];
            }
        }
        else if (puzzle.wordTwoSetBack == false)
        {
            string[] characters = new string[puzzle.keyToothWordTwo.Length];
            for (int i = 0; i < 3; i++)
            {
                characters[i] = System.Convert.ToString(puzzle.keyToothWordTwo[i]);
                newCrossKey.GetComponent<CrossKey>().toothTwoFront[i].text = characters[i];
            }
        }

        //string[] answerChar = new string[puzzle.wordToSolve.Length];
        //for (int i = 0; i < 6; i++)
        //{
        //    answerChar[i] = System.Convert.ToString(puzzle.wordToSolve[i]);

        //}
        newCrossKey.GetComponent<CrossKey>().door = door;
        newCrossKey.GetComponent<CrossKey>().answer = puzzle.wordToSolve;
        hintArea.text = "[C]LUE: " + puzzle.hint;
    }

    void SevenLetterKey(KeyPuzzles key, GameObject newCrossKey, DoorInteraction door)
    {
        int randomPuzzle = Random.Range(0, key.puzzleOptions.Length);

        PuzzleOptions puzzle = key.puzzleOptions[randomPuzzle];
        if (puzzle.wordOneSetBack)
        {
            string[] characters = new string[puzzle.keyToothWordOne.Length];
            for (int i = 0; i < 4; i++)
            {
                characters[i] = System.Convert.ToString(puzzle.keyToothWordOne[i]);
                newCrossKey.GetComponent<CrossKey>().toothOneBack[i].text = characters[i];
            }
        }
        else if (puzzle.wordOneSetBack == false)
        {
            string[] characters = new string[puzzle.keyToothWordOne.Length];
            for (int i = 0; i < 4; i++)
            {
                characters[i] = System.Convert.ToString(puzzle.keyToothWordOne[i]);
                newCrossKey.GetComponent<CrossKey>().toothOneFront[i].text = characters[i];
            }
        }

        if (puzzle.wordTwoSetBack)
        {
            string[] characters = new string[puzzle.keyToothWordTwo.Length];
            for (int i = 0; i < 3; i++)
            {
                characters[i] = System.Convert.ToString(puzzle.keyToothWordTwo[i]);
                newCrossKey.GetComponent<CrossKey>().toothTwoBack[i].text = characters[i];
            }
        }
        else if (puzzle.wordTwoSetBack == false)
        {
            string[] characters = new string[puzzle.keyToothWordTwo.Length];
            for (int i = 0; i < 3; i++)
            {
                characters[i] = System.Convert.ToString(puzzle.keyToothWordTwo[i]);
                newCrossKey.GetComponent<CrossKey>().toothTwoFront[i].text = characters[i];
            }
        }

        //string[] answerChar = new string[puzzle.wordToSolve.Length];
        //for (int i = 0; i < 7; i++)
        //{
        //    answerChar[i] = System.Convert.ToString(puzzle.wordToSolve[i]);

        //}
        newCrossKey.GetComponent<CrossKey>().door = door;
        newCrossKey.GetComponent<CrossKey>().answer = puzzle.wordToSolve;
        hintArea.text = "[C]LUE: " + puzzle.hint;
    }

    public void PuzzleDeath()
    {
        Debug.Log("Dead");
    }
}
