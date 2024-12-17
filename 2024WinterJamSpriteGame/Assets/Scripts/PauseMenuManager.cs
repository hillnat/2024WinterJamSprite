using UnityEngine;
using UnityEngine.Audio;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private string masterVolumeParam = "MasterVolume";
    [SerializeField] private string musicVolumeParam = "MusicVolume";
    [SerializeField] private string sfxVolumeParam = "SFXVolume";


    public void SetMasterVolume(float sliderValue)
    {
        SetVolume(masterVolumeParam, sliderValue);
    }

    public void SetMusicVolume(float sliderValue)
    {
        SetVolume(musicVolumeParam, sliderValue);
    }

    public void SetSFXVolume(float sliderValue)
    {
        SetVolume(sfxVolumeParam, sliderValue);
    }


    private void SetVolume(string parameterName, float sliderValue)
    {
        float volume = Mathf.Log10(sliderValue) * 20; // Converting slider value to decibels

        if (sliderValue == 0) { volume = -80f; } // Set to almost silent

        audioMixer.SetFloat(parameterName, volume);
    }
}
