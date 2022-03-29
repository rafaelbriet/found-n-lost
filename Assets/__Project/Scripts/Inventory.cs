using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<Item> items = new List<Item>();

    public Item SelectedItem { get; private set; }
    public int SelectedItemIndex { get; private set; }
    public IReadOnlyCollection<Item> Items { get => items.AsReadOnly(); }

    public event EventHandler SelectedItemChanged;
    public event EventHandler<ItemAddedEventArgs> ItemAdded;

    private void Update()
    {
        foreach (Item item in items)
        {
            item.UpdateCooldownTimer(Time.deltaTime);
        }
    }

    public void AddItem(Item item)
    {
        items.Add(item);

        ItemAddedEventArgs args = new ItemAddedEventArgs();
        args.Item = item;

        ItemAdded?.Invoke(this, args);
    }

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

public class ItemAddedEventArgs : EventArgs
{
    public Item Item { get; set; }
}