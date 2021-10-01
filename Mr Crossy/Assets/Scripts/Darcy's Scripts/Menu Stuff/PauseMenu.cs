using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    Player_Controller playerController;

    JournalOnSwitch journalOnSwitch;

    [SerializeField]
    GameObject pauseMenuObject, settingsMenuObject;

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

    public void OpenSettingsMenu()
    {
        settingsMenuObject.SetActive(true);
        pauseMenuObject.SetActive(false);
    }

    public void CloseSettingsMenu()
    {
        settingsMenuObject.SetActive(false);
        pauseMenuObject.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ResolutionChange(int val)
    {
        switch (val)
        {
            case 0:
                {
                    Debug.Log("Fullscreen");
                    Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
                    Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                    break;
                }
            case 1:
                {
                    Debug.Log("Windowed F");
                    Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
                    Screen.fullScreenMode = FullScreenMode.Windowed;
                    break;
                }
            case 2:
                {
                    Debug.Log("Windowed");
                    Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, false);
                    break;
                }
        }
    }
}
