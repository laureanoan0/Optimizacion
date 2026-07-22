
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-50)]
public class GameManager : MonoBehaviour
{
    [SerializeField] private EnemySpawnersSO enemySO;
    [SerializeField] private PlayerSO playerSO;
    [SerializeField] private ParticleSystem enemyDeathParticles;

    private EntityManager entityManager;


    private void Awake()
    {
        ServicesRegistration();
    }
    public void ServicesRegistration()
    {
        entityManager = new EntityManager(enemySO, playerSO, enemyDeathParticles);
        //ServiceLocator.Register(entityManager.Enemies);
    }

    public static UnityEngine.Object CreateObject(UnityEngine.Object enemy, Vector3 position)
    {
        return Instantiate(enemy, position, new Quaternion(0,0,0,0));
    }

    public static (Rigidbody, UnityEngine.Object, LineRenderer) CreatePlayer(Rigidbody rbPrefab, UnityEngine.Object orientation, LineRenderer lineRend)
    {
        Rigidbody rbInstance = Instantiate(rbPrefab);
        UnityEngine.Object empty = Instantiate(orientation, rbInstance.transform);
        return (rbInstance, empty, lineRend);
    }

    public static void LoadGameplayScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public static void LoadMainMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

