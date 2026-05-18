using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerRoll : MonoBehaviour
{
    public float rollSpeed = 8f;
    public float rollDuration = 0.5f;
    public float rollCooldown = 1f;

    public KeyCode rollKey = KeyCode.LeftShift;

    [Header("Invincibility Frames")]
    public float iFrameStart = 0.05f;
    public float iFrameEnd = 0.35f;

    private Animator animator;
    private CharacterController controller;
    private PlayerAttack playerAttack;
    private PlayerHealth playerHealth;

    private bool isRolling = false;
    private bool isInvincible = false;

    private float rollTimer = 0f;
    private float cooldownTimer = 0f;

    private Vector3 rollDirection;

    public bool IsRolling => isRolling;
    public bool IsInvincible => isInvincible;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        playerAttack = GetComponent<PlayerAttack>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        HandleRoll();
    }

    void HandleRoll()
    {
        if (cooldownTimer > 0)
            cooldownTimer -= Time.deltaTime;

        if (Input.GetKeyDown(rollKey) && !isRolling && cooldownTimer <= 0)
        {
            if (playerHealth != null && (playerHealth.IsDead() || playerHealth.IsHit()))
                return;

            if (playerAttack != null && playerAttack.IsAttacking)
                return;

            StartRoll();
        }

        if (isRolling)
        {
            rollTimer -= Time.deltaTime;

            float elapsed = rollDuration - rollTimer;

            isInvincible = elapsed >= iFrameStart && elapsed <= iFrameEnd;

            controller.Move(rollDirection * rollSpeed * Time.deltaTime);

            if (rollTimer <= 0)
                EndRoll();
        }
    }

    void StartRoll()
    {
        isRolling = true;
        isInvincible = false;

        rollTimer = rollDuration;
        cooldownTimer = rollCooldown;

        rollDirection = transform.forward;

        animator.ResetTrigger("Attack1");
        animator.ResetTrigger("Attack2");
        animator.SetTrigger("Roll");
    }

    void EndRoll()
    {
        isRolling = false;
        isInvincible = false;
        rollDirection = Vector3.zero;
    }
}