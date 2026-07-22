using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "ScriptableObjects/Player", order = 1)]
public class PlayerSO : ScriptableObject
{
    public GameObject empty;
    public PlayerStatsSO statsSO;
    public Rigidbody playerRb;
    public LineRenderer lineRenderer;
}

