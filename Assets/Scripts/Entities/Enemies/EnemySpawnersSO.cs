using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawners", menuName = "ScriptableObjects/EnemySpawners", order = 1)]
public class EnemySpawnersSO : ScriptableObject
{
    public GameObject prefab;
    public Vector3 position;
}

