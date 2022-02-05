using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingsData //this script is serializable so that the values within can be converted to binary for saving
{
    public float musicVolume, sfxVolume, voiceVolume, mouseSens;

    public SettingsData (AudioSettings audioSettings, Player_Controller player) //updates the values in here to reflect the ones in the player controller and the audio settings
    {
        musicVolume = audioSettings.musicVolume;
        sfxVolume = audioSettings.sfxVolume;
        voiceVolume = audioSettings.voiceVolume;
        mouseSens = player.mouseSensitivity;
    }
}
