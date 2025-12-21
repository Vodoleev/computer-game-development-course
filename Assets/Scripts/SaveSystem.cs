using UnityEngine;

public static class SaveSystem
{
    private const string HighScoreKey = "HIGH_SCORE";

    public static int LoadHighScore()
    {
        return PlayerPrefs.GetInt(HighScoreKey, 0);
    }

    public static void SaveHighScore(int score)
    {
        int current = LoadHighScore();
        if (score > current)
        {
            PlayerPrefs.SetInt(HighScoreKey, score);
            PlayerPrefs.Save();
        }
    }

    public static void ResetHighScore()
    {
        PlayerPrefs.DeleteKey(HighScoreKey);
        PlayerPrefs.Save();
    }
}
