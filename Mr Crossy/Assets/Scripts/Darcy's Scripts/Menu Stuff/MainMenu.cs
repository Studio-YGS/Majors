using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    GameObject menuObject, settingsObject;

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
}
