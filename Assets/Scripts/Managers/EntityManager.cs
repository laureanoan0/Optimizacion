using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public enum EnemyTypes { melee, fast, tank }
public class EntityManager
{
    private (Object, PlayerBehavior) player;

    public EntityManager(EnemySpawnersSO spawnersSo, PlayerSO playerSO, ParticleSystem enemyDeathParticles)
    {
        Boot(spawnersSo, playerSO, playerSO.statsSO, enemyDeathParticles);
    }

    private void Boot(EnemySpawnersSO spawnerSo, PlayerSO playerSo, PlayerStatsSO stats, ParticleSystem enemyDeathParticles)
    {
        ServiceLocator.Register(new Dictionary<UnityEngine.Object, IEnemyBehavior>());
        new ScoreManager();
        new EnemyDeathVFXManager(enemyDeathParticles, burstCount : 5);

        SpawnEnemySpawners(spawnerSo, SpawnPlayer(playerSo, stats));
    }
    private void SpawnEnemySpawners(EnemySpawnersSO spawnerSo, Transform target)
    {
        var spawnPoints = new List<EnemySpawner>();

        int index = 0;
        while (index <= 3)
        {
            UnityEngine.Object spawnerObj = GameplayController.CreateObject(spawnerSo.prefab, spawnerSo.position[index]);
            EnemySpawner spawnerBrain = new EnemySpawner(spawnerObj.GameObject().transform, spawnerSo.enemies, target);
            spawnPoints.Add(spawnerBrain);
            index++;
        }

        const float waveInterval = 3f;
        new WaveManager(spawnPoints, waveInterval, initialWaveSize: 4);
    }

    private Transform SpawnPlayer(PlayerSO playerSo, PlayerStatsSO stats)
    {
        (Rigidbody, Object, LineRenderer) playerObj = GameplayController.CreatePlayer(playerSo.playerRb, playerSo.empty, playerSo.lineRenderer);
        PlayerBehavior playerBehavior = new PlayerBehavior(playerObj.Item2.GameObject().transform, playerObj.Item1.GameObject().transform, playerObj.Item1, stats, stats.entityLayer, playerSo.lineRenderer);
        player.Item1 = playerObj.Item1;
        player.Item2 = playerBehavior;

        return playerObj.Item1.GameObject().transform;
    }
}