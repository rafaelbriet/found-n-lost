using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class PauseCanvasManager : MonoBehaviour
{
    [SerializeField]
    private WarehouseManager warehouseManager;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        HideCanvas();
    }

    private void OnEnable()
    {
        warehouseManager.Paused += OnPaused;
        warehouseManager.Unpaused += OnUnpaused;
    }

    private void OnDisable()
    {
        warehouseManager.Paused -= OnPaused;
        warehouseManager.Unpaused -= OnUnpaused;
    }

    public void Unpause()
    {
        warehouseManager.UnpauseGame();
    }

    private void OnUnpaused(object sender, System.EventArgs e)
    {
        HideCanvas();
    }

    private void OnPaused(object sender, System.EventArgs e)
    {
        ShowCanvas();
    }

    private void HideCanvas()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    private void ShowCanvas()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
}
