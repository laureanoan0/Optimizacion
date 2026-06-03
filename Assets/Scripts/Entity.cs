using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityType { Enemy, Player, Camera}

public class Entity : MonoBehaviour
{
    [SerializeField] private EntityType type;
    [SerializeField] private PlayerStatsSO stats;
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform playerPos;

    private Rigidbody rb;

    private IEntityBehavior behavior;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        behavior = BehaviorFactory.CreateEntityBehavior(type, orientation, transform, playerPos, rb, stats);
    }

    private void OnDestroy()
    {
        behavior.Destroy();
    }
}
