using UnityEngine;

/// <summary>
/// Controls player health, damage handling and death.
/// Shows health in UI via UIManager, triggers Animator feedback on damage,
/// and notifies GameManager on death.
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;

    [Header("Runtime (read-only)")]
    [SerializeField] private int currentHealth;

    private bool isDead = false;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        currentHealth = maxHealth;

        // UI init
        if (UIManager.Instance != null)
        {
            UIManager.Instance.SetMaxHealth(maxHealth);
            UIManager.Instance.SetHealth(currentHealth);
        }
    }

    /// <summary>
    /// Apply damage to the player. Triggers damage animation and updates UI.
    /// </summary>
    public void TakeDamage(int amount)
    {
        if (isDead) return;
        if (amount <= 0) return;

        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;

        // Damage feedback animation (Animator Trigger: "Damage")
        anim?.SetTrigger("Damage");

        // UI update
        if (UIManager.Instance != null)
            UIManager.Instance.SetHealth(currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    /// <summary>
    /// Heal the player and update UI (optional).
    /// </summary>
    public void Heal(int amount)
    {
        if (isDead) return;
        if (amount <= 0) return;

        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        if (UIManager.Instance != null)
            UIManager.Instance.SetHealth(currentHealth);
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        // Inform game manager (will show GameOver, save score, etc.)
        GameManager.Instance?.GameOver();
    }
}
