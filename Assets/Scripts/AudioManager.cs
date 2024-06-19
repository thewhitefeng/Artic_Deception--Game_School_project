using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class AudioManager : MonoBehaviour
{
    public Slider MasterSlider;
    public Slider MusicSlider;
    public Slider SFXSlider;
    public AudioMixer am;


    public void SetMasterVolume()
    {
        float volume = MasterSlider.value;
        am.SetFloat("master", Mathf.Log10(volume) * 20);
    }

    public void SetMusicVolume()
    {
        float volumeMusic = MusicSlider.value;
        am.SetFloat("music", Mathf.Log10(volumeMusic) * 20);
    }

    public void SetSFXVolume()
    {
        float volumeSFX = SFXSlider.value;
        am.SetFloat("sfx", Mathf.Log10(volumeSFX) * 20);
    }
}
