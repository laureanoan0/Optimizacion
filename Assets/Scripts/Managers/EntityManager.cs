
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EnemyTypes { melee, ranged }
public class EntityManager
{
    private (Object, PlayerBehavior) player;

    public EntityManager(EnemySpawnersSO spawnersSo, PlayerSO playerSO)
    {
        Boot(spawnersSo, playerSO, playerSO.statsSO);
    }

    private void Boot(EnemySpawnersSO spawnerSo, PlayerSO playerSo, PlayerStatsSO stats)
    {
        ServiceLocator.Register(new Dictionary<UnityEngine.Object, IEnemyBehavior>());
        SpawnEnemySpawners(spawnerSo, SpawnPlayer(playerSo, stats));
    }
    private void SpawnEnemySpawners(EnemySpawnersSO spawnerSo, Transform target)
    {
        int index = 0;
        while (index <= 3)
        {
            UnityEngine.Object spawnerObj = GameManager.CreateObject(spawnerSo.prefab, spawnerSo.position[index]);
            EnemySpawner spawnerBrain = new EnemySpawner(spawnerObj.GameObject().transform, spawnerSo.enemies, target, spawnerSo.timer);
            index++;
        }
    }

    private Transform SpawnPlayer(PlayerSO playerSo, PlayerStatsSO stats)
    {
        (Rigidbody, Object) playerObj = GameManager.CreatePlayer(playerSo.playerRb, playerSo.empty);
        PlayerBehavior playerBehavior = new PlayerBehavior(playerObj.Item2.GameObject().transform, playerObj.Item1.GameObject().transform, playerObj.Item1, stats, stats.entityLayer);
        player.Item1 = playerObj.Item1;
        player.Item2 = playerBehavior;

        return playerObj.Item1.GameObject().transform;
    }
}

