using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    Player_Controller playerController;

    JournalOnSwitch journalOnSwitch;

    StudioEventEmitter[] studioEventEmitters;

    GameObject[] volumeObjects;

    [SerializeField]
    GameObject pauseMenuObject, settingsMenuObject;

    [SerializeField]
    TextMeshProUGUI musicText, sfxText, voiceText;

    float defTimeScale, musicVolume = 0.5f, sfxVolume = 0.5f, voiceVolume = 0.5f;

    void Start()
    {
        playerController = FindObjectOfType<Player_Controller>();
        journalOnSwitch = FindObjectOfType<JournalOnSwitch>();
        defTimeScale = Time.timeScale;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
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

        journalOnSwitch.HideTab();

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

        journalOnSwitch.ShowTab();

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

                    for(int i = 0; i < volumeObjects.Length; i++)
                    {
                        if (volumeObjects[i].GetComponent<StudioEventEmitter>() && musicVolume < 0.99)
                        {
                            volumeObjects[i].GetComponent<StudioEventEmitter>().EventInstance.setVolume(musicVolume += 0.01f);
                            musicText.text = (Mathf.Round(musicVolume * 100)).ToString();
                        }
                    }

                    break;
                }
            case "SFX":
                {
                    volumeObjects = GameObject.FindGameObjectsWithTag("SFX");

                    for (int i = 0; i < volumeObjects.Length; i++)
                    {
                        if (volumeObjects[i].GetComponent<StudioEventEmitter>() && sfxVolume < 0.99)
                        {
                            volumeObjects[i].GetComponent<StudioEventEmitter>().EventInstance.setVolume(sfxVolume += 0.01f);
                            sfxText.text = (Mathf.Round(sfxVolume * 100)).ToString();
                        }
                    }
                    break;
                }
            case "Voice":
                {
                    volumeObjects = GameObject.FindGameObjectsWithTag("Voice");

                    for (int i = 0; i < volumeObjects.Length; i++)
                    {
                        if (volumeObjects[i].GetComponent<StudioEventEmitter>() && voiceVolume < 0.99)
                        {
                            volumeObjects[i].GetComponent<StudioEventEmitter>().EventInstance.setVolume(voiceVolume += 0.01f);
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

                    for (int i = 0; i < volumeObjects.Length; i++)
                    {
                        if (volumeObjects[i].GetComponent<StudioEventEmitter>() && musicVolume > 0.01)
                        {
                            volumeObjects[i].GetComponent<StudioEventEmitter>().EventInstance.setVolume(musicVolume -= 0.01f);
                            musicText.text = (Mathf.Round(musicVolume * 100)).ToString();
                        }
                    }

                    break;
                }
            case "SFX":
                {
                    volumeObjects = GameObject.FindGameObjectsWithTag("SFX");

                    for (int i = 0; i < volumeObjects.Length; i++)
                    {
                        if (volumeObjects[i].GetComponent<StudioEventEmitter>() && sfxVolume > 0.01)
                        {
                            volumeObjects[i].GetComponent<StudioEventEmitter>().EventInstance.setVolume(sfxVolume -= 0.01f);
                            sfxText.text = (Mathf.Round(sfxVolume * 100)).ToString();
                        }
                    }
                    break;
                }
            case "Voice":
                {
                    volumeObjects = GameObject.FindGameObjectsWithTag("Voice");

                    for (int i = 0; i < volumeObjects.Length; i++)
                    {
                        if (volumeObjects[i].GetComponent<StudioEventEmitter>() && voiceVolume > 0.01)
                        {
                            volumeObjects[i].GetComponent<StudioEventEmitter>().EventInstance.setVolume(voiceVolume -= 0.01f);
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
