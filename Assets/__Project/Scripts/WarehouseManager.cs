using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class WarehouseManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("How long a night lasts in seconds.")]
    private float nightDuration = 30f;
    [SerializeField]
    [Tooltip("How much time will be added to the night duration as nights go by. Time in seconds")]
    private float nightDurationIncreasesBy = 10f;
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
    [SerializeField]
    private List<ItemScriptableObject> items;
    [SerializeField]
    private GameReport gameReport;

    private GameObject[] playerSpawnPoints;
    private GameObject[] ghostSpawnPoints;
    private Timer ghostSpawnTimer;
    private List<GameObject> ghosts = new List<GameObject>();
    private WarehouseState currentState;
    private int ghostsKilled;

    public Timer Timer { get; private set; }
    public int CurrentNight { get; private set; }

    public event EventHandler NightStarted;
    public event EventHandler NightEnded;
    public event EventHandler Paused;
    public event EventHandler Unpaused;

    private enum WarehouseState { Playing, Paused }

    private void Awake()
    {
        playerSpawnPoints = GameObject.FindGameObjectsWithTag("Player Spawn");
        ghostSpawnPoints = GameObject.FindGameObjectsWithTag("Ghost Spawn");

        ghostSpawnTimer = new Timer(maxTimeBetweenGhostSpawn);

        Timer = new Timer(nightDuration);

        player.GetComponent<Character>().Died += OnPlayerDied;
        player.SetActive(false);
    }

    private void Start()
    {
        GivePlayerStarterItems();
    }

    private void OnDestroy()
    {
        Restart();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;

        currentState = WarehouseState.Paused;

        Paused?.Invoke(this, EventArgs.Empty);
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1;

        currentState = WarehouseState.Playing;

        Unpaused?.Invoke(this, EventArgs.Empty);
    }

    public void Restart()
    {
        Time.timeScale = 1;
    }

    public bool IsGamePaused()
    {
        return currentState == WarehouseState.Paused;
    }

    private void GivePlayerStarterItems()
    {
        Inventory playerInventory = player.GetComponent<Inventory>();

        foreach (ItemScriptableObject item in items)
        {
            playerInventory.AddItem(new Item(item));
        }

        playerInventory.SelectItem(0);
    }

    private void ResetPlayerItemsCooldown()
    {
        Inventory playerInventory = player.GetComponent<Inventory>();

        foreach (Item item in playerInventory.Items)
        {
            item.ResetCooldown();
        }
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

        if (CurrentNight > 1)
        {
            Timer.SetDuration(CalculateNightDuration());
            ghostSpawnTimer.SetDuration(CalculateTimeBetweenGhostSpawn());
        }

        Timer.Reset();
        ghostSpawnTimer.Reset();

        player.SetActive(true);
        SetPlayerPosition();
        ResetPlayerItemsCooldown();

        StartCoroutine(GhostTimer());
        StartCoroutine(GameTimer());

        NightStarted?.Invoke(this, EventArgs.Empty);
    }

    private float CalculateNightDuration()
    {
        return nightDuration + (nightDurationIncreasesBy * (CurrentNight - 1));
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

        gameReport.TotalNightsWorked = CurrentNight;
        gameReport.GhostsKilled = ghostsKilled;

        SceneManager.LoadScene("GameOver");
    }

    private void SpawnGhost()
    {
        int maxGhostSpawn = CurrentNight < ghostSpawnPoints.Length ? CurrentNight : ghostSpawnPoints.Length;

        for (int i = 1; i <= maxGhostSpawn; i++)
        {
            GameObject ghostGO = Instantiate(ghostPrefab, GetRandomGhostSpawnPoint().transform.position, Quaternion.identity);
            ghosts.Add(ghostGO);

            Ghost ghost = ghostGO.GetComponent<Ghost>();
            ghost.Init(pathfinder);

            ghost.GetComponent<Character>().Died += OnGhostDied;

            if (CurrentNight > 1)
            {
                ghost.IncreaseSearchRadius(CurrentNight - 1);
                ghost.IncreaseAttackDamage(CurrentNight - 1);
            } 
        }
    }

    private void OnGhostDied(object sender, EventArgs e)
    {
        ghostsKilled++;
    }

    private void RemoveAllGhosts()
    {
        foreach (GameObject ghost in ghosts)
        {
            ghost.GetComponent<Character>().Died -= OnGhostDied;
            Destroy(ghost);
        }

        ghosts.Clear();
    }

    private void EndNight()
    {
        Debug.Log($"Another night of work is over. You're working for {CurrentNight} nights...");

        RemoveAllGhosts();

        gameReport.TotalNightsWorked = CurrentNight;
        gameReport.GhostsKilled = ghostsKilled;

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