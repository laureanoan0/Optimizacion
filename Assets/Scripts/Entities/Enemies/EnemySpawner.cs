using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class EnemySpawner
{
    private Transform transform;
    private EnemySO[] enemiesArray;
    private Transform target;
    private int waveDifficulty;

    private Dictionary<UnityEngine.Object, IEnemyBehavior> enemies;
    public Dictionary<UnityEngine.Object, IEnemyBehavior> Enemies => enemies;

    public EnemySpawner(Transform transform, EnemySO[] enemySo, Transform target)
    {
        enemies = new Dictionary<UnityEngine.Object, IEnemyBehavior>();

        this.transform = transform;
        this.target = target;
        enemiesArray = enemySo;
        waveDifficulty = 10;

        ServiceLocator.Register(enemies);

        SpawnWave();
    }

    public void SpawnWave()
    {
        while (waveDifficulty > 0) 
        {
            EnemySO enemyRand = enemiesArray[Random.Range(0, enemiesArray.Length)];
            var enemy = GameManager.CreateObject(enemyRand.prefab, transform.position);
            var enemyRef = EnemyFactory.CreateEnemy(enemyRand.type, enemy, target);
            enemies.Add(enemy, enemyRef);
            waveDifficulty -= enemyRef.Difficulty; 
        }
    }
}

