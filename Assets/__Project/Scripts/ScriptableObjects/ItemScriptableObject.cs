using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemScriptableObject : ScriptableObject
{
    public Sprite icon;

    public abstract void Use(ItemUseOptions options);
}