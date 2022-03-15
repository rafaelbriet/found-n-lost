using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Game Settings")]
public class GameSettings : ScriptableObject
{
    [SerializeField]
    [Range(0f, 1f)]
    private float musicVolume = 1f;

    public float MusicVolume { get => musicVolume; set => musicVolume = value; }

    private void OnDisable()
    {
        
    }
}
