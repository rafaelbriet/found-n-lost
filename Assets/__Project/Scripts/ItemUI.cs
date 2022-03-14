using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ItemUI : MonoBehaviour
{
    [SerializeField]
    private Image slotBackground;
    [SerializeField]
    private Image itemIcon;
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private Sprite selectedSprite;
    [SerializeField]
    private Sprite normalSprite;

    private Item item;
    private Image image;
    
    public Image ItemIcon { get => itemIcon; set => itemIcon = value; }
    public Image SlotBackground { get => slotBackground; set => slotBackground = value; }

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void Config(Item item)
    {
        this.item = item;

        this.item.ItemUsed += OnItemUsed;

        ItemIcon.sprite = this.item.ItemScriptableObject.icon;
    }

    public void Select()
    {
        image.sprite = selectedSprite;
    }

    public void Unselect()
    {
        image.sprite = normalSprite;
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
