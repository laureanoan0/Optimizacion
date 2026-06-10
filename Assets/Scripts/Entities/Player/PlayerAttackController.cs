using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController
{
    private Transform playerTransform;
    private LayerMask entityLayer;

    public PlayerAttackController(Transform playerTransform, LayerMask entityLayer)
    {
        this.playerTransform = playerTransform;
        this.entityLayer = entityLayer;
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(playerTransform.position, playerTransform.forward, out hit, 1000f, entityLayer))
            {
                if (ServiceLocator.Get<Dictionary<UnityEngine.Object, IEnemyBehavior>>().TryGetValue(hit.collider.gameObject, out var enemyRef))
                {
                    enemyRef.TakeDamage();
                }
            }
        }
    }
}

