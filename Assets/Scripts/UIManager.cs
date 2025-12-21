using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Health UI")]
    public Slider healthSlider;

    [Header("Score UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;

    [Header("Speed UI")]
    public TextMeshProUGUI speedText;

    [Header("Bonus UI")]
    public TextMeshProUGUI bonusText;
    public float bonusMessageDuration = 2f;

    [Header("Panels")]
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverScoreText;
    public TextMeshProUGUI gameOverHighScoreText;

    private float bonusTimer = 0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        if (bonusText != null && bonusTimer > 0f)
        {
            bonusTimer -= Time.deltaTime;
            if (bonusTimer <= 0f)
                bonusText.text = "";
        }
    }

    public void SetMaxHealth(int maxHealth)
    {
        if (healthSlider == null) return;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
    }

    public void SetHealth(int currentHealth)
    {
        if (healthSlider == null) return;
        healthSlider.value = currentHealth;
    }

    public void SetScore(int score)
    {
        if (scoreText != null)
            scoreText.text = $"Score: {score}";
    }

    public void SetHighScore(int highScore)
    {
        if (highScoreText != null)
            highScoreText.text = $"High: {highScore}";
    }

    public void SetSpeed(float speed)
    {
        if (speedText != null)
            speedText.text = $"Speed: {speed:F2}";
    }

    public void ShowBonusMessage(string message)
    {
        if (bonusText == null) return;
        bonusText.text = message;
        bonusTimer = bonusMessageDuration;
    }

    public void ShowPause(bool show)
    {
        if (pausePanel != null)
            pausePanel.SetActive(show);
    }

    public void ShowGameOver(int score, int highScore)
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (gameOverScoreText != null)
            gameOverScoreText.text = $"Score: {score}";

        if (gameOverHighScoreText != null)
            gameOverHighScoreText.text = $"High score: {highScore}";
    }
}
