using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class EnemySpawner : IUpdateable
{
    private Transform transform;
    private EnemySO[] enemiesArray;
    private Transform target;
    private int waveDifficulty;
    private List<IEnemyBehavior> pooledEnemies= new List<IEnemyBehavior>();

    private Dictionary<UnityEngine.Object, IEnemyBehavior> enemies;
    public Dictionary<UnityEngine.Object, IEnemyBehavior> Enemies => enemies;

    float timer;
    float timerBase;


    public EnemySpawner(Transform transform, EnemySO[] enemySo, Transform target, float timer)
    {
        enemies = ServiceLocator.Get<Dictionary<UnityEngine.Object, IEnemyBehavior>>();
        this.timer = timer;
        timerBase = timer;

        this.transform = transform;
        this.target = target;
        enemiesArray = enemySo;
        waveDifficulty = 1;

        UpdateManager.Instance.Register(this);
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
            //Los enemigos se deben spawnear desactivados y agregar a una lista
            Debug.Log("gen");
            EnemySO enemyRand = enemiesArray[Random.Range(0, enemiesArray.Length)];
            var enemy = GameManager.CreateObject(enemyRand.prefab, transform.position);
            var enemyRef = EnemyFactory.CreateEnemy(enemyRand.type, enemy, target, enemyRand);
            enemies.Add(enemy, enemyRef);
            waveDifficulty -= enemyRef.Difficulty; 
        }
    }

    public void SendEnemy()
    {
        //Tomar un enemigo de la lista de desactivados, setear sus variables en valores originales y activarlo
        //Si no quedan enemigos disponibles generar otro
    }

    public void RecycleEnemy(UnityEngine.Object enemy)
    {
        //Devolver enemigo a la lista de desactivados
        enemies[enemy].Reset();
        pooledEnemies.Add(enemies[enemy]);
        enemy.GameObject().SetActive(false);
    }
}

