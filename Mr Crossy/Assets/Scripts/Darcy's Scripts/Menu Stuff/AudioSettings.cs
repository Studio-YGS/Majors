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
        sfx = RuntimeManager.GetBus("bus:/Master/SFX");
        music = RuntimeManager.GetBus("bus:/Master/Music");
        voice = RuntimeManager.GetBus("bus:/Master/Voice");
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
        sfxVolume = newSFXVolume;

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
