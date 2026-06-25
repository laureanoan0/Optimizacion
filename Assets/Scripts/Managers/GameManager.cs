
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private EnemySpawnersSO enemySO;
    [SerializeField] private PlayerSO playerSO;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform orientation;

    private EntityManager entityManager;


    private void Awake()
    {
        ServicesRegistration();
    }
    public void ServicesRegistration()
    {
        entityManager = new EntityManager(enemySO, playerSO);
        //ServiceLocator.Register(entityManager.Enemies);
    }

    public static UnityEngine.Object CreateObject(UnityEngine.Object enemy, Vector3 position)
    {
        return Instantiate(enemy, position, new Quaternion(0,0,0,0));
    }

    public static (Rigidbody, UnityEngine.Object) CreatePlayer(Rigidbody rbPrefab, UnityEngine.Object orientation)
    {
        Rigidbody rbInstance = Instantiate(rbPrefab);
        UnityEngine.Object empty = Instantiate(orientation, rbInstance.transform);
        return (rbInstance, empty);
    }

    public static void LoadGameplayScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
}

