using UnityEngine;

// Базовый класс для всех бонусов
public abstract class BonusBase : MonoBehaviour
{
    [Header("Visual")]
    [Tooltip("Скорость вращения бонуса вокруг вертикальной оси")]
    public float rotateSpeed = 60f;

    private void Update()
    {
        // Красиво вращаем бонус, чтобы его было видно
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Срабатываем только при входе игрока
        if (other.CompareTag("Player"))
        {
            ApplyBonus(other.gameObject);
            Destroy(gameObject); // удаляем бонус после использования
        }
    }

    // Каждому наследнику нужно реализовать, что конкретно он делает
    protected abstract void ApplyBonus(GameObject player);
}
