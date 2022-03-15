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
    private AudioMixer audioMixer;

    private void Start()
    {
        musicVolumeSlide.value = gameSettings.MusicVolume;
    }

    public void ChangeMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20f);
        gameSettings.MusicVolume = volume;
    }
}
