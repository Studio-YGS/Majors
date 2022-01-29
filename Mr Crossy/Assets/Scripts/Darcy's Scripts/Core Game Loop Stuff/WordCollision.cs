using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordCollision : MonoBehaviour
{
    public string mainWord, street;

    public string[] overlappedStreets;

    [HideInInspector]
    public bool puzzleComplete, altarsDisabled, dontWrite, dontCheck;

    [HideInInspector]
    public List<GameObject> altars = new List<GameObject>();
    List<GameObject> wordObjects = new List<GameObject>();

    PuzzleController puzzleController;

    OverseerController seer;

    UIController uiController;

    [SerializeField] 
    CrossyStreetStalk streetStalk;

    RespawnWordColliders respawn;

    void Start()
    {
        respawn = FindObjectOfType<RespawnWordColliders>();
        seer = FindObjectOfType<OverseerController>();
        uiController = FindObjectOfType<UIController>();

        SetUpWords();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GameController"))
        {
            if (gameObject.name.Contains("Home"))
            {
                SetHomeText();
            }
            else
            {
                SetUpController();
            }
        }
    }

    void SetUpWords()
    {
        WordHolder holder = FindObjectOfType<WordHolder>();

        for(int i = 0; i < holder.words.Length; i++)
        {
            if(holder.words[i].name == mainWord)
            {
                wordObjects.Add(holder.words[i]);
            }
        }

        if(overlappedStreets.Length > 0)
        {
            for(int i = 0; i < overlappedStreets.Length; i++)
            {
                for(int x = 0; x < holder.words.Length; x++)
                {
                    if (holder.words[x].name == GameObject.Find(overlappedStreets[i]).GetComponent<WordCollision>().mainWord)
                    {
                        wordObjects.Add(holder.words[x]);
                    }
                }
            }
        }
    }

    void AssignController()
    {
        puzzleController = FindObjectOfType<PuzzleController>();
    }

    public void SetUpController()
    {
        if (!gameObject.name.Contains("Home"))
        {
            if(!dontWrite && !dontCheck)
            {
                uiController.SwitchStreet(this);
            }

            if (puzzleController == null)
            {
                AssignController();
            }

            respawn.RespawnColliders();

            puzzleController.wordCollision = GetComponent<WordCollision>();
            puzzleController.word = mainWord;
            puzzleController.wordObjects.Clear();

            if (wordObjects != null)
            {
                for (int i = 0; i < wordObjects.Count; i++)
                {
                    if (wordObjects[i] != null)
                    {
                        puzzleController.wordObjects.Add(wordObjects[i]);

                        if (wordObjects[i].name == mainWord)
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
            

            if (streetStalk != null)
            {
                seer.m_StalkStreet = streetStalk;
            }

            //gameObject.SetActive(false);
        }
    }

    public void DisableAltars()
    {
        if (altars.Count > 0)
        {
            for (int i = 0; i < altars.Count; i++)
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
        uiController.HomeSwitch();
    }
}
