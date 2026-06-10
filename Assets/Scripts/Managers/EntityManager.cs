
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EntityManager
{
    public Dictionary<UnityEngine.Object, IEnemyBehavior> enemies;
    private (Object, PlayerBehavior) player;

    public EntityManager(EnemySO enemySo, PlayerSO playerSO)
    {
        enemies = new Dictionary<UnityEngine.Object, IEnemyBehavior>();
        SpawnPlayer(playerSO, playerSO.statsSO);
        //SpawnEnemySpawners(enemySo, player.Item1.GameObject().transform);
    }

    private void SpawnEnemySpawners(EnemySpawnersSO spawnerSo, Transform target)
    {
        UnityEngine.Object spawnerObj = GameManager.CreateObject(spawnerSo.prefab, spawnerSo.position);
    }

    private void SpawnPlayer(PlayerSO playerSo, PlayerStatsSO stats)
    {
        (Object, Object) playerObj = GameManager.CreatePlayer(playerSo.prefab, playerSo.empty);
        PlayerBehavior playerBehavior = new PlayerBehavior(playerObj.Item2.GameObject().transform, playerObj.Item1.GameObject().transform, playerObj.Item1.GameObject().GetComponent<Rigidbody>(), stats, stats.entityLayer);
        player.Item1 = playerObj.Item1;
        player.Item2 = playerBehavior;
    }
}

