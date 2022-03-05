using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemScriptableObject : ScriptableObject
{
    public Sprite icon;
    public float cooldown;

    public abstract void Use(ItemUseOptions options);
}