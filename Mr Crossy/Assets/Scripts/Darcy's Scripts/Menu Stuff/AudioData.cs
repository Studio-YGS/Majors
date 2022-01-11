using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioData
{
    public float musicVolume, sfxVolume, voiceVolume;

    public AudioData (AudioSettings audioSettings)
    {
        musicVolume = audioSettings.musicVolume;
        sfxVolume = audioSettings.sfxVolume;
        voiceVolume = audioSettings.voiceVolume;
    }
}
