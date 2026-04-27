using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Animation")]
    public Animator animator;
    public string hitTriggerName = "Hit";
    public string deathTriggerName = "Die";

    [Header("Movement")]
    public MonoBehaviour playerMovement;

    [Header("Events")]
    public UnityEvent onTakeDamage;
    public UnityEvent onHeal;
    public UnityEvent onDeath;

    [Header("Hit Settings")]
    public float hitStunDuration = 2f;

    private Coroutine hitCoroutine;
    private PlayerRoll playerRoll;

    private bool isDead = false;
    private bool isHit = false;

    private void Awake()
    {
        currentHealth = maxHealth;

        if (animator == null)
            animator = GetComponent<Animator>();

        playerRoll = GetComponent<PlayerRoll>();
    }

    public void TakeDamage(int damageAmount)
    {

        if (playerRoll != null && playerRoll.IsInvincible)
            return;

        if (isDead || isHit) return;

        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log("Player took damage. Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        if (hitCoroutine != null)
            StopCoroutine(hitCoroutine);

        hitCoroutine = StartCoroutine(HitStun());

        onTakeDamage?.Invoke();
    }

    public void Heal(int healAmount)
    {
        if (isDead) return;

        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log("Player healed. Health: " + currentHealth);

        onHeal?.Invoke();
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        currentHealth = 0;

        Debug.Log("Player died.");

        // Stop hit state
        isHit = false;

        if (animator != null)
        {
            animator.ResetTrigger("Hit"); // stop hit animation
            animator.SetTrigger(deathTriggerName);
        }

        onDeath?.Invoke();

        if (playerMovement != null)
            playerMovement.enabled = false;
    }

    public void EnableMovementAfterHit()
    {
        if (isDead) return;

        isHit = false;

        if (playerMovement != null)
            playerMovement.enabled = true;

        hitCoroutine = null;
    }

    public bool IsDead()
    {
        return isDead;
    }

    private IEnumerator HitStun()
    {
        isHit = true;

        if (playerMovement != null)
            playerMovement.enabled = false;

        if (animator != null)
        {
            animator.ResetTrigger("Attack");
            animator.SetTrigger(hitTriggerName);
        }

        yield return new WaitForSeconds(hitStunDuration);

        EnableMovementAfterHit();
    }
    public bool IsHit()
    {
        return isHit;
    }
}