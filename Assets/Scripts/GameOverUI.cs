using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public void OnRestartClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameScene");
    }

    public void OnExitToMenuClicked()
    {
        Time.timeScale = 1f;
        GameManager.Instance?.ExitToMenu();
    }
}
