using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Central game flow controller:
/// - Loads scenes (MenuScene/GameScene)
/// - Handles pause (ESC) and game over flow
/// - Starts/stops scoring and saves high score
/// - Keeps itself alive across scene loads
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Scenes")]
    public string menuSceneName = "MenuScene";
    public string gameSceneName = "GameScene";

    [Header("References (auto-found in GameScene)")]
    public ScoreManager scoreManager;

    [Header("State")]
    public bool isPaused = false;
    public bool isGameOver = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Always reset time scale when a new scene loads
        Time.timeScale = 1f;

        if (scene.name == gameSceneName)
        {
            // Unity 6+ replacement for FindObjectOfType
            scoreManager = FindFirstObjectByType<ScoreManager>();
            if (scoreManager != null)
                scoreManager.StartScoring();

            // Update HUD high score
            UIManager.Instance?.SetHighScore(SaveSystem.LoadHighScore());

            // Ensure panels are hidden at start
            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowPause(false);

                if (UIManager.Instance.gameOverPanel != null)
                    UIManager.Instance.gameOverPanel.SetActive(false);
            }

            isPaused = false;
            isGameOver = false;
        }

        if (scene.name == menuSceneName)
        {
            isPaused = false;
            isGameOver = false;
        }
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameSceneName, LoadSceneMode.Single);
    }

    public void ExitToMenu()
    {
        isPaused = false;
        isGameOver = false;

        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName, LoadSceneMode.Single);
    }

    public void TogglePause()
    {
        if (isGameOver) return;

        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;

        UIManager.Instance?.ShowPause(isPaused);
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        isPaused = false;
        Time.timeScale = 0f;

        int finalScore = 0;
        if (scoreManager != null)
        {
            scoreManager.StopScoring();
            finalScore = scoreManager.CurrentScore;
        }

        SaveSystem.SaveHighScore(finalScore);
        int highScore = SaveSystem.LoadHighScore();

        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowPause(false);
            UIManager.Instance.ShowGameOver(finalScore, highScore);
            UIManager.Instance.SetHighScore(highScore);
        }
    }
}
