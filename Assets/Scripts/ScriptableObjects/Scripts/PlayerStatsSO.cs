
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "ScriptableObjects/PlayerStats", order = 1)]
public class PlayerStatsSO: ScriptableObject
{
    public float moveSpeed;
    public float groundDrag;
    public float jumpForce;
    public float jumpCd;
    public float airMult;
    public float playerHeight;
    public LayerMask groundLayer;
    public LayerMask entityLayer;
}

