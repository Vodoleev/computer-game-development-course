using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Health UI")]
    public Slider healthSlider;

    [Header("Speed UI")]
    public TextMeshProUGUI speedText;

    [Header("Bonus UI")]
    public TextMeshProUGUI bonusText;
    public float bonusMessageDuration = 2f;

    private float bonusTimer = 0f;

    private void Awake()
    {
        // Простейший синглтон для доступа из других скриптов
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        // Таймер для скрытия сообщения о бонусах
        if (bonusText != null && bonusTimer > 0f)
        {
            bonusTimer -= Time.deltaTime;
            if (bonusTimer <= 0f)
            {
                bonusText.text = "";
            }
        }
    }

    public void SetMaxHealth(int maxHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = maxHealth;
        }
    }

    public void SetHealth(int currentHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
    }

    public void SetSpeed(float speed)
    {
        if (speedText != null)
        {
            speedText.text = $"Speed: {speed:F1}";
        }
    }

    public void ShowBonusMessage(string message)
    {
        if (bonusText != null)
        {
            bonusText.text = message;
            bonusTimer = bonusMessageDuration;
        }
    }
}
