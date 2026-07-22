using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreHUD : MonoBehaviour
{
    [Header("Comun")]
    [SerializeField] private bool isMainMenuScene;

    [Header("GameplayScene")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private string prefix = "Score: ";

    [Header("MainMenuScene")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private string gameplaySceneName = "SampleScene"; 
    

    private ScoreManager scoreManager;
    private AsyncOperation preloadOperation;

    private void Start()
    {
        if(isMainMenuScene)
        {
            InitializeMainMenu();
        }
        else
        {
            InitializeScoreDisplay();
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
        playButton.onClick.AddListener(OnPlayClicked);
        quitButton.onClick.AddListener(OnQuitClicked);

        preloadOperation = SceneManager.LoadSceneAsync(gameplaySceneName);
        preloadOperation.allowSceneActivation = false;
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
        playButton.interactable = false;

        if(preloadOperation != null)
        {
            preloadOperation.allowSceneActivation = true;
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
