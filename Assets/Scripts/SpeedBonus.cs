using UnityEngine;

public class SpeedBonus : MonoBehaviour
{
    public float multiplier = 1.5f;
    public float duration = 3f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // 1) анимация получения бонуса
        other.GetComponentInParent<Animator>()?.SetTrigger("Bonus");

        // 2) применить буст
        other.GetComponentInParent<PlayerController>()?.ApplySpeedBoost(multiplier, duration);

        UIManager.Instance?.ShowBonusMessage("Speed boost!");

        Destroy(gameObject);
    }
}
