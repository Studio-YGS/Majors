using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Written by Darcy Glover

public class WordControl : MonoBehaviour
{
    [Tooltip("This puzzle's word.")]
    public string word;

    [Tooltip("The length of this word.")]
    public int wordLength;

    [Tooltip("The gate associated with this particular word")]
    public GameObject gate;

    [SerializeField, Tooltip("The canvas letters underneath the clue on the UI, specifically for this word")]
    Text[] canvasLetters;

    string playersWord, letter, altarName;

    public void ReceiveLetterAndName(string firstLetter, string altarOrigName) //receiving the letter of the object, and the name of the altar it came from
    {
        letter = firstLetter;
        altarName = altarOrigName;
        DisplayLetter();
        PlayerWordControl();
    }

    void DisplayLetter() 
    {
        GameObject.Find(altarName).GetComponent<DetermineLetter>().assignedLetter.GetComponent<Text>().text = letter; //changing the letter on the canvas to what the current letter is
    }

    void PlayerWordControl() //this method forms the players word as they place objects, and also controls the win condition
    {
        playersWord = "";
        for(int i = 0; i < wordLength; i++) //the player's word becomes equal to all the texts within the canvas letters combined
        {
            playersWord += canvasLetters[i].text;
        }
        Debug.Log("The player's word is now " + playersWord);

        if(playersWord == word) //the script then checks to see if the players formed word is the same as the puzzle's answer
        {
            gate.GetComponent<AnimationControl>().OpenGate();
        }
    }
}
