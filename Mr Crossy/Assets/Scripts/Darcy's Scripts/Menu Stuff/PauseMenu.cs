using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    Player_Controller playerController;

    JournalOnSwitch journalOnSwitch;

    [SerializeField]
    GameObject pauseMenuObject;

    void Start()
    {
        playerController = FindObjectOfType<Player_Controller>();
        journalOnSwitch = FindObjectOfType<JournalOnSwitch>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!playerController.inJournal && !pauseMenuObject.activeInHierarchy)
            {
                OpenPauseMenu();
            }
            else if(!playerController.inJournal && pauseMenuObject.activeInHierarchy)
            {
                ClosePauseMenu();
            }
        }
    }

    public void OpenPauseMenu()
    {
        pauseMenuObject.SetActive(true);
        playerController.DisableController();
        playerController.inJournal = false;
        journalOnSwitch.HideTab();
    }

    public void ClosePauseMenu()
    {
        pauseMenuObject.SetActive(false);
        playerController.EnableController();
        playerController.inJournal = false;
        journalOnSwitch.ShowTab();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
