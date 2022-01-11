using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class AudioSettings : MonoBehaviour
{
    EventInstance sfxVolumePreview, voiceVolumePreview;

    MenuManager menuManager;

    Player_Controller player;

    Bus music, sfx, voice;

    public float musicVolume = 0.5f, sfxVolume = 0.5f, voiceVolume = 0.5f;

    public bool mainMenu;

    void Awake()
    {
        menuManager = FindObjectOfType<MenuManager>();
        player = FindObjectOfType<Player_Controller>();

        sfx = RuntimeManager.GetBus("bus:/SFX");
        music = RuntimeManager.GetBus("bus:/Music");
        voice = RuntimeManager.GetBus("bus:/Voice");
        sfxVolumePreview = RuntimeManager.CreateInstance("event:/UI_Multimedia/Tutorial_Info");
        voiceVolumePreview = RuntimeManager.CreateInstance("event:/MR_C_Attack/Mr_C_Attack");
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

        //PLAYBACK_STATE pbState;
        //sfxVolumePreview.getPlaybackState(out pbState);

        //if(pbState != PLAYBACK_STATE.PLAYING)
        //{
        //    sfxVolumePreview.start();
        //}
    }

    public void VoiceVolumeLevel(float newVoiceVolume)
    {
        voiceVolume = newVoiceVolume;

        //PLAYBACK_STATE pbState;
        //voiceVolumePreview.getPlaybackState(out pbState);

        //if (pbState != PLAYBACK_STATE.PLAYING)
        //{
        //    voiceVolumePreview.start();
        //}
    }
}
