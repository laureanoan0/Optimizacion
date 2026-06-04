using UnityEngine;

public class EnemyMeleBehavior : IEntityBehavior, IUpdateable, IFixedUpdateables
{
    private Transform transform;
    private Transform target;
    private float speed = 10f;
    public EnemyMeleBehavior(Entity entity, Transform playerPos)
    {
        transform = entity.transform;
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

    public void CustomUpdate(float time)
    {
        transform.position += EnemySteeringBehavior.Seek(transform, target) * time * speed;
    }

    public void Attack()
    {

    }

    public void OnDeath()
    {
        Destroy();
    }
}
