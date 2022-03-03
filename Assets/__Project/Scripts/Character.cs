using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    private int maxHitPoints = 100;

    private Coroutine sprayCoroutine;

    public int MaxHitPoints => maxHitPoints;
    public int CurrentHitPoints { get; private set; }
    public bool HasSprayApplied { get; private set; }

    public event EventHandler Damaged;
    public event EventHandler Healed;
    public event EventHandler Died;

    private void Awake()
    {
        CurrentHitPoints = maxHitPoints;
    }

    public void Damage(int amout)
    {
        CurrentHitPoints -= amout;

        CurrentHitPoints = Mathf.Clamp(CurrentHitPoints, 0, maxHitPoints);

        Damaged?.Invoke(this, EventArgs.Empty);

        if (CurrentHitPoints <= 0)
        {
            Died?.Invoke(this, EventArgs.Empty);
        }
    }

    public void Heal(int amount)
    {
        CurrentHitPoints += amount;

        CurrentHitPoints = Mathf.Clamp(CurrentHitPoints, 0, maxHitPoints);

        Healed?.Invoke(this, EventArgs.Empty);
    }

    public void ApplySpray(float duration)
    {
        if (sprayCoroutine != null)
        {
            StopCoroutine(sprayCoroutine);
        }

        sprayCoroutine = StartCoroutine(ApplySprayCoroutine(duration));
    }

    private IEnumerator ApplySprayCoroutine(float duration)
    {
        HasSprayApplied = true;

        while (duration > 0)
        {
            duration -= Time.deltaTime;
            yield return null;
        }

        HasSprayApplied = false;
    }
}
