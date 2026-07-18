using UnityEngine;
using System;

public interface IEnemyBehavior
{
    public int Difficulty { get; }
    public EnemyTypes Type { get; }
    public UnityEngine.Object GameObjectRef { get; }

    public event Action<IEnemyBehavior> OnDeath;
    public void Init(UnityEngine.Object entity, Transform target, EnemySO data);
    public void Activate(Vector3 position);
    public void Deactivate();
    public void TakeDamage();
    public void Reset();
}