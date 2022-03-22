using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameReport", menuName = "Game Report")]
public class GameReport : ScriptableObject
{
    public int TotalNightsWorked { get; set; }
    public int GhostsKilled { get; set; }

    private void OnDisable()
    {
        TotalNightsWorked = default;
        GhostsKilled = default;
    }

    public int TotalScore()
    {
        if (GhostsKilled == 0)
        {
            return TotalNightsWorked;
        }

        return TotalNightsWorked * GhostsKilled;
    }
}
