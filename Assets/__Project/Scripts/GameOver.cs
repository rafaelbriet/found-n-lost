using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField]
    private GameReport gameReport;
    [SerializeField]
    private TextMeshProUGUI message;

    private void Awake()
    {
        message.text = $"After fight so many ghosts for {gameReport.TotalNightsWorked} {PluralizeNight()}, you decided to quit this job...";
    }

    private string PluralizeNight()
    {
        return gameReport.TotalNightsWorked > 1 ? "nights" : "night";
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
