using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    [SerializeField]
    private Image slotBackground;
    [SerializeField]
    private Image itemIcon;
    [SerializeField]
    private Slider slider;

    private Item item;
    
    public Image ItemIcon { get => itemIcon; set => itemIcon = value; }
    public Image SlotBackground { get => slotBackground; set => slotBackground = value; }

    public void Config(Item item)
    {
        this.item = item;

        this.item.ItemUsed += OnItemUsed;

        ItemIcon.sprite = this.item.ItemScriptableObject.icon;
    }

    private void OnItemUsed(object sender, System.EventArgs e)
    {
        StartCoroutine(ItemCooldownCoroutine());
    }

    private IEnumerator ItemCooldownCoroutine()
    {
        slider.value = 1;

        while (item.CanUse == false)
        {
            slider.value = item.CooldownTimer.TimeLeft / item.CooldownTimer.Duration;
            yield return null;
        }
        
        slider.value = 0;
    }
}
