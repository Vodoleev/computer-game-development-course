using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [Header("Damage")]
    [Tooltip("Сколько урона наносит игроку при столкновении")]
    public int damage = 20;

    [Header("Lifetime")]
    [Tooltip("На сколько позади игрока объект должен уйти, чтобы его удалить")]
    public float destroyDistanceBehindPlayer = 15f;

    private Transform player;

    private void Start()
    {
        // Находим игрока по тегу
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("Obstacle: Player with tag 'Player' not found!");
        }
    }

    private void Update()
    {
        if (player == null) return;

        // Если препятствие сильно позади игрока — удаляем
        if (transform.position.z < player.position.z - destroyDistanceBehindPlayer)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Срабатывает, когда игрок "влетает" в триггер
        if (other.CompareTag("Player"))
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }

            // Можно (необязательно) уничтожать препятствие сразу:
            // Destroy(gameObject);
        }
    }
}
