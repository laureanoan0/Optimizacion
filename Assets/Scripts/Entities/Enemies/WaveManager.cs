using System.Collections.Generic;
using UnityEngine;

public class WaveManager : IUpdateable
{
    private readonly List<EnemySpawner> spawnPoints;
    private readonly float waveInterval;

    private int waveSize;
    private float timer;

    public WaveManager(List<EnemySpawner> spawnPoints, float waveInterval, int initialWaveSize)
    {
        this.spawnPoints = spawnPoints;
        this.waveInterval = waveInterval;
        waveSize = initialWaveSize;

        timer = waveInterval;
        UpdateManager.Instance.Register(this);

        SpawnWave();
    }

    public void CustomUpdate(float time)
    {
        timer -= time;
        if (timer <= 0)
        {
            SpawnWave();
            timer = waveInterval;
        }
    }

    private void SpawnWave()
    {
<<<<<<< Updated upstream
        var validSpawnPoints = spawnPoints.FindAll(s => s.HasEnemyTypes);
        if (validSpawnPoints.Count == 0) return; 

        for (int i = 0; i < waveSize; i++)
=======
        currentValidSpawnPoints = spawnPoints.FindAll(s => s.HasEnemyTypes && !s.EnemiesAlive);
        if (currentValidSpawnPoints.Count != 4)
            return;

        if (waveNumber >= 3)
        {
            GameplayController.PlayerWon = true;
            GameplayController.LoadFinalScene();
            return;
        }

        OnWaveFinished?.Invoke(waveNumber++);
        enemiesSpawnedThisWave = 0;
        spawnTimer = 0;
        isSpawning = true;
    }
    private void HandleSpawning(float time)
    {
        spawnTimer -= time;
        if (spawnTimer <= 0)
>>>>>>> Stashed changes
        {
            EnemySpawner spawner = validSpawnPoints[Random.Range(0, validSpawnPoints.Count)];
            spawner.SpawnOne();
        }

        waveSize++;
    }
}