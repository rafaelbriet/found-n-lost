using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    private int maxHitPoints = 100;

    public int CurrentHitPoints { get; private set; }

    private void Awake()
    {
        CurrentHitPoints = maxHitPoints;
    }

    public void Damage(int amout)
    {
        CurrentHitPoints -= amout;
    }
}
