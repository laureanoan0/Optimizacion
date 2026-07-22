using Unity.VisualScripting;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class BasicEnemyBehavior : IEnemyBehavior, IUpdateable
{
    private UnityEngine.Object entity;
    private Transform transform;
    private Transform target;
    private Vector3 originalPosition;
    private float speed;
    private EnemyTypes enemyType = EnemyTypes.melee;
    private int difficulty = 1;
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
    }

    public void Activate(Vector3 position)
    {
        transform.position = position;
        entity.GameObject().SetActive(true);

        UpdateManager.Instance.Register((IUpdateable)this);
    }

    public void Destroy()
    {
    }

    public void Deactivate()
    {
        UpdateManager.Instance.Unregister((IUpdateable)this);
        entity.GameObject().SetActive(false);
    }

    public void TakeDamage()
    {
        OnDeath?.Invoke(this);
    }

    public void CustomUpdate(float time)
    {
        var (direction, kill) = EnemySteeringBehavior.Seek(transform, target);

        Vector3 direct = direction * time * speed;
        transform.position += direct;
        if (direct.sqrMagnitude > 0.0001f)
        {
            transform.rotation = Quaternion.LookRotation(direct);
        }
        if (kill)
        {
            GameManager.LoadMainMenuScene();
        }
    }

    public void Reset()
    {
        transform.position = originalPosition;
    }
}