using System.Collections.Generic;
using UnityEngine;
public class EnemySpawner
{
    private Transform transform;
    private EnemySO[] enemiesArray;
    private Transform target;
    private int waveDifficulty;

    public EnemySpawner(Transform transform, EnemySO[] enemySo, Transform target)
    {
        this.transform = transform;
        enemiesArray = enemySo;
        this.target = target;
        waveDifficulty = 10;

        SpawnWave();
    }

    public void SpawnWave()
    {
        while (waveDifficulty > 0) 
        {
            var enemy = GameManager.CreateObject(enemiesArray[Random.Range(0, enemiesArray.Length)].prefab, transform.position);
            ServiceLocator.Get<Dictionary<UnityEngine.Object, IEnemyBehavior>>().TryGetValue(enemy, out var enemyRef);
            waveDifficulty -= enemyRef.Difficulty; 
        }
    }
}

