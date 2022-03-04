using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private List<Item> items;

    public Item SelectedItem { get; private set; }
    public int SelectedItemIndex { get; private set; }
    public IReadOnlyCollection<Item> Items { get => items.AsReadOnly(); }

    public event EventHandler SelectedItemChanged;

    public void SelectItem(int index)
    {
        if (index < 0 || index >= items.Count)
        {
            return;
        }

        SelectedItem = items[index];
        SelectedItemIndex = index;

        SelectedItemChanged?.Invoke(this, EventArgs.Empty);
    }
}
