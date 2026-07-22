using System;

public class ScoreManager
{
    public event Action<int> OnScoreChanged;

    private int score;
    public int Score => score;

    public ScoreManager()
    {
        ServiceLocator.Register(this);
    }

    public void AddScore(int amount)
    {
        if(amount == 0) return;
        
        score += amount;
        OnScoreChanged?.Invoke(score);
    }

    public void ResetScore()
    {
        score = 0;
        OnScoreChanged?.Invoke(score);
    }

}
