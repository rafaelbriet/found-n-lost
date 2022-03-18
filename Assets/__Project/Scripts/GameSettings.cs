using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Game Settings")]
public class GameSettings : ScriptableObject
{
    [SerializeField]
    [Range(0.0001f, 1f)]
    private float musicVolume = 1f;
    [SerializeField]
    [Range(0.0001f, 1f)]
    private float sfxVolume = 1f;

    public float MusicVolume { get => musicVolume; set => musicVolume = value; }
    public float SfxVolume { get => sfxVolume; set => sfxVolume = value; }

    private void OnDisable()
    {
        
    }
}
