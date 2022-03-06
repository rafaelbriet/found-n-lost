using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameReport", menuName = "Game Report")]
public class GameReport : ScriptableObject
{
    public int TotalNightsWorked { get; set; }

    private void OnDisable()
    {
        TotalNightsWorked = default;
    }
}
