using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private List<Item> items;

    public Item SelectedItem { get; private set; }

    public void SelectItem(int index)
    {
        if (index < 0 || index >= items.Count)
        {
            return;
        }

        SelectedItem = items[index];
    }
}
