using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public bool CanUse { get; private set; } = true;
    public ItemScriptableObject ItemScriptableObject { get; private set; }
    public Timer CooldownTimer { get; private set; }

    public Item(ItemScriptableObject itemScriptableObject)
    {
        ItemScriptableObject = itemScriptableObject;
        CooldownTimer = new Timer(ItemScriptableObject.cooldown);
    }

    public event EventHandler ItemUsed;

    public void Use(ItemUseOptions options)
    {
        if (CanUse == false)
        {
            Debug.Log("Item cannot be used now");
            return;
        }

        Debug.Log("Item can be used now");
        CanUse = false;
        ItemScriptableObject.Use(options);
        CooldownTimer.Reset();
        ItemUsed?.Invoke(this, EventArgs.Empty);
    }

    public void UpdateCooldownTimer(float elapsed)
    {
        CooldownTimer.Tick(elapsed);

        if (CooldownTimer.HasFinished)
        {
            CanUse = true;
            CooldownTimer.Reset();
        }
    }

    public void ResetCooldown()
    {
        CanUse = true;
        CooldownTimer.Reset();
    }
}
