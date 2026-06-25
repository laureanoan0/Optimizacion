using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class EnemySpawner : IUpdateable
{
    private Transform transform;
    private EnemySO[] enemiesArray;
    private Transform target;
    private int waveDifficulty;

    private Dictionary<UnityEngine.Object, IEnemyBehavior> enemies;
    public Dictionary<UnityEngine.Object, IEnemyBehavior> Enemies => enemies;

    float timer;
    float timerBase;


    public EnemySpawner(Transform transform, EnemySO[] enemySo, Transform target, float timer)
    {
        enemies = new Dictionary<UnityEngine.Object, IEnemyBehavior>();
        this.timer = timer;
        timerBase = timer;

        this.transform = transform;
        this.target = target;
        enemiesArray = enemySo;
        waveDifficulty = 10;

        UpdateManager.Instance.Register(this);
        ServiceLocator.Register(enemies);
    }
    public void CustomUpdate(float time)
    {
        timer -= time;
        if (timer <= 0)
        {
            SpawnWave();
            timer = timerBase;
        }
    }

    public void SpawnWave()
    {
        if (waveDifficulty > 0) 
        {
            Debug.Log("gen");
            EnemySO enemyRand = enemiesArray[Random.Range(0, enemiesArray.Length)];
            var enemy = GameManager.CreateObject(enemyRand.prefab, transform.position);
            var enemyRef = EnemyFactory.CreateEnemy(enemyRand.type, enemy, target, enemyRand);
            enemies.Add(enemy, enemyRef);
            waveDifficulty -= enemyRef.Difficulty; 
        }
    }
}

