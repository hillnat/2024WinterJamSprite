using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsHandler : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    private string masterVolumeParam = "MasterVolume";
    private string musicVolumeParam = "MusicVolume";
    private string sfxVolumeParam = "SFXVolume";
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    public void SetMasterVolume(float sliderValue)
    {
        GameManager.masterVolume = sliderValue;
        SetVolume(masterVolumeParam, sliderValue);
    }

    public void SetMusicVolume(float sliderValue)
    {
        GameManager.musicVolume = sliderValue;
        SetVolume(musicVolumeParam, sliderValue);
    }

    public void SetSFXVolume(float sliderValue)
    {
        GameManager.sfxVolume = sliderValue;
        SetVolume(sfxVolumeParam, sliderValue);
    }

    private void SetVolume(string parameterName, float sliderValue)
    {
        float volume = Mathf.Log10(sliderValue) * 20; // Converting slider value to decibels

        if (sliderValue == 0) { volume = -80f; } // Set to almost silent

        audioMixer.SetFloat(parameterName, volume);
    }

    public void FetchSliderValues(){
        masterVolumeSlider.value = GameManager.masterVolume;
        musicVolumeSlider.value = GameManager.musicVolume;
        sfxVolumeSlider.value = GameManager.sfxVolume;
    }
}
