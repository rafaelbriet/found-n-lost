using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    private int maxHitPoints = 100;

    private Coroutine sprayCoroutine;

    public int CurrentHitPoints { get; private set; }
    public bool HasSprayApplied { get; private set; }

    private void Awake()
    {
        CurrentHitPoints = maxHitPoints;
    }

    public void Damage(int amout)
    {
        CurrentHitPoints -= amout;
    }

    public void Heal(int amount)
    {
        CurrentHitPoints += amount;

        Mathf.Clamp(CurrentHitPoints, 0, maxHitPoints);
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
