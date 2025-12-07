using UnityEngine;
using UnityEngine.SceneManagement; // для перезапуска сцены

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;

    [SerializeField]
    private int currentHealth;

    private bool isDead = false;

    private void Start()
    {
        // В начале игры здоровье = максимум
        currentHealth = maxHealth;
        UIManager.Instance?.SetMaxHealth(maxHealth);
        UIManager.Instance?.SetHealth(currentHealth);

        Debug.Log($"Player HP: {currentHealth}/{maxHealth}");
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);
        UIManager.Instance?.SetHealth(currentHealth);

        Debug.Log($"Player took {amount} damage. HP: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (isDead) return;

        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        UIManager.Instance?.SetHealth(currentHealth);

        Debug.Log($"Player healed {amount}. HP: {currentHealth}/{maxHealth}");
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("Player died! Restarting scene...");

        // Перезапускаем текущую сцену
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.buildIndex);
    }
}
