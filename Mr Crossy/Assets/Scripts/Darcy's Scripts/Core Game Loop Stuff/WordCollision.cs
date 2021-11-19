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

        for (int i = 0; i < wordObjects.Length; i++)
        {
            puzzleController.wordObjects.Add(wordObjects[i]);

            if(wordObjects[i].name == word)
            {
                wordPoint = i;
            }
        }

        puzzleController.currentStreet = street;
        puzzleController.SetUpLetters(wordPoint);
        puzzleController.storedObjects.Clear();
        puzzleController.wordCollision = gameObject.GetComponent<WordCollision>();

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
