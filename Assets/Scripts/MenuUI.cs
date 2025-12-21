using UnityEngine;
using TMPro;

public class MenuUI : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;

    private void OnEnable()
    {
        int hs = SaveSystem.LoadHighScore();
        if (highScoreText != null)
            highScoreText.text = $"High score: {hs}";
    }

    public void OnStartClicked()
    {
        GameManager.Instance?.StartGame();
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }
}
