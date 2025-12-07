using UnityEngine;

public class SpeedBonus : BonusBase
{
    [Header("Speed Bonus Settings")]
    [Tooltip("Во сколько раз умножить скорость игрока")]
    public float speedMultiplier = 1.5f;

    [Tooltip("Длительность действия бонуса (секунды)")]
    public float duration = 3f;

    protected override void ApplyBonus(GameObject player)
    {
        PlayerController controller = player.GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.ApplySpeedBoost(speedMultiplier, duration);
            UIManager.Instance?.ShowBonusMessage($"Speed x{speedMultiplier} for {duration:0.0}s");

            Debug.Log($"Speed bonus picked up: x{speedMultiplier} for {duration} seconds");
        }
    }
}
