using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    public Sprite icon;

    public abstract void Use(ItemUseOptions options);
}