using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class AudioSettings : MonoBehaviour //this script handles the volume of the audio in the game
{
    EventInstance sfxVolumePreview, voiceVolumePreview;

    Bus musicP, sfxP, voiceP, sfxUP, voiceUP;

    public float musicVolume = 0.5f, sfxVolume = 0.5f, voiceVolume = 0.5f;

    public bool mainMenu;

    void Awake()
    {
        sfxP = RuntimeManager.GetBus("bus:/Pause Group/SFX");
        musicP = RuntimeManager.GetBus("bus:/Pause Group/Music");
        voiceP = RuntimeManager.GetBus("bus:/Pause Group/Voice");

        sfxUP = RuntimeManager.GetBus("bus:/Unpause Group/SFX");
        voiceUP = RuntimeManager.GetBus("bus:/Unpause Group/Voice");

        sfxVolumePreview = RuntimeManager.CreateInstance("event:/UI_Multimedia/Tutorial_Info");
        voiceVolumePreview = RuntimeManager.CreateInstance("event:/MR_C_Attack/Mr_C_Attack");
    }

    void Update()
    {
        musicP.setVolume(musicVolume);
        sfxP.setVolume(sfxVolume);
        voiceP.setVolume(voiceVolume);

        sfxUP.setVolume(sfxVolume);
        voiceUP.setVolume(voiceVolume);
    }

    public void MuteControl(bool mute, int whichBusGroup) //1 = all, 2 = Pause Group, 3 = Unpause Group
    { //different groups need different muting so that certain sounds can play while the game is paused.
        if(whichBusGroup == 1)
        {
            musicP.setMute(mute);
            sfxP.setMute(mute);
            voiceP.setMute(mute);
            sfxUP.setMute(mute);
            voiceUP.setMute(mute);
        }
        else if(whichBusGroup == 2)
        {
            musicP.setMute(mute);
            sfxP.setMute(mute);
            voiceP.setMute(mute);
        }
        else if(whichBusGroup == 3)
        {
            sfxUP.setMute(mute);
            voiceUP.setMute(mute);
        }
    }

    public void MusicVolumeLevel(float newMusicVolume) //this is called by sliders
    {
        musicVolume = newMusicVolume;
    }

    public void SFXVolumeLevel(float newSFXVolume) //called by sliders
    {
        sfxVolume = newSFXVolume;

        PLAYBACK_STATE pbState; 
        sfxVolumePreview.getPlaybackState(out pbState); //gets the current playback state of the preview event

        if(pbState != PLAYBACK_STATE.PLAYING) //if it is not playing, it will play the preview
        {
            sfxVolumePreview.start();
        }
    }

    public void VoiceVolumeLevel(float newVoiceVolume) //same as above
    {
        voiceVolume = newVoiceVolume;

        PLAYBACK_STATE pbState;
        voiceVolumePreview.getPlaybackState(out pbState);

        if (pbState != PLAYBACK_STATE.PLAYING)
        {
            voiceVolumePreview.start();
        }
    }
}
