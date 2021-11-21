using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using FMODUnity;

public class MenuManager : MonoBehaviour
{
    Player_Controller playerController;

    JournalOnSwitch journalOnSwitch;

    JournalController journalController;

    [SerializeField]
    GameObject pauseMenuObject, settingsMenuObject, mainMenuObject, pressSpace, controlsUI, loadingAni;

    public GameObject streetName;

    float defTimeScale;

    public bool mainMenu;
    [HideInInspector] 
    public bool menuOpen;

    bool dontEnable;

    AsyncOperation loadingScene;

    void Start()
    {
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
        if (!mainMenu && !FindObjectOfType<CrossKeyManager>().puzzleOn)
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
        menuOpen = true;
        playerController.DisableController();
        playerController.inJournal = false;

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

        RuntimeManager.PauseAllEvents(true);

        streetName.SetActive(false);
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

        RuntimeManager.PauseAllEvents(false);

        streetName.SetActive(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        Time.timeScale = defTimeScale;
    }

    public void StartGame()
    {
        controlsUI.SetActive(true);

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

    public void QuitGame()
    {
        Application.Quit();
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
            }
        }
    }

    IEnumerator ReadingControls()
    {
        loadingScene = SceneManager.LoadSceneAsync("Main_Cael", LoadSceneMode.Single);
        loadingScene.allowSceneActivation = false;

        while (!loadingScene.isDone)
        {
            Debug.Log(loadingScene.progress);
            if(loadingScene.progress >= 0.9f)
            {
                pressSpace.SetActive(true);
                if (Input.GetKey(KeyCode.Space))
                {
                    loadingScene.allowSceneActivation = true;
                }
            }
            yield return null;
        }
       
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
