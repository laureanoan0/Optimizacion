using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum Scenes { MainMenu, GameplayScene, FinalScreen}

public class UiManager : MonoBehaviour
{
    [Header("Comun")]
    [SerializeField] private Scenes CurrentScene = Scenes.MainMenu;

    [Header("Gameplay Scene")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private string prefix = "Score: ";

    [Header("Main Menu Scene")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private string gameplaySceneName = "GameplayScene"; 

    [Header("Final Scene")]
    [SerializeField] private Button replayButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private string menuSceneName = "MainMenuScene";
    

    private ScoreManager scoreManager;
    private AsyncOperation preloadGameplayScene;
    private AsyncOperation preloadMainMenuScene;

    private void Awake()
    {
        if(CurrentScene == Scenes.MainMenu)
        {
            InitializeMainMenu();
        }
        else if(CurrentScene == Scenes.GameplayScene) 
        {
            InitializeScoreDisplay();
        }else
        {
            InitializeFinalScreen();
        }
    }

    private void InitializeScoreDisplay()
    {
        scoreManager = ServiceLocator.Get<ScoreManager>();
        scoreManager.OnScoreChanged += UpdateScoreText;
        UpdateScoreText(scoreManager.Score);
    }
    private void InitializeMainMenu()
    {
        Debug.Log("HOLA");
        playButton.onClick.AddListener(OnPlayClicked);
        quitButton.onClick.AddListener(OnQuitClicked);

        preloadGameplayScene = SceneManager.LoadSceneAsync(gameplaySceneName);
        preloadGameplayScene.allowSceneActivation = false;
    }
    private void InitializeFinalScreen()
    {
        replayButton.onClick.AddListener(OnPlayClicked);
        mainMenuButton.onClick.AddListener(OnMenuClicked);

        preloadGameplayScene = SceneManager.LoadSceneAsync(gameplaySceneName);
        preloadGameplayScene.allowSceneActivation = false;

        preloadMainMenuScene = SceneManager.LoadSceneAsync(menuSceneName);
        preloadMainMenuScene.allowSceneActivation = false;
    }

    private void OnQuitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; 
#else
        Application.Quit();  
#endif
    }
    private void OnPlayClicked()
    {
        if(preloadGameplayScene != null)
        {
            Debug.Log("CHau");
           // preloadGameplayScene.allowSceneActivation = true;
        }
    }
    private void OnMenuClicked()
    {
        if (preloadMainMenuScene != null)
        {
            preloadMainMenuScene.allowSceneActivation = true;
        }
    }

    private void OnDisable()
    {
        if (scoreManager != null)
        {
            scoreManager.OnScoreChanged -= UpdateScoreText;
        }
    }
    private void UpdateScoreText(int newScore)
    {
        scoreText.text = prefix + newScore;
    }
}
