using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Endless runner player controller (Input System only):
/// - Constant forward movement (Z)
/// - Lane switching (Left/Right arrows or A/D)
/// - Jump (Space or Up arrow or W)
/// - Speed grows over time
/// - Speed boost (ApplySpeedBoost)
/// - Compatibility field "forwardSpeed" for DifficultyScaler
/// - Animator bool "SpeedBoostActive" while boost is active
///
/// Jump grounding is handled via collisions (most reliable for beginners):
/// - Player MUST have a Collider (not trigger)
/// - Ground MUST have a Collider (not trigger)
/// - Rigidbody on Player (Is Kinematic OFF)
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Forward speed")]
    public float baseSpeed = 7f;
    public float accelerationPerSecond = 0.25f;
    public float maxSpeed = 16f;

    // Compatibility with older scripts (DifficultyScaler expects forwardSpeed).
    [HideInInspector] public float forwardSpeed = 7f;

    [Header("Lane movement")]
    public float laneOffset = 2f;
    public float laneChangeSpeed = 8f;
    public bool forceCenterLaneOnStart = true;

    [Header("Jump")]
    public float jumpForce = 7f;

    [Tooltip("How much upward-facing normal is required to treat collision as ground (0.5 ~ slopes allowed).")]
    public float groundNormalMinY = 0.5f;

    [Header("Boost")]
    public bool useAnimatorBoostBool = true;

    // Runtime
    private Rigidbody rb;
    private Animator anim;

    // Lanes: 0 left, 1 center, 2 right
    [SerializeField] private int currentLane = 1;

    private float elapsedRunTime = 0f;

    private float speedMultiplier = 1f;
    private float speedBoostTimer = 0f;

    // Grounding via collisions
    private bool isGrounded = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        Time.timeScale = 1f;
    }

    private void Start()
    {
        rb.freezeRotation = true;

        // sync compatibility
        forwardSpeed = baseSpeed;

        // start lane
        if (forceCenterLaneOnStart)
            currentLane = 1;
        else
            currentLane = GetNearestLaneIndex(transform.position.x);

        SnapToLaneX();

        elapsedRunTime = 0f;
        speedMultiplier = 1f;
        speedBoostTimer = 0f;

        if (useAnimatorBoostBool)
            anim?.SetBool("SpeedBoostActive", false);
    }

    private void Update()
    {
        baseSpeed = Mathf.Max(0.1f, forwardSpeed);

        elapsedRunTime += Time.deltaTime;

        HandleInput_InputSystemOnly();
        HandleSpeedBoostTimer();

        UIManager.Instance?.SetSpeed(GetCurrentForwardSpeed());
    }

    private void FixedUpdate()
    {
        Vector3 pos = rb.position;

        float forward = GetCurrentForwardSpeed();
        pos.z += forward * Time.fixedDeltaTime;

        float targetX = LaneToX(currentLane);
        pos.x = Mathf.MoveTowards(pos.x, targetX, laneChangeSpeed * Time.fixedDeltaTime);

        rb.MovePosition(pos);
    }

    // ------------------- Public API -------------------

    public void ApplySpeedBoost(float multiplier, float duration)
    {
        if (multiplier <= 1f || duration <= 0f) return;

        speedMultiplier = multiplier;
        speedBoostTimer = duration;

        if (useAnimatorBoostBool)
            anim?.SetBool("SpeedBoostActive", true);
    }

    // ------------------- Input -------------------

    private void HandleInput_InputSystemOnly()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        bool leftDown = kb.leftArrowKey.wasPressedThisFrame || kb.aKey.wasPressedThisFrame;
        bool rightDown = kb.rightArrowKey.wasPressedThisFrame || kb.dKey.wasPressedThisFrame;

        if (leftDown) TryChangeLane(-1);
        if (rightDown) TryChangeLane(+1);

        bool jumpDown = kb.spaceKey.wasPressedThisFrame || kb.upArrowKey.wasPressedThisFrame || kb.wKey.wasPressedThisFrame;
        if (jumpDown) TryJump();
    }

    private void TryChangeLane(int delta)
    {
        currentLane = Mathf.Clamp(currentLane + delta, 0, 2);
    }

    private void TryJump()
    {
        if (!isGrounded) return;

        // reset vertical velocity for consistent jump
#if UNITY_6000_0_OR_NEWER
        Vector3 v = rb.linearVelocity;
        v.y = 0f;
        rb.linearVelocity = v;
#else
        Vector3 v = rb.velocity;
        v.y = 0f;
        rb.velocity = v;
#endif

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        // immediately mark not grounded (prevents double jump on same contact frame)
        isGrounded = false;
    }

    // ------------------- Grounding via collisions -------------------

    private void OnCollisionStay(Collision collision)
    {
        // If any contact normal points up enough, we treat it as ground
        for (int i = 0; i < collision.contactCount; i++)
        {
            if (collision.GetContact(i).normal.y >= groundNormalMinY)
            {
                isGrounded = true;
                return;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Leaving a collider may mean leaving ground; we'll re-confirm in Stay
        isGrounded = false;
    }

    // ------------------- Boost timer -------------------

    private void HandleSpeedBoostTimer()
    {
        if (speedBoostTimer <= 0f) return;

        speedBoostTimer -= Time.deltaTime;
        if (speedBoostTimer <= 0f)
        {
            speedBoostTimer = 0f;
            speedMultiplier = 1f;

            if (useAnimatorBoostBool)
                anim?.SetBool("SpeedBoostActive", false);
        }
    }

    // ------------------- Speed -------------------

    private float GetCurrentForwardSpeed()
    {
        float grown = baseSpeed + accelerationPerSecond * elapsedRunTime;
        float clamped = Mathf.Min(grown, maxSpeed);
        return clamped * speedMultiplier;
    }

    // ------------------- Lanes helpers -------------------

    private float LaneToX(int laneIndex)
    {
        return (laneIndex - 1) * laneOffset;
    }

    private int GetNearestLaneIndex(float x)
    {
        float leftX = LaneToX(0);
        float midX = LaneToX(1);
        float rightX = LaneToX(2);

        float dl = Mathf.Abs(x - leftX);
        float dm = Mathf.Abs(x - midX);
        float dr = Mathf.Abs(x - rightX);

        if (dm <= dl && dm <= dr) return 1;
        if (dl <= dr) return 0;
        return 2;
    }

    private void SnapToLaneX()
    {
        Vector3 p = rb.position;
        p.x = LaneToX(currentLane);
        rb.position = p;
    }
}
