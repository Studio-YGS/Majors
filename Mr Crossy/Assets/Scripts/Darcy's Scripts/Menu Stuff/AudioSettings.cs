using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class AudioSettings : MonoBehaviour
{
    EventInstance sfxVolumePreview;

    Bus music, sfx, voice;

    float musicVolume = 0.5f, sfxVolume = 0.5f, voiceVolume = 0.5f;

    void Awake()
    {
        music = RuntimeManager.GetBus("bus:/Music");
        sfx = RuntimeManager.GetBus("bus:/SFX");
        voice = RuntimeManager.GetBus("bus:/Voice");
        sfxVolumePreview = RuntimeManager.CreateInstance("event:/UI_Multimedia/Tutorial_Info");
    }

    void Update()
    {
        music.setVolume(musicVolume);
        sfx.setVolume(sfxVolume);
        voice.setVolume(voiceVolume);
    }

    public void MusicVolumeLevel(float newMusicVolume)
    {
        musicVolume = newMusicVolume;
    }

    public void SFXVolumeLevel(float newSFXVolume)
    {
        Debug.Log("Trying to change SFX Volume from: " + sfxVolume);
        sfxVolume = newSFXVolume;
        Debug.Log("Have changed the previous value to: " + newSFXVolume + ", confirming: " + sfxVolume);

        music.getVolume(out float vol);
        Debug.Log("The actual SFX volume is now: " + vol);

        PLAYBACK_STATE pbState;
        sfxVolumePreview.getPlaybackState(out pbState);

        if(pbState != PLAYBACK_STATE.PLAYING)
        {
            sfxVolumePreview.start();
        }
    }

    public void VoiceVolumeLevel(float newVoiceVolume)
    {
        voiceVolume = newVoiceVolume;
    }
}
