using UnityEngine;
using System;

public interface IEnemyBehavior
{
    public int Difficulty { get; }
    public EnemyTypes Type { get; }
    public void TakeDamage();
    public void Reset();
}

