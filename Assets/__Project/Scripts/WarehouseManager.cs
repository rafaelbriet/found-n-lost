using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WarehouseManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("How long a night lasts in seconds.")]
    private float nightDuration = 30f;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject ghostPrefab;
    [SerializeField]
    [Tooltip("The maximum time a ghost will take to spawn. Time in seconds.")]
    private float maxTimeBetweenGhostSpawn = 10f;
    [SerializeField]
    [Tooltip("The minimum time a ghost will take to spawn. Time in seconds.")]
    private float minTimeBetweenGhostSpawn = 5f;
    [SerializeField]
    [Tooltip("How much the time between ghost spawn decreases as nights pass. Time in seconds.")]
    private float timeBetweenGhostSpawnDecreasesBy = 1f;
    [SerializeField]
    private Pathfinder pathfinder;

    private GameObject[] playerSpawnPoints;
    private GameObject[] ghostSpawnPoints;
    private Timer ghostSpawnTimer;
    private List<GameObject> ghosts = new List<GameObject>();

    public Timer Timer { get; private set; }
    public int CurrentNight { get; private set; }

    public event EventHandler NightStarted;
    public event EventHandler NightEnded;

    private void Awake()
    {
        playerSpawnPoints = GameObject.FindGameObjectsWithTag("Player Spawn");
        ghostSpawnPoints = GameObject.FindGameObjectsWithTag("Ghost Spawn");

        ghostSpawnTimer = new Timer(maxTimeBetweenGhostSpawn);

        Timer = new Timer(nightDuration);

        player.GetComponent<Character>().Died += OnPlayerDied;
        player.SetActive(false);
    }

    private void OnPlayerDied(object sender, System.EventArgs e)
    {
        GameOver();
    }

    public void StartNight()
    {
        Debug.Log("Another night of work has begun...");

        CurrentNight++;

        SpawnGhost();

        Timer.Reset();

        if (CurrentNight > 1)
        {
            ghostSpawnTimer.SetDuration(CalculateTimeBetweenGhostSpawn());
        }

        ghostSpawnTimer.Reset();

        player.SetActive(true);
        SetPlayerPosition();

        StartCoroutine(GhostTimer());
        StartCoroutine(GameTimer());

        NightStarted?.Invoke(this, EventArgs.Empty);
    }

    private float CalculateTimeBetweenGhostSpawn()
    {
        float time = maxTimeBetweenGhostSpawn - (timeBetweenGhostSpawnDecreasesBy * CurrentNight);

        return Mathf.Clamp(time, minTimeBetweenGhostSpawn, maxTimeBetweenGhostSpawn);
    }

    private void GameOver()
    {
        Debug.Log($"After fight so many ghosts for {CurrentNight} nights, you decided to quit this job...");

        Destroy(player);
    }

    private void SpawnGhost()
    {
        GameObject ghostGO = Instantiate(ghostPrefab, GetRandomGhostSpawnPoint().transform.position, Quaternion.identity);
        ghosts.Add(ghostGO);

        NPC ghostNPC = ghostGO.GetComponent<NPC>();
        ghostNPC.Init(pathfinder);
    }

    private void RemoveAllGhosts()
    {
        foreach (GameObject ghost in ghosts)
        {
            Destroy(ghost);
        }

        ghosts.Clear();
    }

    private void EndNight()
    {
        Debug.Log($"Another night of work is over. You're working for {CurrentNight} nights...");

        RemoveAllGhosts();

        NightEnded?.Invoke(this, EventArgs.Empty);

        player.SetActive(false);
    }

    private void SetPlayerPosition()
    {
        player.transform.position = GetRandomPlayerSpawnPoint().transform.position;
    }

    private GameObject GetRandomPlayerSpawnPoint()
    {
        int index = Random.Range(0, playerSpawnPoints.Length);
        return playerSpawnPoints[index];
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

        EndNight();
    }

    private IEnumerator GhostTimer()
    {
        while (Timer.HasFinished == false)
        {
            while (ghostSpawnTimer.HasFinished == false)
            {
                if (Timer.HasFinished)
                {
                    break;
                }

                ghostSpawnTimer.Tick(Time.deltaTime);
                yield return null;
            }

            SpawnGhost();
            ghostSpawnTimer.Reset();

            yield return null;
        }
    }
}