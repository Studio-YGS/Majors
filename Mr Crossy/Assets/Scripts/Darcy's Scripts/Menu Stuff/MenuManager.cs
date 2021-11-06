using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;
using TMPro;

public class MenuManager : MonoBehaviour
{
    GameObject instance;

    Player_Controller playerController;

    JournalOnSwitch journalOnSwitch;

    JournalController journalController;

    [SerializeField]
    GameObject pauseMenuObject, settingsMenuObject, mainMenuObject;

    public GameObject streetName;

    float defTimeScale;

    bool mainMenu = true;

    void Start()
    {
        //if(instance != gameObject)
        //{
        //    instance = gameObject;
        //    DontDestroyOnLoad(instance);
        //}

        if (!mainMenu)
        {
            playerController = FindObjectOfType<Player_Controller>();
            journalOnSwitch = FindObjectOfType<JournalOnSwitch>();
            journalController = FindObjectOfType<JournalController>();
            defTimeScale = Time.timeScale;
        }
    }

    void Update()
    {
        if (!mainMenu)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!playerController.inJournal && !pauseMenuObject.activeInHierarchy && !settingsMenuObject.activeInHierarchy)
                {
                    OpenPauseMenu();
                }
                else if (!playerController.inJournal && !pauseMenuObject.activeInHierarchy && settingsMenuObject.activeInHierarchy)
                {
                    CloseSettingsMenu();
                    ClosePauseMenu();
                }
                else if (!playerController.inJournal && pauseMenuObject.activeInHierarchy)
                {
                    ClosePauseMenu();
                }
            }
        }
    }

    public void OpenPauseMenu()
    {
        pauseMenuObject.SetActive(true);

        playerController.DisableController();
        playerController.inJournal = false;

        journalController.DisableJournal();

        journalOnSwitch.HideTab();

        streetName.SetActive(false);

        Time.timeScale = 0f;
    }

    public void ClosePauseMenu()
    {
        pauseMenuObject.SetActive(false);

        playerController.EnableController();
        playerController.inJournal = false;

        journalController.EnableJournal();

        journalOnSwitch.ShowTab();

        streetName.SetActive(true);

        Time.timeScale = defTimeScale;
    }

    public void StartGame()
    {
        mainMenu = false;
        SceneManager.LoadScene("Main_Cael");
    }

    public void OpenSettingsMenu()
    {
        settingsMenuObject.SetActive(true);

        if (!mainMenu)
        {
            pauseMenuObject.SetActive(false);
        }
        else
        {
            mainMenuObject.SetActive(false);
        }
    }

    public void CloseSettingsMenu()
    {
        settingsMenuObject.SetActive(false);

        if (!mainMenu)
        {
            pauseMenuObject.SetActive(true);
        }
        else
        {
            mainMenuObject.SetActive(true);
        }
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
                    Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
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
