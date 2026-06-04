using UnityEngine;
public class EnemySteeringBehavior
{
    public static Vector3 Seek(Transform user, Transform objective)
    {
        Vector3 direction = (user.position - objective.position).normalized;
        return direction;
    }
}
