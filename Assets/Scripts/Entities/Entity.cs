using UnityEngine;

public enum EntityType { Enemy, Player, Camera}

public class Entity : MonoBehaviour, IStarteable
{
    [SerializeField] private EntityType type;
    [SerializeField] private PlayerStatsSO stats;
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform playerPos;

    private Rigidbody rb;
    private IEntityBehavior behavior;

    public void Awake()
    {
        UpdateManager.Instance.Register(this);
    }

    public void CustomStart()
    {
        rb = GetComponent<Rigidbody>();
        behavior = BehaviorFactory.CreateEntityBehavior(type, orientation, transform, playerPos, rb, stats, this);
    }

    private void OnDestroy()
    {
        UpdateManager.Instance.Unregister(this);
        behavior.Destroy();
    }
}
