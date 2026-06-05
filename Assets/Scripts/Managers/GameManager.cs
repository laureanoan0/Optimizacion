
using System;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private EnemySO enemySO;
    [SerializeField] private PlayerSO playerSO;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform orientation;
    [SerializeField] private Rigidbody rb;

    private EntityManager enemyManager;

    public Action ServicesRegistration => () =>
    {
        enemyManager = new EntityManager(enemySO, playerSO);
        ServiceLocator.Register(enemyManager.enemies);
    };

    private void Awake()
    {
        ServicesRegistration?.Invoke();
    }

    public static UnityEngine.Object CreateEnemy(UnityEngine.Object enemy)
    {
        return Instantiate(enemy);
    }

    public static (UnityEngine.Object, UnityEngine.Object) CreatePlayer(UnityEngine.Object player, UnityEngine.Object orientation)
    {
        UnityEngine.Object playerO = Instantiate(player);
        UnityEngine.Object empty = Instantiate(orientation, playerO.GameObject().transform);
        return (playerO, empty);
    }
}

