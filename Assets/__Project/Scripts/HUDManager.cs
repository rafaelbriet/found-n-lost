using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField]
    private Character playerCharacter;
    [SerializeField]
    private Slider hitPointsSlider;

    private void Awake()
    {
        playerCharacter.Damaged += OnPlayerCharacterDamaged;
        playerCharacter.Healed += OnPlayerCharacterHealed;
    }

    private void Start()
    {
        UpdateHitPointsDisplay();
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
