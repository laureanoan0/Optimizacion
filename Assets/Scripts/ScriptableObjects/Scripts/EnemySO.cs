using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "ScriptableObjects/EnemyStats", order = 1)]
public class EnemySO : ScriptableObject
{
    public GameObject prefab;
    public float speed;
}
