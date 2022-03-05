using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private List<ItemScriptableObject> items;

    public ItemScriptableObject SelectedItem { get; private set; }
    public int SelectedItemIndex { get; private set; }
    public IReadOnlyCollection<ItemScriptableObject> Items { get => items.AsReadOnly(); }

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
