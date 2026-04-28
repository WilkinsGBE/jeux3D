using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class PlayerHealth : MonoBehaviour, IDamageable
{

    [Header("UI")]
    public PlayerHUD playerHUD;

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

        if (playerHUD != null)
            playerHUD.Setup(this);

        if (UIManager.instance != null)
            UIManager.instance.SetHealth(1f);
    }

    public void TakeDamage(int damageAmount)
    {

        if (playerRoll != null && playerRoll.IsInvincible)
            return;

        if (isDead || isHit) return;

        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (UIManager.instance != null)
            UIManager.instance.SetHealth((float)currentHealth / maxHealth);

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

        if (UIManager.instance != null)
            UIManager.instance.SetHealth((float)currentHealth / maxHealth);

        Debug.Log("Player healed. Health: " + currentHealth);

        onHeal?.Invoke();
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        currentHealth = 0;

        Debug.Log("Player died.");

        isHit = false;

        if (animator != null)
        {
            animator.ResetTrigger(hitTriggerName);
            animator.SetTrigger(deathTriggerName);
        }

        onDeath?.Invoke();

        if (playerMovement != null)
            playerMovement.enabled = false;

        StartCoroutine(DeathDelay());
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

    private IEnumerator DeathDelay()
    {
        yield return new WaitForSecondsRealtime(2f);

        if (GameManager.instance != null)
            GameManager.instance.ShowDeathScreen();
    }
    public bool IsHit()
    {
        return isHit;
    }
}