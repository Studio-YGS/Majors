using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordCollision : MonoBehaviour
{
    public string word;

    [SerializeField, Tooltip("The object on the canvas with the TMP components as its children")]
    GameObject wordObject;

    [SerializeField]
    PuzzleController wordControl;

    RespawnWordColliders respawn;

    void Start()
    {
        respawn = FindObjectOfType<RespawnWordColliders>();
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
