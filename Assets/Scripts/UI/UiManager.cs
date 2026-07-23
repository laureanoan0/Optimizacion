using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public enum Scenes { MainMenu, GameplayScene, FinalScreen }

public class UiManager : MonoBehaviour
{
    [Header("Comun")]
    [SerializeField] private Scenes CurrentScene = Scenes.MainMenu;

    [Header("Gameplay Scene")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private CanvasGroup waveText;
    [SerializeField] private TMP_Text waveNumber;
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private string scorePrefix = "Score: ";
    [SerializeField] private string wavePrefix = "Wave ";

    [Header("Main Menu Scene")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private string gameplaySceneName = "GameplayScene";

    [Header("Final Scene")]
    [SerializeField] private Button replayButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private string menuSceneName = "MainMenuScene";

    private ScoreManager scoreManager;
    private WaveManager waveManager;

    private void Awake()
    {
        if (CurrentScene == Scenes.MainMenu)
        {
            InitializeMainMenu();
        }
        else if (CurrentScene == Scenes.GameplayScene)
        {
            InitializeScoreDisplay();
        }
        else
        {
            InitializeFinalScreen();
        }
    }

    private void InitializeScoreDisplay()
    {
        scoreManager = ServiceLocator.Get<ScoreManager>();
        waveManager = ServiceLocator.Get<WaveManager>();

        waveManager.OnWaveFinished += ShowWaveChanged;
        scoreManager.OnScoreChanged += UpdateScoreText;
        UpdateScoreText(scoreManager.Score);
    }

    private void InitializeMainMenu()
    {
        ServiceLocator.Clear();
        playButton.onClick.AddListener(OnPlayClicked);
        quitButton.onClick.AddListener(OnQuitClicked);
    }

    private void InitializeFinalScreen()
    {
        ServiceLocator.Clear();

        if (GameplayController.PlayerWon)
            resultText.text = "¡GANASTE!";
        else
            resultText.text = "¡PERDISTE!";

        replayButton.onClick.AddListener(OnPlayClicked);
        mainMenuButton.onClick.AddListener(OnMenuClicked);
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
        SceneManager.LoadScene(gameplaySceneName);
    }

    private void OnMenuClicked()
    {
        SceneManager.LoadScene(menuSceneName);
    }

    private void OnDisable()
    {
        if (scoreManager != null)
        {
            scoreManager.OnScoreChanged -= UpdateScoreText;
            waveManager.OnWaveFinished -= ShowWaveChanged;
        }
    }

    private void UpdateScoreText(int newScore)
    {
        scoreText.text = scorePrefix + newScore;
    }

    private void ShowWaveChanged(int waveNumber)
    {
        StopAllCoroutines();
        this.waveNumber.text = wavePrefix + waveNumber;
        waveText.alpha = 1f;
        StartCoroutine(FadeText(waveText, 1.5f));
    }

    private IEnumerator FadeText(CanvasGroup text, float duration)
    {
        float startTime = Time.time;
        float endTime = Time.time + duration;

        while (Time.time < endTime)
        {
            float elapsedTime = Time.time - startTime;
            float t = elapsedTime / duration;
            text.alpha = Mathf.Lerp(1f, 0f, t);
            yield return null;
        }
        text.alpha = 0f;    
    }
}