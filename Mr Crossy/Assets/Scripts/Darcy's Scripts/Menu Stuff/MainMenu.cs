using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;
using TMPro;

public class MainMenu : MonoBehaviour
{
    TutorialController tutorialController;

    [SerializeField]
    GameObject menuObject, settingsObject;

    GameObject[] volumeObjects;

    [SerializeField]
    TextMeshProUGUI musicText, sfxText, voiceText;

    float musicVolume = 0.5f, sfxVolume = 0.5f, voiceVolume = 0.5f;

    void Start()
    {
        tutorialController = FindObjectOfType<TutorialController>();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Tutorial Testing");
    }

    public void OpenSettings()
    {
        menuObject.SetActive(false);
        settingsObject.SetActive(true);
    }

    public void CloseSettings()
    {
        menuObject.SetActive(true);
        settingsObject.SetActive(false);
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

                    if(voiceVolume < 0.99)
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

                    if(sfxVolume > 0.01)
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
