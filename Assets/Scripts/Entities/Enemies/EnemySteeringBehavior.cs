using UnityEngine;
public class EnemySteeringBehavior
{
    public static (Vector3, bool) Seek (Transform user, Transform objective)
    {
        Vector3 toTarget = objective.transform.position - user.transform.position;
        float distance = toTarget.magnitude;

        Vector3 futurePosition = objective.transform.position;

        Vector3 desiredDir = (futurePosition - user.transform.position).normalized;
        return (desiredDir, distance < 1f);
    }
}
