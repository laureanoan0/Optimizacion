using UnityEngine;
using System.Collections.Generic;

public class EnemyFactory
{
    private static readonly HashSet<EnemyTypes> implementedTypes = new HashSet<EnemyTypes>
    {
        EnemyTypes.melee
    };

    public static bool IsImplemented(EnemyTypes type)
    {
        return implementedTypes.Contains(type);
    }

    public static IEnemyBehavior CreateEnemy(EnemyTypes type, UnityEngine.Object entity, Transform target, EnemySO data)
    {
        IEnemyBehavior enemy;

        switch (type)
        {
            case EnemyTypes.melee:
                enemy = new BasicEnemyBehavior();
                break;
            case EnemyTypes.ranged:
                enemy = null;
                break;
            default:
                enemy = null;
                break;
        }

        enemy?.Init(entity, target, data);
        return enemy;
    }
}