using UnityEngine;

public class BonusSpawner : MonoBehaviour
{
    [Header("References")]
    public Transform player;             // Игрок
    public GameObject[] bonusPrefabs;    // Префабы бонусов (HealBonus, SpeedBonus и т.п.)

    [Header("Spawn Settings")]
    [Tooltip("На каком расстоянии ВПЕРЁДИ от игрока начинать спавн по Z")]
    public float baseSpawnDistanceAhead = 25f;

    [Tooltip("Минимальное случайное смещение по Z")]
    public float minZOffset = 0f;

    [Tooltip("Максимальное случайное смещение по Z")]
    public float maxZOffset = 10f;

    [Tooltip("Интервал времени между спавнами бонусов (секунды)")]
    public float spawnInterval = 4f;

    [Header("Lanes")]
    [Tooltip("Расстояние между полосами (то же, что у PlayerController.laneWidth)")]
    public float laneWidth = 2f;

    [Header("Height")]
    [Tooltip("Высота появления бонуса над землёй")]
    public float spawnHeight = 1f;

    private float timer = 0f;

    private void Update()
    {
        if (player == null || bonusPrefabs == null || bonusPrefabs.Length == 0)
            return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnBonus();
        }
    }

    private void SpawnBonus()
    {
        // случайный префаб бонуса
        GameObject prefab = bonusPrefabs[Random.Range(0, bonusPrefabs.Length)];

        // случайная полоса: -1, 0 или 1
        int laneIndex = Random.Range(-1, 2);
        float x = laneIndex * laneWidth;

        // Z = позиция игрока + базовое расстояние + случайный оффсет
        float z = player.position.z + baseSpawnDistanceAhead + Random.Range(minZOffset, maxZOffset);

        Vector3 spawnPos = new Vector3(x, spawnHeight, z);
        Instantiate(prefab, spawnPos, Quaternion.identity);
    }
}
