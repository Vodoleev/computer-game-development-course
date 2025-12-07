using UnityEngine;
using UnityEngine.InputSystem; // новая система ввода

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Forward Movement")]
    [Tooltip("Базовая скорость движения вперёд")]
    public float forwardSpeed = 7f;

    [Header("Lanes")]
    [Tooltip("Расстояние между полосами по X")]
    public float laneWidth = 2f;
    [Tooltip("Скорость перестроения между полосами")]
    public float laneChangeSpeed = 10f;

    [Header("Jump")]
    [Tooltip("Сила прыжка")]
    public float jumpForce = 7f;
    [Tooltip("Максимальное число прыжков подряд (1 или 2)")]
    public int maxJumps = 1;

    [Header("Speed Boost")]
    [Tooltip("Текущий множитель скорости (1 = без буста)")]
    public float speedMultiplier = 1f;

    private Rigidbody rb;
    private int currentLane = 0;   // -1 (левая), 0 (центр), 1 (правая)
    private int jumpsUsed = 0;

    // таймер действия буста
    private float speedBoostTimer = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        HandleSpeedBoostTimer();
        MoveForward();
        HandleLaneInput();
        HandleJumpInput();
    }

    // === ЛОГИКА БУСТА СКОРОСТИ ===

    private void HandleSpeedBoostTimer()
    {
        if (speedBoostTimer > 0f)
        {
            speedBoostTimer -= Time.deltaTime;
            if (speedBoostTimer <= 0f)
            {
                speedMultiplier = 1f; // буст закончился
            }
        }
    }

    public void ApplySpeedBoost(float multiplier, float duration)
    {
        if (multiplier <= 0f) return;

        speedMultiplier = multiplier;
        speedBoostTimer = duration;
    }

    // === ДВИЖЕНИЕ ВПЕРЁД ===

    private void MoveForward()
    {
        float currentSpeed = forwardSpeed * speedMultiplier;

        // Обновляем текст скорости на экране
        UIManager.Instance?.SetSpeed(currentSpeed);

        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
    }


    // === ПОЛОСЫ ===

    private void HandleLaneInput()
    {
        if (Keyboard.current == null) return;

        // реагируем на само нажатие, а не удержание
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame || Keyboard.current.aKey.wasPressedThisFrame)
        {
            ChangeLane(-1);
        }
        else if (Keyboard.current.rightArrowKey.wasPressedThisFrame || Keyboard.current.dKey.wasPressedThisFrame)
        {
            ChangeLane(1);
        }

        float targetX = currentLane * laneWidth;
        Vector3 pos = transform.position;
        pos.x = Mathf.MoveTowards(pos.x, targetX, laneChangeSpeed * Time.deltaTime);
        transform.position = pos;
    }

    private void ChangeLane(int direction)
    {
        int targetLane = currentLane + direction;
        targetLane = Mathf.Clamp(targetLane, -1, 1); // не выходим за -1..1
        currentLane = targetLane;
    }

    // === ПРЫЖОК ===

    private void HandleJumpInput()
    {
        if (Keyboard.current == null) return;

        bool jumpPressed =
            Keyboard.current.spaceKey.wasPressedThisFrame ||
            Keyboard.current.upArrowKey.wasPressedThisFrame ||
            Keyboard.current.wKey.wasPressedThisFrame;

        if (jumpPressed)
        {
            TryJump();
        }
    }

    private void TryJump()
    {
        if (jumpsUsed < maxJumps)
        {
            // сбрасываем вертикальную скорость и прыгаем
            Vector3 vel = rb.velocity;
            vel.y = 0f;
            rb.velocity = vel;

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpsUsed++;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpsUsed = 0;
        }
    }
}
