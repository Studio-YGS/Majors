using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using FMODUnity;
using FMOD.Studio;

public class MenuManager : MonoBehaviour
{
    Player_Controller playerController;

    JournalOnSwitch journalOnSwitch;

    JournalController journalController;

    AudioSettings audioSettings;

    [SerializeField]
    GameObject pauseMenuObject, settingsMenuObject, mainMenuObject, pressSpace, controlsUI, loadingAni;

    public GameObject streetName;

    float defTimeScale;

    public bool mainMenu;
    [HideInInspector] 
    public bool menuOpen;

    bool dontEnable, quitGagPlaying;

    AsyncOperation loadingScene;

    public GameObject screenFade;

    void Start()
    {
        if (!mainMenu)
        {
            playerController = FindObjectOfType<Player_Controller>();
            journalOnSwitch = FindObjectOfType<JournalOnSwitch>();
            journalController = FindObjectOfType<JournalController>();
            defTimeScale = Time.timeScale;
        }

        audioSettings = FindObjectOfType<AudioSettings>();
        audioSettings.MuteControl(true, 3);

        LoadSettings();
        UpdateSliders();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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

    public void OpenPauseMenu()
    {
        pauseMenuObject.SetActive(true);
        menuOpen = true;
        playerController.DisableController();
        playerController.inJournal = false;

        audioSettings.MuteControl(false, 3);

        if (journalController.disabled)
        {
            dontEnable = true;
        }
        else
        {
            dontEnable = false;
        }

        journalController.DisableJournal();

        journalOnSwitch.HideTab();

        for(int i = 0; i < GetComponent<TutorialController>().objectsToSwitchOn.Length; i++)
        {
            GetComponent<TutorialController>().objectsToSwitchOn[i].SetActive(false);
        }

        //RuntimeManager.PauseAllEvents(true);
        RuntimeManager.GetBus("bus:/Pause Group").setPaused(true);

        //streetName.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Time.timeScale = 0f;
    }

    public void ClosePauseMenu()
    {
        pauseMenuObject.SetActive(false);
        menuOpen = false;
        playerController.EnableController();
        playerController.inJournal = false;

        if (!dontEnable)
        {
            journalController.EnableJournal();
            journalOnSwitch.ShowTab();
        }

        for (int i = 0; i < GetComponent<TutorialController>().objectsToSwitchOn.Length; i++)
        {
            GetComponent<TutorialController>().objectsToSwitchOn[i].SetActive(true);
        }

        //RuntimeManager.PauseAllEvents(false);
        RuntimeManager.GetBus("bus:/Pause Group").setPaused(false);

        //streetName.SetActive(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = defTimeScale;
    }

    public void StartGame()
    {
        OpenControlsUI();

        loadingAni.SetActive(true);

        mainMenuObject.SetActive(false);

        StartCoroutine(ReadingControls());
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

    public void OpenControlsUI()
    {
        controlsUI.SetActive(true);
        settingsMenuObject.SetActive(false);

        if (!mainMenu)
        {
            controlsUI.GetComponentInChildren<Button>().gameObject.SetActive(true);
        }
        else
        {
            controlsUI.GetComponentInChildren<Button>().gameObject.SetActive(false);
        }
    }

    public void CloseControlsUI()
    {
        controlsUI.SetActive(false);
        OpenSettingsMenu();
    }

    public void QuitGame()
    {
        if (!quitGagPlaying)
        {
            quitGagPlaying = true;
            StartCoroutine("QuitGag");
        }
    }

    public void UpdateSliders()
    {
        AudioSettings audio = FindObjectOfType<AudioSettings>();

        Slider[] sliders = settingsMenuObject.GetComponentsInChildren<Slider>();

        for(int i = 0; i < sliders.Length; i++)
        {
            switch (sliders[i].name)
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

    public void LoadSettings()
    {
        SettingsData data = SettingsSaveSystem.LoadSettings();

        if(data != null)
        {
            audioSettings.musicVolume = data.musicVolume;
            audioSettings.sfxVolume = data.sfxVolume;
            audioSettings.voiceVolume = data.voiceVolume;
            playerController.mouseSensitivity = data.mouseSens;
        }
    } 

    public void SaveSettings()
    {
        SettingsSaveSystem.SaveSettings(audioSettings, playerController);
    }

    IEnumerator ReadingControls()
    {
        loadingScene = SceneManager.LoadSceneAsync("Submission", LoadSceneMode.Single);
        loadingScene.allowSceneActivation = false;

        while (!loadingScene.isDone)
        {
            Debug.Log(loadingScene.progress);
            if(loadingScene.progress >= 0.9f)
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

    public IEnumerator QuitGag()
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
