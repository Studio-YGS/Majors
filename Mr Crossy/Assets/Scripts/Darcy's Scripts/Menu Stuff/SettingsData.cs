using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingsData
{
    public float musicVolume, sfxVolume, voiceVolume, mouseSens;

    public SettingsData (AudioSettings audioSettings, Player_Controller player)
    {
        musicVolume = audioSettings.musicVolume;
        sfxVolume = audioSettings.sfxVolume;
        voiceVolume = audioSettings.voiceVolume;
        mouseSens = player.mouseSensitivity;
    }
}
