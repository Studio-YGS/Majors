using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordCollision : MonoBehaviour
{
    public string word, street;

    [HideInInspector]
    public bool puzzleComplete;

    [SerializeField]
    GameObject[] wordObjects;

    [SerializeField]
    PuzzleController puzzleController;

    MenuManager menuManager;

    [SerializeField] 
    CrossyStreetStalk streetStalk;

    RespawnWordColliders respawn;

    void Start()
    {
        menuManager = FindObjectOfType<MenuManager>();
        respawn = FindObjectOfType<RespawnWordColliders>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GameController"))
        {
            SetUpController();
        }
    }

    void SetUpController()
    {
        int wordPoint = 0;

        puzzleController.word = word;
        puzzleController.wordObjects.Clear();

        if(wordObjects != null)
        {
            for (int i = 0; i < wordObjects.Length; i++)
            {
                if(wordObjects[i] != null)
                {
                    puzzleController.wordObjects.Add(wordObjects[i]);

                    if (wordObjects[i].name == word)
                    {
                        wordPoint = i;
                        puzzleController.SetUpLetters(wordPoint);
                    }
                }
            }
        }

        puzzleController.currentStreet = street;
        puzzleController.storedObjects.Clear();
        puzzleController.wordCollision = GetComponent<WordCollision>();

        if (!puzzleComplete)
        {
            puzzleController.PlayerWordControl();
        }

        menuManager.streetName.SetActive(true);

        FindObjectOfType<OverseerController>().m_StalkStreet = streetStalk;

        respawn.RespawnColliders();

        gameObject.SetActive(false);
    }
}
