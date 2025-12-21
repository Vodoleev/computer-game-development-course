using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public string gameSceneName = "GameScene";

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != gameSceneName)
            return;

        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            GameManager.Instance?.TogglePause();
        }
    }

    public void OnResumeClicked()
    {
        GameManager.Instance?.TogglePause();
    }

    public void OnExitToMenuClicked()
    {
        GameManager.Instance?.ExitToMenu();
    }
}
