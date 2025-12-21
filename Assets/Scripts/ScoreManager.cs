using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Header("Score Settings")]
    [Tooltip("Очков в секунду")]
    public float scorePerSecond = 10f;

    public int CurrentScore { get; private set; }

    private float scoreFloat = 0f;
    private bool isRunning = false;

    public void StartScoring()
    {
        scoreFloat = 0f;
        CurrentScore = 0;
        isRunning = true;
    }

    public void StopScoring()
    {
        isRunning = false;
    }

    private void Update()
    {
        if (!isRunning) return;

        scoreFloat += scorePerSecond * Time.deltaTime;
        int newScore = Mathf.FloorToInt(scoreFloat);

        if (newScore != CurrentScore)
        {
            CurrentScore = newScore;
            UIManager.Instance?.SetScore(CurrentScore);
        }
    }
}
