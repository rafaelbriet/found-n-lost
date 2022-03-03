using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField]
    private Character playerCharacter;
    [SerializeField]
    private Slider hitPointsSlider;
    [SerializeField]
    private WarehouseManager warehouseManager;
    [SerializeField]
    private TextMeshProUGUI clockText;


    private void Awake()
    {
        playerCharacter.Damaged += OnPlayerCharacterDamaged;
        playerCharacter.Healed += OnPlayerCharacterHealed;
    }

    private void Start()
    {
        UpdateHitPointsDisplay();
    }

    private void Update()
    {
        UpdateClockDisplay();
    }

    private void UpdateClockDisplay()
    {
        clockText.text = warehouseManager.Timer.PrintTimeLeftInMinutes();
    }

    private void OnPlayerCharacterDamaged(object sender, System.EventArgs e)
    {
        UpdateHitPointsDisplay();
    }
    private void OnPlayerCharacterHealed(object sender, System.EventArgs e)
    {
        UpdateHitPointsDisplay();
    }

    private void UpdateHitPointsDisplay()
    {
        hitPointsSlider.value = (float)playerCharacter.CurrentHitPoints / (float)playerCharacter.MaxHitPoints;
    }
}
