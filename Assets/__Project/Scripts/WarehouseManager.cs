using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarehouseManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("How long a night lasts in seconds.")]
    private float nightDuration = 30f;
    [SerializeField]
    private GameObject ghostPrefab;
    [SerializeField]
    [Tooltip("How long takes to a ghost to spawn in seconds.")]
    private float timeBetweenGhostSpawn = 10f;
    [SerializeField]
    private Pathfinder pathfinder;

    private GameObject[] ghostSpawnPoints;
    private Timer ghostSpawnTimer;

    public Timer Timer { get; private set; }

    private void Awake()
    {
        ghostSpawnPoints = GameObject.FindGameObjectsWithTag("Ghost Spawn");

        ghostSpawnTimer = new Timer(timeBetweenGhostSpawn);

        Timer = new Timer(nightDuration);
    }

    private void Start()
    {
        StartNight();
    }

    private void StartNight()
    {
        Debug.Log("Another night of work has begun...");
        
        SpawnGhost();

        StartCoroutine(GhostTimer());
        StartCoroutine(GameTimer());
    }

    private void SpawnGhost()
    {
        GameObject ghostGO = Instantiate(ghostPrefab, GetRandomGhostSpawnPoint().transform.position, Quaternion.identity);
        NPC ghostNPC = ghostGO.GetComponent<NPC>();
        ghostNPC.Init(pathfinder);
    }

    private GameObject GetRandomGhostSpawnPoint()
    {
        int index = Random.Range(0, ghostSpawnPoints.Length);
        return ghostSpawnPoints[index];
    }

    private IEnumerator GameTimer()
    {
        while (Timer.HasFinished == false)
        {
            Timer.Tick(Time.deltaTime);
            yield return null;
        }

        Debug.Log("Another night of work is over...");
    }

    private IEnumerator GhostTimer()
    {
        while (Timer.HasFinished == false)
        {
            while (ghostSpawnTimer.HasFinished == false)
            {
                ghostSpawnTimer.Tick(Time.deltaTime);
                yield return null;
            }

            SpawnGhost();
            ghostSpawnTimer.Reset();

            yield return null;
        }
    }
}