using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner
{
    private const int PREWARM_COUNT = 5;
    private Transform transform;
    private EnemySO[] enemiesArray;
    private Transform target;
    private Dictionary<EnemySO, ObjectPool<IEnemyBehavior>> pools = new Dictionary<EnemySO, ObjectPool<IEnemyBehavior>>();
    private Dictionary<UnityEngine.Object, IEnemyBehavior> enemies;
    public Dictionary<UnityEngine.Object, IEnemyBehavior> Enemies => enemies;
    public bool HasEnemyTypes => enemiesArray.Length > 0;

    public EnemySpawner(Transform transform, EnemySO[] enemySo, Transform target)
    {
        enemies = ServiceLocator.Get<Dictionary<UnityEngine.Object, IEnemyBehavior>>();

        this.transform = transform;
        this.target = target;

        var implemented = new List<EnemySO>();
        foreach (var so in enemySo)
        {
            if (EnemyFactory.IsImplemented(so.type))
            {
                implemented.Add(so);
            }
            else
            {
                Debug.LogWarning($"EnemySpawner: el tipo '{so.type}' ({so.name}) todavía no tiene comportamiento implementado, se omite del spawn.");
            }
        }
        enemiesArray = implemented.ToArray();

        foreach (var enemySOItem in enemiesArray)
        {
            if (!pools.ContainsKey(enemySOItem))
            {
                pools.Add(enemySOItem, CreatePool(enemySOItem, prewarmCount: PREWARM_COUNT));
            }
        }
    }
    public void SpawnOne()
    {
        if (enemiesArray.Length == 0) return;

        EnemySO enemyRand = enemiesArray[Random.Range(0, enemiesArray.Length)];
        pools[enemyRand].Get();
    }

    private ObjectPool<IEnemyBehavior> CreatePool(EnemySO data, int prewarmCount)
    {
        ObjectPool<IEnemyBehavior> pool = null;

        pool = new ObjectPool<IEnemyBehavior>(
            createFunc: () => CreateEnemyInstance(data, pool),
            actionOnGet: enemy => enemy.Activate(transform.position),
            actionOnRelease: enemy => enemy.Deactivate(),
            actionOnDestroy: enemy => UnityEngine.Object.Destroy(enemy.GameObjectRef.GameObject()),
            collectionCheck: false,
            defaultCapacity: prewarmCount,
            maxSize: 30);

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
    private IEnemyBehavior CreateEnemyInstance(EnemySO data, ObjectPool<IEnemyBehavior> pool)
    {
        UnityEngine.Object entity = GameManager.CreateObject(data.prefab, transform.position);
        IEnemyBehavior enemy = EnemyFactory.CreateEnemy(data.type, entity, target, data);

        enemy.OnDeath += behavior =>
        {
            behavior.Reset();
            pool.Release(behavior);
        };

        enemies[entity] = enemy;
        return enemy;
    }
}