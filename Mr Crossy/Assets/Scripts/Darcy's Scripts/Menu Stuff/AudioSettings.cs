using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class AudioSettings : MonoBehaviour
{
    EventInstance sfxVolumePreview, voiceVolumePreview;

    Player_Controller player;

    MenuManager menuManager;

    Bus music, sfx, voice;

    public float musicVolume = 0.5f, sfxVolume = 0.5f, voiceVolume = 0.5f;

    public bool mainMenu;

    void Awake()
    {
        player = FindObjectOfType<Player_Controller>();

        if (mainMenu)
        {
            DontDestroyOnLoad(gameObject);
        }

        if(GameObject.Find("Audio Settings") && !mainMenu)
        {
            musicVolume = GameObject.Find("Audio Settings").GetComponent<AudioSettings>().musicVolume;
            sfxVolume = GameObject.Find("Audio Settings").GetComponent<AudioSettings>().sfxVolume;
            voiceVolume = GameObject.Find("Audio Settings").GetComponent<AudioSettings>().voiceVolume;

            menuManager = FindObjectOfType<MenuManager>();

            menuManager.UpdateSliders();

            Destroy(GameObject.Find("Audio Settings"));
        }

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

        voiceVolumePreview.setPaused(false);

        PLAYBACK_STATE pbState;
        voiceVolumePreview.getPlaybackState(out pbState);

        if (pbState != PLAYBACK_STATE.PLAYING)
        {
            voiceVolumePreview.start();
        }
    }
}
