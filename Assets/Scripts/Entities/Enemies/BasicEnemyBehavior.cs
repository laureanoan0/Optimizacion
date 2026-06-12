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

    public BasicEnemyBehavior(Object entity, Transform playerPos)
    {
        transform = entity.GameObject().transform;
        target = playerPos;

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
        transform.position += EnemySteeringBehavior.Seek(transform, target) * time * speed;
    }

    public void OnDeath()
    {
        Destroy();
    }
}
