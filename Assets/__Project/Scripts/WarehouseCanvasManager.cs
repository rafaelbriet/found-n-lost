using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WarehouseCanvasManager : MonoBehaviour
{
    [Header("General")]
    [SerializeField]
    private WarehouseManager warehouseManager;
    [SerializeField]
    private GameReport gameReport;
    [Header("Start of the night")]
    [SerializeField]
    private CanvasGroup startOfTheNightCanvasGroup;
    [SerializeField]
    private float introDuration = 3f;
    [Header("End of the night")]
    [SerializeField]
    private CanvasGroup endOfTheNightCanvasGroup;
    [SerializeField]
    private TextMeshProUGUI message;
    [SerializeField]
    private TextMeshProUGUI totalScore;

    private void Awake()
    {
        HideCanvasGroup(endOfTheNightCanvasGroup);
        ShowCanvasGroup(startOfTheNightCanvasGroup);
        StartCoroutine(StartOfTheNightIntroCoroutine());
    }

    private void OnEnable()
    {
        warehouseManager.NightEnded += OnNightEnded;
    }

    private void OnDisable()
    {
        warehouseManager.NightEnded -= OnNightEnded;
    }

    public void ContinueToNextNight()
    {
        StartCoroutine(StartOfTheNightIntroCoroutine());
        HideCanvasGroup(endOfTheNightCanvasGroup);
    }

    private void OnNightEnded(object sender, System.EventArgs e)
    {
        ShowCanvasGroup(endOfTheNightCanvasGroup);

        message.text = $"Another night of work is over. " +
            $"You're working for {warehouseManager.CurrentNight} {Pluralize(warehouseManager.CurrentNight, "night", "nights")} " +
            $"and killed {gameReport.GhostsKilled} {Pluralize(gameReport.GhostsKilled, "ghost", "ghosts")}";

        totalScore.text = gameReport.TotalScore().ToString();
    }

    private string Pluralize(int amount, string singular, string plural)
    {
        return amount > 1 ? plural : singular;
    }

    private IEnumerator StartOfTheNightIntroCoroutine()
    {
        ShowCanvasGroup(startOfTheNightCanvasGroup);

        yield return new WaitForSeconds(introDuration);

        HideCanvasGroup(startOfTheNightCanvasGroup);

        warehouseManager.StartNight();
    }

    private void HideCanvasGroup(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    private void ShowCanvasGroup(CanvasGroup canvasGroup)
    {
        if (canvasGroup.gameObject.activeInHierarchy == false)
        {
            canvasGroup.gameObject.SetActive(true);
        }

        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
}
