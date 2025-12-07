using UnityEngine;

public class DifficultyScaler : MonoBehaviour
{
    [Header("References")]
    public PlayerController playerController;
    public ObstacleSpawner obstacleSpawner;

    [Header("Forward Speed Growth")]
    [Tooltip("На сколько единиц в СЕКУНДУ увеличивать скорость вперёд")]
    public float speedIncreasePerSecond = 0.3f;

    [Header("Spawn Interval Settings")]
    [Tooltip("Минимальный интервал спавна препятствий (секунды)")]
    public float minSpawnInterval = 0.6f;

    private float initialSpawnInterval;
    private float elapsedTime = 0f;

    private void Start()
    {
        if (obstacleSpawner != null)
        {
            initialSpawnInterval = obstacleSpawner.spawnInterval;
        }
        else
        {
            Debug.LogWarning("DifficultyScaler: ObstacleSpawner is not assigned!");
        }
    }

    private void Update()
    {
        if (playerController == null || obstacleSpawner == null)
            return;

        elapsedTime += Time.deltaTime;

        // 1) Увеличиваем скорость игрока
        float deltaSpeed = speedIncreasePerSecond * Time.deltaTime;
        playerController.forwardSpeed += deltaSpeed;

        // 2) Плавно уменьшаем интервал спавна препятствий
        float t = Mathf.Clamp01(elapsedTime / 60f); // за минуту дойдём до минимума
        obstacleSpawner.spawnInterval = Mathf.Lerp(initialSpawnInterval, minSpawnInterval, t);
    }
}
