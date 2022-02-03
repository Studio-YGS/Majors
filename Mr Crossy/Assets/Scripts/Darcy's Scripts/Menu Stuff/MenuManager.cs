using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using FMODUnity;
using FMOD.Studio;

public class MenuManager : MonoBehaviour //this script manages the menus, such as the main menu, the pause menu and the settings menu
{
    Player_Controller playerController;

    JournalOnSwitch journalOnSwitch;

    JournalController journalController;

    AudioSettings audioSettings;

    [SerializeField]
    GameObject pauseMenuObject, settingsMenuObject, mainMenuObject, pressSpace, controlsUI, loadingAni;

    float defTimeScale;

    public bool mainMenu;
    [HideInInspector] 
    public bool menuOpen;

    bool dontEnable, quitGagPlaying;

    AsyncOperation loadingScene;

    public GameObject screenFade;

    void Start()
    {
        if (!mainMenu) //will only find these things if it isn't the main menu
        {
            playerController = FindObjectOfType<Player_Controller>();
            journalOnSwitch = FindObjectOfType<JournalOnSwitch>();
            journalController = FindObjectOfType<JournalController>();
            defTimeScale = Time.timeScale;
        }

        audioSettings = FindObjectOfType<AudioSettings>();
        audioSettings.MuteControl(true, 3); //mutes the unpause group

        LoadSettings(); //loads the player's audio settings that they had last time they played
        UpdateSliders(); //updates the sliders to reflect this
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) //escape is the pause key
        {
            if (!mainMenu && !FindObjectOfType<CrossKeyManager>().puzzleOn && !journalController.readingHowTo && !journalController.waitForCrossy && !controlsUI.activeInHierarchy)
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

    public void OpenPauseMenu() //this method opens the pause menu
    {
        pauseMenuObject.SetActive(true);
        menuOpen = true;
        playerController.DisableController(); //disables the player controller
        playerController.inJournal = false;

        audioSettings.MuteControl(false, 3); //unmutes the unpause group

        if (journalController.disabled) //this stops the journal from being enabled after closing the pause menu if it was disabled when the pause happened.
        {
            dontEnable = true;
        }
        else
        {
            dontEnable = false;
        }

        journalController.DisableJournal();

        journalOnSwitch.HideTab();

        for(int i = 0; i < GetComponent<TutorialController>().objectsToSwitchOn.Length; i++) //turns off all ui stuff
        {
            GetComponent<TutorialController>().objectsToSwitchOn[i].SetActive(false);
        }

        //RuntimeManager.PauseAllEvents(true);
        RuntimeManager.GetBus("bus:/Pause Group").setPaused(true); //pauses the pause group

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Time.timeScale = 0f; 
    }

    public void ClosePauseMenu() //this method closes the pause menu
    {
        pauseMenuObject.SetActive(false);
        menuOpen = false;
        playerController.EnableController();
        playerController.inJournal = false;

        if (!dontEnable) //wont reenable the journal if it was disabled when the game was paused
        {
            journalController.EnableJournal();
            journalOnSwitch.ShowTab();
        }

        for (int i = 0; i < GetComponent<TutorialController>().objectsToSwitchOn.Length; i++) //turns back on ui elements
        {
            GetComponent<TutorialController>().objectsToSwitchOn[i].SetActive(true);
        }

        //RuntimeManager.PauseAllEvents(false);
        RuntimeManager.GetBus("bus:/Pause Group").setPaused(false); //unpauses the pause group

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = defTimeScale;
    }

    public void StartGame() //this method starts the game
    {
        OpenControlsUI();

        loadingAni.SetActive(true);

        mainMenuObject.SetActive(false);

        StartCoroutine(ReadingControls());
    }

    public void OpenSettingsMenu() //this method opens the settings menu
    {
        settingsMenuObject.SetActive(true);

        if (!mainMenu) //wont close the pause menu if its the main menu
        {
            pauseMenuObject.SetActive(false);
        }
        else
        {
            mainMenuObject.SetActive(false);
        }
    }

    public void CloseSettingsMenu() //thsi method closes the settings menu
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

    public void OpenControlsUI() //this method shows the controls screen for the player when the game is loading, or if the player clicks it in the settings menu
    {
        controlsUI.SetActive(true);
        settingsMenuObject.SetActive(false);

        if (!mainMenu) //will only have a back button in-game, not on the main menu
        {
            controlsUI.GetComponentInChildren<Button>().gameObject.SetActive(true);
        }
        else 
        {
            controlsUI.GetComponentInChildren<Button>().gameObject.SetActive(false);
        }
    }

    public void CloseControlsUI() //closes the controls screen
    {
        controlsUI.SetActive(false);
        OpenSettingsMenu();
    }

    public void QuitGame() //this method starts the quit process
    {
        if (!quitGagPlaying) //a voiceline plays before the game quits
        {
            quitGagPlaying = true;
            StartCoroutine("QuitGag");
        }
    }

    public void UpdateSliders() //this method updates the sliders to reflect the float values of the volume, and the mouse sens. gets called when the settings are loaded, or if another scene is loaded.
    {
        AudioSettings audio = FindObjectOfType<AudioSettings>();

        Slider[] sliders = settingsMenuObject.GetComponentsInChildren<Slider>();

        for(int i = 0; i < sliders.Length; i++)
        {
            switch (sliders[i].name) //switch statement to find which slider to update
            {
                case "Music Slider":
                    {
                        sliders[i].value = audio.musicVolume;
                        break;
                    }
                case "SFX Slider":
                    {
                        sliders[i].value = audio.sfxVolume;
                        break;
                    }
                case "Voice Slider":
                    {
                        sliders[i].value = audio.voiceVolume;
                        break;
                    }
                case "Mouse Slider":
                    {
                        sliders[i].value = playerController.mouseSensitivity;
                        break;
                    }
            }
        }
    }

    public void LoadSettings() //this method loads the saved audio and mouse settings
    {
        SettingsData data = SettingsSaveSystem.LoadSettings();

        if(data != null) //if it has data, it loads
        {
            audioSettings.musicVolume = data.musicVolume;
            audioSettings.sfxVolume = data.sfxVolume;
            audioSettings.voiceVolume = data.voiceVolume;
            playerController.mouseSensitivity = data.mouseSens;
        }
    } 

    public void SaveSettings() //this method saves the settings 
    {
        SettingsSaveSystem.SaveSettings(audioSettings, playerController);
    }

    IEnumerator ReadingControls() //this co-routine happens while the game is loading, in an a-sync fashion
    {
        loadingScene = SceneManager.LoadSceneAsync("Submission", LoadSceneMode.Single);
        loadingScene.allowSceneActivation = false;

        while (!loadingScene.isDone) //while it is not finished loading, the animation plays
        {
            Debug.Log(loadingScene.progress);
            if(loadingScene.progress >= 0.9f) //after it reaches 90%, the start game text appears, and the player can finish the loading process by pressing space.
            {
                pressSpace.SetActive(true);
                screenFade.SetActive(true);
                if (Input.GetKey(KeyCode.Space))
                {
                    pressSpace.SetActive(false);
                    screenFade.GetComponent<Animator>().SetTrigger("Fade");
                    StartCoroutine(FadeLoading());
                }
            }
            yield return null;
        }
       
    }

    IEnumerator FadeLoading()
    {
        yield return new WaitForSeconds(3);
        loadingScene.allowSceneActivation = true;
        
    }

    public IEnumerator QuitGag() //this is the quit gag
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance("event:/MR_C_Random/I_Quit");

        eventInstance.start();

        yield return new WaitForSecondsRealtime(7f);

        Debug.Log("quit");

        //PLAYBACK_STATE pbState;
        //eventInstance.getPlaybackState(out pbState);

        //Debug.Log(pbState);

        //while (pbState != PLAYBACK_STATE.PLAYING && pbState != PLAYBACK_STATE.STARTING)
        //{
        //    Debug.Log(pbState);
        //}

        //Debug.Log(pbState);
        Application.Quit();
        //Debug.Log("quit");
        //yield return null;
    }

    public void ResolutionChange(int val) //this method handles the resolution changing when the player clicks the buttons in the settings
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
