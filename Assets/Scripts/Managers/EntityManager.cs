
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EntityManager
{
    public Dictionary<UnityEngine.Object, EnemyBehavior> enemies;
    private (Object, PlayerBehavior) player;

    public EntityManager(EnemySO enemySo, PlayerSO playerSO)
    {
        enemies = new Dictionary<UnityEngine.Object, EnemyBehavior>();
        SpawnPlayer(playerSO, playerSO.statsSO);
        SpawnEnemy(enemySo, player.Item1.GameObject().transform);
    }

    private void SpawnEnemy(EnemySO enemySo, Transform target)
    {
        UnityEngine.Object enemyObj = GameManager.CreateEnemy(enemySo.prefab);
        EnemyBehavior enemyBehavior = new EnemyBehavior(enemyObj, target, enemySo);
        enemies.Add(enemyObj, enemyBehavior);
    }

    private void SpawnPlayer(PlayerSO playerSo, PlayerStatsSO stats)
    {
        (Object, Object) playerObj = GameManager.CreatePlayer(playerSo.prefab, playerSo.empty);
        PlayerBehavior playerBehavior = new PlayerBehavior(playerObj.Item2.GameObject().transform, playerObj.Item1.GameObject().transform, playerObj.Item1.GameObject().GetComponent<Rigidbody>(), stats);
        player.Item1 = playerObj.Item1;
        player.Item2 = playerBehavior;
    }
}

