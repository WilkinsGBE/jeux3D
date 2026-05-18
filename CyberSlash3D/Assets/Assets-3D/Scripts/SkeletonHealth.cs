using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class SkeletonHealth : MonoBehaviour, IDamageable
{
    [Header("Health")]
    public int maxHealth = 80;
    public int currentHealth;

    [Header("Revive")]
    public BossHealth bossHealth;
    public float reviveDelay = 5f;
    public float reviveAnimationLength = 1.5f;
    public float reviveStandDelay = 1f;

    [Header("Disable On Death")]
    public MonoBehaviour[] scriptsToDisable;

    [Header("Animation")]
    public Animator animator;
    public string deathTriggerName = "Die";
    public string reviveTriggerName = "Revive";
    public string hitTriggerName = "Hit";

    [Header("Events")]
    public UnityEvent onDeath;
    public UnityEvent onRevive;

    private bool isDead;
    private bool canRevive = true;

    private void Awake()
    {
        currentHealth = maxHealth;

        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth > 0 && animator != null)
            animator.SetTrigger(hitTriggerName);

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        currentHealth = 0;

        foreach (MonoBehaviour script in scriptsToDisable)
        {
            if (script != null)
                script.enabled = false;
        }

        SkeletonDamageHitbox[] hitboxes = GetComponentsInChildren<SkeletonDamageHitbox>();
        foreach (var hitbox in hitboxes)
            hitbox.gameObject.SetActive(false);

        if (animator != null)
            animator.SetTrigger(deathTriggerName);

        onDeath?.Invoke();

        if (canRevive && bossHealth != null && bossHealth.CurrentHealth > 0)
            StartCoroutine(ReviveAfterDelay());

        Debug.Log("Revive check | canRevive: " + canRevive +
          " | bossHealth assigned: " + (bossHealth != null) +
          " | boss HP: " + (bossHealth != null ? bossHealth.CurrentHealth : -1));
    }

    public void PermanentDeath()
    {
        canRevive = false;

        StopAllCoroutines();

        if (!isDead)
            Die();
    }

    private IEnumerator ReviveAfterDelay()
    {
        yield return new WaitForSeconds(reviveDelay);

        if (!canRevive || bossHealth == null || bossHealth.CurrentHealth <= 0)
            yield break;

        Revive();
    }

    private void Revive()
    {
        isDead = false;
        currentHealth = maxHealth;

        if (animator != null)
        {
            animator.ResetTrigger(deathTriggerName);
            animator.SetTrigger(reviveTriggerName);
        }

        StartCoroutine(EnableAfterReviveAnimation());
        Debug.Log("Skeleton reviving...");
    }

    private IEnumerator EnableAfterReviveAnimation()
    {
        yield return new WaitForSeconds(reviveAnimationLength);

        yield return new WaitForSeconds(reviveStandDelay);

        foreach (MonoBehaviour script in scriptsToDisable)
        {
            if (script != null)
                script.enabled = true;
        }

        SkeletonDamageHitbox[] hitboxes = GetComponentsInChildren<SkeletonDamageHitbox>(true);
        foreach (var hitbox in hitboxes)
            hitbox.gameObject.SetActive(true);

        SkeletonAI ai = GetComponent<SkeletonAI>();
        if (ai != null)
            ai.StartChasing();

        onRevive?.Invoke();
    }
}