using System;
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
    private Inventory playerInventory;
    [SerializeField]
    private Image hitPointsFill;
    [SerializeField]
    private WarehouseManager warehouseManager;
    [SerializeField]
    private TextMeshProUGUI clockText;
    [SerializeField]
    private GameObject itemSlotPrefab;
    [SerializeField]
    private GameObject invetoryBar;

    private void Awake()
    {
        playerCharacter.Damaged += OnPlayerCharacterDamaged;
        playerCharacter.Healed += OnPlayerCharacterHealed;
        playerInventory.SelectedItemChanged += OnSelectedItemChanged;
        playerInventory.ItemAdded += OnItemAdded;
    }

    private void Start()
    {
        hitPointsFill.fillAmount = 1f;
    }

    private void Update()
    {
        UpdateClockDisplay();
    }

    private void OnItemAdded(object sender, ItemAddedEventArgs args)
    {
        ItemUI itemUI = Instantiate(itemSlotPrefab, invetoryBar.transform).GetComponent<ItemUI>();
        itemUI.Config(args.Item);
    }

    private void OnSelectedItemChanged(object sender, System.EventArgs e)
    {
        UpdateInventoryBar();
    }

    private void UpdateInventoryBar()
    {
        foreach (Transform item in invetoryBar.transform)
        {
            ItemUI itemUI = item.GetComponent<ItemUI>();
            itemUI.Unselect();
        }
        
        invetoryBar.transform.GetChild(playerInventory.SelectedItemIndex).GetComponent<ItemUI>().Select();
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
        hitPointsFill.fillAmount = (float)playerCharacter.CurrentHitPoints / playerCharacter.MaxHitPoints;
    }
}
