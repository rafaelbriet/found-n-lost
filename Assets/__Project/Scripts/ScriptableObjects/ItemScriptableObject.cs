using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemScriptableObject : ScriptableObject
{
    public Sprite icon;
    public float cooldown;
    public float duration;

    public abstract void Use(ItemUseOptions options);
    public virtual void ResetAnimation(ItemUseOptions options) { }
}