using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioVolumeManager : MonoBehaviour
{
    [SerializeField]
    private GameSettings gameSettings;
    [SerializeField]
    private Slider musicVolumeSlide;
    [SerializeField]
    private Slider sfxVolumeSlide;
    [SerializeField]
    private AudioMixer audioMixer;

    private void Start()
    {
        musicVolumeSlide.value = gameSettings.MusicVolume;
        sfxVolumeSlide.value = gameSettings.SfxVolume;
    }

    public void ChangeMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20f);
        gameSettings.MusicVolume = volume;
    }

    public void ChangeSfxVolume(float volume)
    {
        audioMixer.SetFloat("SfxVolume", Mathf.Log10(volume) * 20f);
        gameSettings.SfxVolume = volume;
    }
}
