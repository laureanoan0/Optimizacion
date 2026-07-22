using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController: IUpdateable
{
    private Transform playerTransform;
    private LayerMask entityLayer;
    private float shootDistance = 1000f;
    private LineRendererPool lineRendPool;

    float rightOffset = 0.5f;
    float upOffset = 0f;

    public PlayerAttackController(Transform playerTransform, LayerMask entityLayer, LineRenderer lineRend)
    {
        this.playerTransform = playerTransform;
        this.entityLayer = entityLayer;
        lineRendPool = new LineRendererPool(lineRend);
        UpdateManager.Instance.Register(this);
    }

    public void Destroy()
    {
        UpdateManager.Instance.Unregister(this);
    }

    public void CustomUpdate(float time)
    {
        if (Input.GetMouseButtonDown(0))
        {


            Vector3 startPos = playerTransform.position + playerTransform.right * rightOffset + playerTransform.up * upOffset;
            Vector3 direcction = playerTransform.forward;

            RaycastHit hit;
            bool didHit = Physics.Raycast(playerTransform.position, playerTransform.forward, out hit, shootDistance, entityLayer);

            Vector3 endPoint = didHit ? hit.point : startPos + direcction * (shootDistance / 4);
            
            lineRendPool.Shoot(startPos, endPoint);

            if (didHit)
            {
                if (ServiceLocator.Get<Dictionary<UnityEngine.Object, IEnemyBehavior>>().TryGetValue(hit.collider.gameObject, out var enemyRef))
                {
                    enemyRef.TakeDamage();
                }
            }
        }
    }
}

