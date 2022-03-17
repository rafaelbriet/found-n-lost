using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    private ItemUseOptions options;

    public bool CanUse { get; private set; } = true;
    public ItemScriptableObject ItemScriptableObject { get; private set; }
    public Timer CooldownTimer { get; private set; }
    public Timer DurationTimer { get; private set; }

    public Item(ItemScriptableObject itemScriptableObject)
    {
        ItemScriptableObject = itemScriptableObject;
        CooldownTimer = new Timer(ItemScriptableObject.cooldown);
        DurationTimer = new Timer(ItemScriptableObject.duration);
    }

    public event EventHandler ItemUsed;

    public void Use(ItemUseOptions options)
    {
        if (CanUse == false)
        {
            return;
        }

        this.options = options;

        CanUse = false;
        ItemScriptableObject.Use(options);
        CooldownTimer.Reset();
        DurationTimer.Reset();
        ItemUsed?.Invoke(this, EventArgs.Empty);
    }

    public void UpdateCooldownTimer(float elapsed)
    {
        CooldownTimer.Tick(elapsed);
        DurationTimer.Tick(elapsed);

        if (CooldownTimer.HasFinished)
        {
            ResetCooldown();
        }

        if (DurationTimer.HasFinished)
        {
            ItemScriptableObject.ResetAnimation(options);
        }
    }

    public void ResetCooldown()
    {
        ItemScriptableObject.ResetAnimation(options);

        CanUse = true;
        CooldownTimer.Reset();
        DurationTimer.Reset();
    }
}
