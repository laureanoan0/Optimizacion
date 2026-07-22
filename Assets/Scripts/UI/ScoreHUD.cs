using UnityEngine;
using TMPro;

public class ScoreHUD : MonoBehaviour
{

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private string prefix = "Score: ";

    private ScoreManager scoreManager;

    private void Start()
    {
        scoreManager = ServiceLocator.Get<ScoreManager>();
        scoreManager.OnScoreChanged += UpdateScoreText;
        UpdateScoreText(scoreManager.Score);
    }
    private void OnEnable()
    {
        scoreManager = ServiceLocator.Get<ScoreManager>();
        scoreManager.OnScoreChanged += UpdateScoreText;
        UpdateScoreText(scoreManager.Score);
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
        scoreText.text =prefix + newScore;
    }
}
