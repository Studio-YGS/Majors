using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordCollision : MonoBehaviour //this script sets up the puzzle controllers with the correct words, canvas letters, altars and street names whenever a player enters the box colliders attached to these objects
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
        if (other.CompareTag("GameController")) //ensuring that the object that has entered is the player
        {
            if (gameObject.name.Contains("Home")) //hard coded for the home, because home doesn't have properly set up colliders
            {
                SetHomeText();
            }
            else
            {
                SetUpController();
            }
        }
    }

    void SetUpWords() //this method finds the right words to add to this object from the word holder script.
    {
        WordHolder holder = FindObjectOfType<WordHolder>();

        for(int i = 0; i < holder.words.Length; i++)
        {
            if(holder.words[i].name == mainWord) //checks all the words to see if they are the same as the mainword for this object
            {
                wordObjects.Add(holder.words[i]);
            }
        }

        if(overlappedStreets.Length > 0) //if there is an overlap, it will then add all of the words from the overlapped ones too.
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

    void AssignController() //there will only ever been one puzzle controller active at a time, so it just finds whichever one is active to use.
    {
        puzzleController = FindObjectOfType<PuzzleController>();
    }

    public void SetUpController() //this method sets up the puzzle controller with the right words and calls some of the methods in the puzzle controller to get some of the systems started.
    {
        if (!gameObject.name.Contains("Home")) //ignores this method if it came from the home object
        {
            if(!dontWrite && !dontCheck)
            {
                uiController.SwitchStreet(this); //switches the street name out and animates
            }

            if (puzzleController == null) //if it doesn't already have one, assigns a puzzle controller
            {
                AssignController();
            }

            //respawn.RespawnColliders();

            puzzleController.wordCollision = GetComponent<WordCollision>(); //makes itself the word collision variable in the controller
            puzzleController.word = mainWord; //sets the current word to its main word
            puzzleController.wordObjects.Clear(); //and then clears the wordobjects list.

            if (wordObjects != null)
            {
                for (int i = 0; i < wordObjects.Count; i++)
                {
                    if (wordObjects[i] != null)
                    {
                        puzzleController.wordObjects.Add(wordObjects[i]); //the method then goes through and adds all of the word objects that are assigned to this objects to the puzzle controller, so it can check them while the player is completing the words.

                        if (wordObjects[i].name == mainWord)
                        {
                            puzzleController.SetUpLetters(i); //calls the method to set up the canvas letters
                        }
                    }
                }
            }

            puzzleController.storedObjects.Clear(); //clears the stored objects

            if (!puzzleComplete && !dontCheck) //checks to see if the player has completed any of the words in this object, including both single and overlapped word colliders
            {
                puzzleController.PlayerWordControl();
            }
            

            if (streetStalk != null) //Mr crossy ai stuff
            {
                seer.m_StalkStreet = streetStalk;
            }

            //gameObject.SetActive(false);
        }
    }

    public void DisableAltars() //this method disables the altars, the objects on the altars, and the outline on both. it is called when the player correctly completes a word.
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

    public void SetHomeText() //forces the home switch for the home colliders   
    {
        uiController.HomeSwitch();
    }
}
