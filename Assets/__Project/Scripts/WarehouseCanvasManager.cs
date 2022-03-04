using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WarehouseCanvasManager : MonoBehaviour
{
    [Header("General")]
    [SerializeField]
    private WarehouseManager warehouseManager;
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

    private void Awake()
    {
        HideCanvasGroup(endOfTheNightCanvasGroup);
        StartCoroutine(StartOfTheNightIntroCoroutine());
    }

    private void OnEnable()
    {
        //warehouseManager.NightStarted += OnNightStarted;
        warehouseManager.NightEnded += OnNightEnded;
    }

    private void OnDisable()
    {
        //warehouseManager.NightStarted -= OnNightStarted;
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

        message.text = $"Another night of work is over. You're working for {warehouseManager.CurrentNight} nights...";
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
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
}
