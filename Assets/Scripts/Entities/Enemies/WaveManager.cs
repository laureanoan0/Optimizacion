using System.Collections.Generic;
using UnityEngine;
using System;


public class WaveManager : IUpdateable
{
    private readonly List<EnemySpawner> spawnPoints;
    private readonly float waveInterval;
    private readonly float spawnDelay = 0.5f;

    private readonly float maxWaves;

    private float waveSize;
    private int waveNumber;
    private float timer;
    private bool isSpawning;
    private int enemiesSpawnedThisWave;
    private float spawnTimer;
    private readonly List<EnemySpawner> currentValidSpawnPoints = new List<EnemySpawner>();


    public event Action<int> OnWaveFinished;
    public event Action OnGameWon;

    public WaveManager(List<EnemySpawner> spawnPoints, float waveInterval, float initialWaveSize, int maxWaves = 2)
    {
        this.spawnPoints = spawnPoints;
        this.waveInterval = waveInterval;
        this.maxWaves = maxWaves;

        waveSize = initialWaveSize;

        timer = waveInterval;
        UpdateManager.Instance.Register(this);
        ServiceLocator.Register(this);

        waveNumber = 1;

        StartWave();
    }

    public void CustomUpdate(float time)
    {
        {
            if (isSpawning)
            {
                HandleSpawning(time);
                return;
            }

            timer -= time;
            if (timer <= 0)
            {
                StartWave();
                timer = waveInterval;
            }
        }
    }

    private void StartWave()
    {
        RefreshValidSpawnPoints();

        if (currentValidSpawnPoints.Count != 4) return;

        if (waveNumber > maxWaves)
        {
            OnGameWon?.Invoke();
            return;
        }

        OnWaveFinished?.Invoke(waveNumber++);
        enemiesSpawnedThisWave = 0;
        spawnTimer = 0;
        isSpawning = true;
    }
    private void RefreshValidSpawnPoints()
    {
        currentValidSpawnPoints.Clear();

        for (int i = 0; i < spawnPoints.Count; i++)
        {
            EnemySpawner spawner = spawnPoints[i];
            if (spawner.HasEnemyTypes && !spawner.EnemiesAlive)
            {
                currentValidSpawnPoints.Add(spawner);
            }
        }
    }

    private void HandleSpawning(float time)
    {
        spawnTimer -= time;
        if (spawnTimer <= 0)
        {
            EnemySpawner spawner = currentValidSpawnPoints[UnityEngine.Random.Range(0, currentValidSpawnPoints.Count)];
            spawner.SpawnOne();
            enemiesSpawnedThisWave++;
            spawnTimer = spawnDelay;

            if (enemiesSpawnedThisWave >= waveSize)
            {
                isSpawning = false;
                waveSize = Mathf.Floor(waveSize * 1.5f);
            }
        }
    }
}