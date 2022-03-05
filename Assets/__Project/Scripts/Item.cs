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
            return;
        }

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
