using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordCollision : MonoBehaviour
{
    string word;

    [SerializeField, Tooltip("The object on the canvas with the TMP components as its children")]
    GameObject wordObject;

    PuzzleController wordControl;

    RespawnWordColliders respawn;

    void Start()
    {
        word = gameObject.name;
        respawn = FindObjectOfType<RespawnWordColliders>();
        wordControl = FindObjectOfType<PuzzleController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GameController"))
        {
            wordControl.wordObject = wordObject;
            wordControl.word = word;
            wordControl.SetUpLetters();

            respawn.RespawnColliders();

            gameObject.SetActive(false);
        }
    }
}
