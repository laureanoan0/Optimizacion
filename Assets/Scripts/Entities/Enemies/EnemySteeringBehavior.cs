using JetBrains.Annotations;
using System.Runtime.CompilerServices;
using UnityEngine;
public class EnemySteeringBehavior
{
    public static Vector3 Seek(Transform user, Transform objective)
    {
        Vector3 dist = user.position - objective.position;
        return dist;
    }
}
