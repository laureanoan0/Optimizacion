using Unity.VisualScripting;
using UnityEngine;

public class BasicEnemyBehavior : IEnemyBehavior, IUpdateable, IFixedUpdateables 
{
    private Transform transform;
    private Transform target;
    private float speed = 10f;
    private EnemyTypes enemyType = EnemyTypes.melee;
    private int difficulty = 1;

    public int Difficulty => difficulty;
    public EnemyTypes Type => enemyType;

    public BasicEnemyBehavior(Object entity, Transform playerPos, EnemySO data)
    {
        transform = entity.GameObject().transform;
        target = playerPos;
        speed = data.speed;

        UpdateManager.Instance.Register((IUpdateable)this);
        UpdateManager.Instance.Register((IFixedUpdateables)this);
    }
    public void Destroy()
    {
        UpdateManager.Instance.Unregister((IFixedUpdateables)this);
        UpdateManager.Instance.Unregister((IUpdateable)this);
    }
    public void CustomFixedUpdate()
    {

    }

    public void TakeDamage()
    {
        Debug.Log("Holaaaaaaaaa");
    }
    public void CustomUpdate(float time)
    {
        Vector3 direction = EnemySteeringBehavior.Seek(transform, target).Item1;
        bool kill = EnemySteeringBehavior.Seek(transform, target).Item2;

        Vector3 direct = direction * time * speed;
        if (kill) GameManager.LoadGameplayScene();

        transform.position += direct;
        transform.rotation = Quaternion.LookRotation(direct);
    }

    public void OnDeath()
    {
        Destroy();
    }
}
