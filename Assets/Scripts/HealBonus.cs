using UnityEngine;

public class HealBonus : BonusBase
{
    [Header("Heal Settings")]
    [Tooltip("На сколько здоровья вылечить игрока")]
    public int healAmount = 20;

    protected override void ApplyBonus(GameObject player)
    {
        PlayerHealth health = player.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.Heal(healAmount);
            UIManager.Instance?.ShowBonusMessage($"+{healAmount} HP");

            Debug.Log($"Heal bonus picked up: +{healAmount} HP");
        }
    }
}
