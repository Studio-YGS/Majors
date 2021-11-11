using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordCollision : MonoBehaviour
{
    public string word, street;

    [SerializeField, Tooltip("The object on the canvas with the TMP components as its children")]
    GameObject wordObject;

    [SerializeField]
    PuzzleController puzzleController;

    [SerializeField] CrossyStreetStalk streetStalk;

    RespawnWordColliders respawn;

    void Start()
    {
        respawn = FindObjectOfType<RespawnWordColliders>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GameController"))
        {
            puzzleController.wordObject = wordObject;
            puzzleController.word = word;
            puzzleController.currentStreet = street;
            puzzleController.SetUpLetters();
            puzzleController.storedObjects.Clear();

            FindObjectOfType<OverseerController>().m_StalkStreet = streetStalk;

            respawn.RespawnColliders();

            

            gameObject.SetActive(false);
        }
    }
}
