
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
        SpawnEnemySpawners(spawnerSo, SpawnPlayer(playerSo, stats));
    }
    private void SpawnEnemySpawners(EnemySpawnersSO spawnerSo, Transform target)
    {
        int index = 0;
        while (index <= 3)
        {
            UnityEngine.Object spawnerObj = GameManager.CreateObject(spawnerSo.prefab, spawnerSo.position[index]);
            EnemySpawner spawnerBrain = new EnemySpawner(spawnerObj.GameObject().transform, spawnerSo.enemies, target);
            index++;
        }
    }

    private Transform SpawnPlayer(PlayerSO playerSo, PlayerStatsSO stats)
    {
        (Object, Object) playerObj = GameManager.CreatePlayer(playerSo.prefab, playerSo.empty);
        PlayerBehavior playerBehavior = new PlayerBehavior(playerObj.Item2.GameObject().transform, playerObj.Item1.GameObject().transform, playerObj.Item1.GameObject().GetComponent<Rigidbody>(), stats, stats.entityLayer);
        player.Item1 = playerObj.Item1;
        player.Item2 = playerBehavior;

        return playerObj.Item1.GameObject().transform;
    }
}

