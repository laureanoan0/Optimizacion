using UnityEngine;

public class PlayerBehavior: IUpdateable, IFixedUpdateables
{
    private CameraBehavior camera;
    private PlayerMovementController movementCon;
    private PlayerAttackController attackCon;

    public PlayerBehavior(Transform orientation, Transform playerTransform, Rigidbody rb, PlayerStatsSO stats, LayerMask entityLayer)
    {
        camera = new CameraBehavior(playerTransform, orientation);
        movementCon = new PlayerMovementController(orientation, playerTransform, rb, stats);
        attackCon = new PlayerAttackController(orientation, entityLayer);

        UpdateManager.Instance.Register((IUpdateable)this);
        UpdateManager.Instance.Register((IFixedUpdateables)this);
    }

    public void Die()
    {
        Destroy();
        GameManager.LoadGameplayScene();
    }
    public void Destroy()
    {
        UpdateManager.Instance.Unregister((IUpdateable)this);
        UpdateManager.Instance.Unregister((IFixedUpdateables)this);
    }

    public void CustomUpdate(float time)
    {
        movementCon.CustomUpdate(time);
        attackCon.Update();
    }

    public void CustomFixedUpdate()
    {
        movementCon.CustomFixedUpdate();
    }
}
