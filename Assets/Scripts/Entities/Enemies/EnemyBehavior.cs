using Unity.VisualScripting;
using UnityEngine;

public class EnemyBehavior : IUpdateable, IFixedUpdateables, IEnemyBehavior
{
    private Transform transform;
    private Transform target;
    private float speed = 10f;

    private int difficulty = 1;

    public int Difficulty => difficulty;

    public EnemyBehavior(Object entity, Transform playerPos, EnemySO scriptableObject)
    {
        transform = entity.GameObject().transform;
        target = playerPos;
        speed = scriptableObject.speed;

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
