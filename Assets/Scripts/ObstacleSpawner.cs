using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("References")]
    public Transform player;              // игрок
    public GameObject[] obstaclePrefabs;  // префабы препятствий
    public GameObject[] bonusPrefabs;     // префабы бонусов (HealBonus, SpeedBonus)

    [Header("Obstacle Spawn Settings")]
    [Tooltip("На каком расстоянии ВПЕРЁДИ от игрока начинать спавн по Z (для ОГРАНИЧЕНИЙ и бонусов)")]
    public float baseSpawnDistanceAhead = 20f;

    [Tooltip("Минимальное случайное смещение по Z")]
    public float minZOffset = 5f;

    [Tooltip("Максимальное случайное смещение по Z")]
    public float maxZOffset = 10f;

    [Tooltip("Интервал времени между спавнами препятствий (секунды)")]
    public float spawnInterval = 2f;      // ВАЖНО: это использует DifficultyScaler

    [Header("Lanes")]
    [Tooltip("Расстояние между полосами (то же, что у PlayerController.laneWidth)")]
    public float laneWidth = 2f;

    [Header("Bonus Spawn Settings")]
    [Tooltip("Интервал времени между спавнами бонусов (секунды)")]
    public float bonusSpawnInterval = 4f;

    [Tooltip("Высота появления бонуса над землёй")]
    public float bonusHeight = 1f;

    private float obstacleTimer = 0f;
    private float bonusTimer = 0f;

    private void Update()
    {
        if (player == null) return;

        // --- ОБСТРЕЛ ПРЕПЯТСТВИЯМИ ---
        if (obstaclePrefabs != null && obstaclePrefabs.Length > 0)
        {
            obstacleTimer += Time.deltaTime;
            if (obstacleTimer >= spawnInterval)
            {
                obstacleTimer = 0f;
                SpawnObstacle();
            }
        }

        // --- НЕЗАВИСИМЫЙ СПАВН БОНУСОВ ---
        if (bonusPrefabs != null && bonusPrefabs.Length > 0)
        {
            bonusTimer += Time.deltaTime;
            if (bonusTimer >= bonusSpawnInterval)
            {
                bonusTimer = 0f;
                SpawnBonus();
            }
        }
    }

    private void SpawnObstacle()
    {
        // случайный префаб препятствия
        GameObject obstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

        // случайная полоса: -1, 0, 1
        int laneIndex = Random.Range(-1, 2);
        float x = laneIndex * laneWidth;

        // позиция по Z впереди игрока
        float z = player.position.z + baseSpawnDistanceAhead + Random.Range(minZOffset, maxZOffset);

        Vector3 obstaclePos = new Vector3(x, 0.5f, z);
        Instantiate(obstaclePrefab, obstaclePos, Quaternion.identity);
    }

    private void SpawnBonus()
    {
        // случайный префаб бонуса
        GameObject bonusPrefab = bonusPrefabs[Random.Range(0, bonusPrefabs.Length)];

        // случайная полоса (НЕ обязана совпадать с полосой препятствия)
        int laneIndex = Random.Range(-1, 2);
        float x = laneIndex * laneWidth;

        // позиция по Z вперёд по ходу движения
        float z = player.position.z + baseSpawnDistanceAhead + Random.Range(minZOffset, maxZOffset);

        Vector3 bonusPos = new Vector3(x, bonusHeight, z);
        Instantiate(bonusPrefab, bonusPos, Quaternion.identity);
    }
}
