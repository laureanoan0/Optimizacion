using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DefaultExecutionOrder(-50)]
public class GameplayController : MonoBehaviour
{
    [SerializeField] private EnemySpawnersSO enemySO;
    [SerializeField] private PlayerSO playerSO;
    [SerializeField] private ParticleSystem enemyDeathParticles;

    [SerializeField] private CanvasGroup pauseMenu;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button menuButton;

    private EntityManager entityManager;
    private WaveManager waveManager;

    private void Awake()
    {
        ServicesRegistration();
        waveManager = ServiceLocator.Get<WaveManager>();
        waveManager.OnGameWon += HandleGameWon;

        resumeButton.onClick.AddListener(ResumeGame);
        restartButton.onClick.AddListener(LoadGameplayScene);
        menuButton.onClick.AddListener(LoadMainMenuScene);

        GameEvents.OnSceneExit += CleanUp;
    }

    private void CleanUp()
    {
        if (waveManager != null)
        {
            waveManager.OnGameWon -= HandleGameWon;
        }

        resumeButton.onClick.RemoveListener(ResumeGame);
        restartButton.onClick.RemoveListener(LoadGameplayScene);
        menuButton.onClick.RemoveListener(LoadMainMenuScene);

        GameEvents.OnSceneExit -= CleanUp;
    }

    private void HandleGameWon()
    {
        LoadFinalScene(true);
    }

    public void ServicesRegistration()
    {
        ServiceLocator.Register(this);
        entityManager = new EntityManager(enemySO, playerSO, enemyDeathParticles);
        //ServiceLocator.Register(entityManager.Enemies);
    }

    public static UnityEngine.Object CreateObject(UnityEngine.Object enemy, Vector3 position)
    {
        return Instantiate(enemy, position, new Quaternion(0, 0, 0, 1));
    }

    public static (Rigidbody, UnityEngine.Object, LineRenderer) CreatePlayer(Rigidbody rbPrefab, UnityEngine.Object orientation, LineRenderer lineRend)
    {
        Rigidbody rbInstance = Instantiate(rbPrefab);
        UnityEngine.Object empty = Instantiate(orientation, rbInstance.transform);
        return (rbInstance, empty, lineRend);
    }

    public static void LoadGameplayScene()
    {
        GameEvents.ExitScene();
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameplayScene");
    }

    public static void LoadMainMenuScene()
    {
        GameEvents.ExitScene();
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("MainMenuScene");
    }

    public static void LoadFinalScene(bool playerWon)
    {
        GameEvents.ExitScene();
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        GameResult.PlayerWon = playerWon;
        Cursor.visible = true;
        SceneManager.LoadScene("FinalScreen");
    }

    public void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
        UpdateManager.Instance.SetPaused(true);
        pauseMenu.alpha = 1f;
        pauseMenu.blocksRaycasts = true;
        pauseMenu.interactable = true;
    }

    private void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1f;
        UpdateManager.Instance.SetPaused(false);
        pauseMenu.alpha = 0f;
        pauseMenu.blocksRaycasts = false;
        pauseMenu.interactable = false;
    }
}