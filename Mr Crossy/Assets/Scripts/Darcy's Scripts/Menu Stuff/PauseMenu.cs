using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    Player_Controller playerController;

    JournalOnSwitch journalOnSwitch;

    JournalController journalController;

    StudioEventEmitter[] studioEventEmitters;

    GameObject[] volumeObjects;

    [SerializeField]
    GameObject pauseMenuObject, settingsMenuObject;

    public GameObject streetName;

    [SerializeField]
    TextMeshProUGUI musicText, sfxText, voiceText;

    float defTimeScale, musicVolume = 0.5f, sfxVolume = 0.5f, voiceVolume = 0.5f;

    void Start()
    {
        playerController = FindObjectOfType<Player_Controller>();
        journalOnSwitch = FindObjectOfType<JournalOnSwitch>();
        journalController = FindObjectOfType<JournalController>();
        defTimeScale = Time.timeScale;
    }

    void Update()
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

    public void OpenPauseMenu()
    {
        pauseMenuObject.SetActive(true);

        playerController.DisableController();
        playerController.inJournal = false;

        journalController.DisableJournal();

        journalOnSwitch.HideTab();

        streetName.SetActive(false);

        Time.timeScale = 0f;

        studioEventEmitters = FindObjectsOfType<StudioEventEmitter>();

        for(int i = 0; i < studioEventEmitters.Length; i++)
        {
            studioEventEmitters[i].EventInstance.setPaused(true);
        }
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

        studioEventEmitters = FindObjectsOfType<StudioEventEmitter>();

        for (int i = 0; i < studioEventEmitters.Length; i++)
        {
            studioEventEmitters[i].EventInstance.setPaused(false);
        }
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

    public void IncreaseVolume(string volType)
    {
        switch (volType)
        {
            case "Music":
                {
                    volumeObjects = GameObject.FindGameObjectsWithTag("Music");

                    if (musicVolume < 0.99)
                    {
                        musicVolume += 0.01f;

                        for (int i = 0; i < volumeObjects.Length; i++)
                        {
                            volumeObjects[i].GetComponent<StudioEventEmitter>().EventInstance.setVolume(musicVolume);
                            musicText.text = (Mathf.Round(musicVolume * 100)).ToString();
                        }
                    }
                    break;
                }
            case "SFX":
                {
                    volumeObjects = GameObject.FindGameObjectsWithTag("SFX");

                    if (sfxVolume < 0.99)
                    {
                        sfxVolume += 0.01f;

                        for (int i = 0; i < volumeObjects.Length; i++)
                        {
                            volumeObjects[i].GetComponent<StudioEventEmitter>().EventInstance.setVolume(sfxVolume);
                            sfxText.text = (Mathf.Round(sfxVolume * 100)).ToString();
                        }
                    }
                    break;
                }
            case "Voice":
                {
                    volumeObjects = GameObject.FindGameObjectsWithTag("Voice");

                    if (voiceVolume < 0.99)
                    {
                        voiceVolume += 0.01f;

                        for (int i = 0; i < volumeObjects.Length; i++)
                        {
                            volumeObjects[i].GetComponent<StudioEventEmitter>().EventInstance.setVolume(voiceVolume);
                            voiceText.text = (Mathf.Round(voiceVolume * 100)).ToString();
                        }
                    }
                    break;
                }
        }
    }
    public void DecreaseVolume(string volType)
    {
        switch (volType)
        {
            case "Music":
                {
                    volumeObjects = GameObject.FindGameObjectsWithTag("Music");

                    if (musicVolume > 0.01)
                    {
                        musicVolume -= 0.01f;

                        for (int i = 0; i < volumeObjects.Length; i++)
                        {
                            volumeObjects[i].GetComponent<StudioEventEmitter>().EventInstance.setVolume(musicVolume);
                            musicText.text = (Mathf.Round(musicVolume * 100)).ToString();
                        }
                    }
                    break;
                }
            case "SFX":
                {
                    volumeObjects = GameObject.FindGameObjectsWithTag("SFX");

                    if (sfxVolume > 0.01)
                    {
                        sfxVolume -= 0.01f;

                        for (int i = 0; i < volumeObjects.Length; i++)
                        {
                            volumeObjects[i].GetComponent<StudioEventEmitter>().EventInstance.setVolume(sfxVolume);
                            sfxText.text = (Mathf.Round(sfxVolume * 100)).ToString();
                        }
                    }
                    break;
                }
            case "Voice":
                {
                    volumeObjects = GameObject.FindGameObjectsWithTag("Voice");

                    if (voiceVolume > 0.01)
                    {
                        voiceVolume -= 0.01f;

                        for (int i = 0; i < volumeObjects.Length; i++)
                        {
                            volumeObjects[i].GetComponent<StudioEventEmitter>().EventInstance.setVolume(voiceVolume);
                            voiceText.text = (Mathf.Round(voiceVolume * 100)).ToString();
                        }
                    }
                    break;
                }
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
