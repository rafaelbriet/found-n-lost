using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarehouseManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("How long a night lasts in seconds.")]
    private float nightDuration = 30f;

    public Timer Timer { get; private set; }

    private void Awake()
    {
        Timer = new Timer(nightDuration);
    }

    private void Start()
    {
        StartCoroutine(GameTimer());
    }

    private IEnumerator GameTimer()
    {
        Debug.Log("Another night of work has begun...");

        while (Timer.HasFinished == false)
        {
            Timer.Tick(Time.deltaTime);
            yield return null;
        }

        Debug.Log("Another night of work is over...");
    }
}