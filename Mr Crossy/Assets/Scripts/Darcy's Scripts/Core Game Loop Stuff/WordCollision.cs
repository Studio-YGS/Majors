using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordCollision : MonoBehaviour
{
    public string word, street;

    public string[] overlappedStreets;

    [HideInInspector]
    public bool puzzleComplete, altarsDisabled, dontWrite, dontCheck;

    [SerializeField]
    GameObject[] wordObjects, altars;

    [SerializeField]
    PuzzleController puzzleController;

    OverseerController seer;

    MenuManager menuManager;

    [SerializeField] 
    CrossyStreetStalk streetStalk;

    RespawnWordColliders respawn;

    void Start()
    {
        menuManager = FindObjectOfType<MenuManager>();
        respawn = FindObjectOfType<RespawnWordColliders>();
        seer = FindObjectOfType<OverseerController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GameController"))
        {
            SetUpController();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("GameController"))
        {
            puzzleController.streetText.text = "";
        }
    }

    public void SetUpController()
    {
        respawn.RespawnColliders();

        puzzleController.wordCollision = GetComponent<WordCollision>();
        puzzleController.word = word;
        puzzleController.wordObjects.Clear();
        puzzleController.currentStreet = street;

        if (wordObjects != null)
        {
            for (int i = 0; i < wordObjects.Length; i++)
            {
                if(wordObjects[i] != null)
                {
                    puzzleController.wordObjects.Add(wordObjects[i]);

                    if (wordObjects[i].name == word)
                    {
                        puzzleController.SetUpLetters(i);
                    }
                }
            }
        }

        puzzleController.storedObjects.Clear();

        if (!puzzleComplete && !dontCheck)
        {
            puzzleController.PlayerWordControl();
        }

        menuManager.streetName.SetActive(true);

        if(streetStalk != null)
        {
            seer.m_StalkStreet = streetStalk;
        }

        gameObject.SetActive(false);
    }

    public void DisableAltars()
    {
        if (altars.Length > 0)
        {
            for (int i = 0; i < altars.Length; i++)
            {
                if (altars[i].GetComponent<OverlappedAltar>())
                {
                    altars[i].GetComponent<Outline>().enabled = false;
                    altars[i].GetComponentInChildren<ObjectPlacement>().enabled = false;
                    altars[i].GetComponentInChildren<DetermineLetter>().storedObject.GetComponent<ObjectHolder>().enabled = false;
                    altars[i].GetComponentInChildren<DetermineLetter>().storedObject.GetComponent<Outline>().enabled = false;
                }
                else if (altars[i].GetComponent<DetermineLetter>())
                {
                    altars[i].GetComponent<Outline>().enabled = false;
                    altars[i].GetComponentInChildren<ObjectPlacement>().enabled = false;
                    altars[i].GetComponent<DetermineLetter>().storedObject.GetComponent<ObjectHolder>().enabled = false;
                    altars[i].GetComponent<DetermineLetter>().storedObject.GetComponent<Outline>().enabled = false;
                }
            }

            altarsDisabled = true;
        }
    }

    public void SetHomeText()
    {
        puzzleController.streetText.text = "Home.";
    }
}
