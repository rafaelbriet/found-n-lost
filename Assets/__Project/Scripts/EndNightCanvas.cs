using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndNightCanvas : MonoBehaviour
{
    [SerializeField]
    private WarehouseManager warehouseManager;
    [SerializeField]
    private TextMeshProUGUI message;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        HideCanvasGroup();
    }

    private void OnEnable()
    {
        warehouseManager.NightStarted += OnNightStarted;
        warehouseManager.NightEnded += OnNightEnded;
    }

    private void OnDisable()
    {
        warehouseManager.NightStarted -= OnNightStarted;
        warehouseManager.NightEnded -= OnNightEnded;
    }

    private void OnNightStarted(object sender, EventArgs e)
    {
        HideCanvasGroup();
    }

    private void OnNightEnded(object sender, System.EventArgs e)
    {
        ShowCanvasGroup();

        message.text = $"Another night of work is over. You're working for {warehouseManager.CurrentNight} nights...";
    }

    private void HideCanvasGroup()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    private void ShowCanvasGroup()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
}
