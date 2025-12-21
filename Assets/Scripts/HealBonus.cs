using UnityEngine;

public class HealBonus : MonoBehaviour
{
    [Header("Heal Settings")]
    public int healAmount = 25;

    [Header("UI Message")]
    public string message = "+HP";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // 1) Bonus pickup animation (Animator Trigger: "Bonus")
        other.GetComponentInParent<Animator>()?.SetTrigger("Bonus");

        // 2) Apply heal
        var health = other.GetComponentInParent<PlayerHealth>();
        if (health != null)
            health.Heal(healAmount);

        // 3) UI message (optional)
        UIManager.Instance?.ShowBonusMessage(message);

        // 4) Remove bonus object
        Destroy(gameObject);
    }
}
