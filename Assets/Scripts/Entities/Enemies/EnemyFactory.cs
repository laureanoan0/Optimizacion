
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory
{

    public static IEnemyBehavior CreateEnemy(EnemyTypes type, UnityEngine.Object entity, Transform target)
    {
        switch (type) 
        {
            case EnemyTypes.melee: return new BasicEnemyBehavior(entity, target);
            case EnemyTypes.ranged: return default;
            default: return null;
                
        }
    }
}

