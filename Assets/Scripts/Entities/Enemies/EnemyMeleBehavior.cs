using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyMeleBehavior : IEntityBehavior, IUpdateable, IFixedUpdateables
{
    private Transform transform;
    private Transform target;
    public EnemyMeleBehavior(Entity entity, Transform playerPos)
    {
        transform = entity.transform;
        target = playerPos;
        UpdateManager.Instance.Register((IUpdateable)this);
        UpdateManager.Instance.Register((IFixedUpdateables)this);
    }
    public void CustomFixedUpdate()
    {

    }

    public void CustomUpdate(float time)
    {
        EnemySteeringBehavior.Seek(transform, target);
        transform.position -= target.position;
    }
    public void Destroy()
    {
        UpdateManager.Instance.Unregister((IFixedUpdateables)this);
        UpdateManager.Instance.Unregister((IUpdateable)this);
    }

    public void Attack()
    {

    }

    public void OnDeath()
    {
        Destroy();
    }
}
