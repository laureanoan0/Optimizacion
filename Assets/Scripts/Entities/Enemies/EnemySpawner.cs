using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.VFX;

public class EnemySpawner //Controla las pools
{
    private int prewarmCount = 50;

    private Transform transform;
    private EnemySO[] enemiesArray;
    private Transform target;

    private Dictionary<EnemySO, ObjectPool<IEnemyBehavior>> pools = new Dictionary<EnemySO, ObjectPool<IEnemyBehavior>>();
    private Dictionary<UnityEngine.Object, IEnemyBehavior> enemies;
    private List<IEnemyBehavior> activeEnemies = new List<IEnemyBehavior>();

    public bool EnemiesAlive => activeEnemies.Count > 0;
    public bool HasEnemyTypes => enemiesArray.Length > 0;

    private ScoreManager scoreManager;
    private EnemyDeathVFXManager vfxManager;

    public EnemySpawner(Transform transform, EnemySO[] enemySo, Transform target)
    {
        enemies = ServiceLocator.Get<Dictionary<UnityEngine.Object, IEnemyBehavior>>();
        scoreManager = ServiceLocator.Get<ScoreManager>();
        vfxManager = ServiceLocator.Get<EnemyDeathVFXManager>();  

        this.transform = transform;
        this.target = target;

        var implemented = new List<EnemySO>();
        foreach (var so in enemySo)
        {
            if (EnemyFactory.IsImplemented(so.type))
            {
                implemented.Add(so);
            }
        }
        enemiesArray = implemented.ToArray();

        foreach (var enemySOItem in enemiesArray)
        {
            if (!pools.ContainsKey(enemySOItem))
            {
                pools.Add(enemySOItem, CreatePool(enemySOItem, prewarmCount: prewarmCount));
            }
        }
    }
    public void SpawnOne()
    {
        if (enemiesArray.Length == 0) return;

        EnemySO enemyRand = enemiesArray[Random.Range(0, enemiesArray.Length)];
        pools[enemyRand].Get();
    }

    #region Pools
    private ObjectPool<IEnemyBehavior> CreatePool(EnemySO data, int prewarmCount)
    {
        ObjectPool<IEnemyBehavior> pool = null;

        pool = new ObjectPool<IEnemyBehavior>(
            createFunc: () => CreateEnemyInstance(data, pool),
            actionOnGet: GetEnemy,
            actionOnRelease: ReleaseEnemy,
            actionOnDestroy: DestroyEnemy,
            collectionCheck: false,
            defaultCapacity: prewarmCount,
            maxSize: prewarmCount * 2);

        var warmBatch = new List<IEnemyBehavior>(prewarmCount);
        for (int i = 0; i < prewarmCount; i++)
        {
            warmBatch.Add(pool.Get());
        }
        foreach (var enemy in warmBatch)
        {
            pool.Release(enemy);
        }

        return pool;
    }
    private void GetEnemy(IEnemyBehavior enemy)
    {
        activeEnemies.Add(enemy);
        enemy.Activate(transform.position);
    }
    private void ReleaseEnemy(IEnemyBehavior enemy)
    {
        activeEnemies.Remove(enemy);
        enemy.Deactivate();
    }
    private void DestroyEnemy(IEnemyBehavior enemy)
    {
        enemy.Destroy();
        UnityEngine.Object.Destroy(enemy.GameObjectRef.GameObject());
    }
    private IEnemyBehavior CreateEnemyInstance(EnemySO data, ObjectPool<IEnemyBehavior> pool)
    {

        UnityEngine.Object entity = GameManager.CreateObject(data.prefab, transform.position);
        IEnemyBehavior enemy = EnemyFactory.CreateEnemy(data.type, entity, target, data);

        enemy.OnDeath += behavior =>
        {
            Vector3 deathPosition = behavior.GameObjectRef.GameObject().transform.position;

            scoreManager.AddScore(behavior.Difficulty);
            vfxManager.PlayAt(deathPosition);

            behavior.Reset();
            pool.Release(behavior);
        };

        enemies[entity] = enemy;
        return enemy;
    }
    #endregion
}