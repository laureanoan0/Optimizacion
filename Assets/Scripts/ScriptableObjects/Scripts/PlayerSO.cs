using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "ScriptableObjects/Player", order = 1)]
public class PlayerSO : ScriptableObject
{
    public GameObject prefab;
    public GameObject empty;
    public PlayerStatsSO statsSO;
}

