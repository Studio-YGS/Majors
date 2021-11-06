using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

        if (mainMenu && Input.GetKey(KeyCode.Space) && pressSpace.activeInHierarchy)
        {
            SceneManager.LoadScene("Main_Cael");
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
        yield return new WaitForSeconds(5f);

        pressSpace.SetActive(true);
        StopCoroutine(ReadingControls());
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
