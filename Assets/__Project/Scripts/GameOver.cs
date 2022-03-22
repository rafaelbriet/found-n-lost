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
    [SerializeField]
    private TextMeshProUGUI totalScore;

    private void Awake()
    {
        message.text = $"After fighting and killing {gameReport.GhostsKilled} {Pluralize(gameReport.GhostsKilled, "ghost", "ghosts")} during " +
            $"{gameReport.TotalNightsWorked} {Pluralize(gameReport.TotalNightsWorked, "night", "nights")}, you decided to quit this job.";

        totalScore.text = gameReport.TotalScore().ToString();
    }

    private string Pluralize(int amount, string singular, string plural)
    {
        return amount > 1 ? plural : singular;
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
