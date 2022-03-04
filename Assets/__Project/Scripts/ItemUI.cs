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
    
    public Image ItemIcon { get => itemIcon; set => itemIcon = value; }
    public Image SlotBackground { get => slotBackground; set => slotBackground = value; }
}
