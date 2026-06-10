
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


    private void Awake()
    {
        ServicesRegistration();
    }
    public void ServicesRegistration()
    {
        enemyManager = new EntityManager(enemySO, playerSO);
        Debug.Log("Registre desde game manager");
        ServiceLocator.Register(enemyManager.enemies);

    }
    public static UnityEngine.Object CreateObject(UnityEngine.Object enemy, Vector3 position)
    {
        return Instantiate(enemy, position, new Quaternion(0,0,0,0));
    }

    public static (UnityEngine.Object, UnityEngine.Object) CreatePlayer(UnityEngine.Object player, UnityEngine.Object orientation)
    {
        UnityEngine.Object playerO = Instantiate(player);
        UnityEngine.Object empty = Instantiate(orientation, playerO.GameObject().transform);
        return (playerO, empty);
    }
}

