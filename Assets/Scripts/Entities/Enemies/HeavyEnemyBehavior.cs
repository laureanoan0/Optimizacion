using Unity.VisualScripting;
using UnityEngine;
using System;

public class HeavyEnemyBehavior : IEnemyBehavior, IUpdateable, IFixedUpdateables
{
    private const int MAX_HITS = 5;

    private UnityEngine.Object entity;
    private Transform transform;
    private Transform target;
    private Vector3 originalPosition;
    private float speed;
    private EnemyTypes enemyType = EnemyTypes.tank;
    private int difficulty = 3;
    private int currentHits;

    public int Difficulty => difficulty;
    public EnemyTypes Type => enemyType;
    public UnityEngine.Object GameObjectRef => entity;

    public event Action<IEnemyBehavior> OnDeath;

    public void Init(UnityEngine.Object entity, Transform playerPos, EnemySO data)
    {
        this.entity = entity;
        transform = entity.GameObject().transform;
        originalPosition = transform.position;
        target = playerPos;
        speed = data.speed;
        currentHits = 0;
    }

    public void Activate(Vector3 position)
    {
        transform.position = position;
        entity.GameObject().SetActive(true);

        UpdateManager.Instance.Register((IUpdateable)this);
        UpdateManager.Instance.Register((IFixedUpdateables)this);
    }

    public void Deactivate()
    {
        UpdateManager.Instance.Unregister((IFixedUpdateables)this);
        UpdateManager.Instance.Unregister((IUpdateable)this);

        entity.GameObject().SetActive(false);
    }

    public void CustomFixedUpdate()
    {

    }

    public void TakeDamage()
    {
        currentHits++;

        if (currentHits >= MAX_HITS)
        {
            OnDeath?.Invoke(this);
        }
    }

    public void CustomUpdate(float time)
    {
        var (direction, kill) = EnemySteeringBehavior.Seek(transform, target);

        Vector3 direct = direction * time * speed;
        transform.position += direct;

        // Evita el error de Unity al pedirle rotacion a un vector cero
        if (direct != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direct);
        }
    }

    public void Reset()
    {
        transform.position = originalPosition;
        currentHits = 0; // importante: si no lo reseteamos, al volver del pool nace "medio muerto"
    }
}