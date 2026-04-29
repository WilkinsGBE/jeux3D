using UnityEngine;
using UnityEngine.Events;

public class BossHealth : MonoBehaviour, IDamageable
{
    [Header("Health")]
    public int maxHealth = 300;
    public int currentHealth;

    [Header("Hit Reaction")]
    public int hitThreshold = 150;
    private int accumulatedDamage = 0;

    [Header("Disable On Death")]
    public MonoBehaviour[] scriptsToDisable;

    [Header("Animation")]
    public Animator animator;
    public string hitTriggerName = "Hit";
    public string deathTriggerName = "Die";

    [Header("Boss Fight End")]
    public AudioSource bossMusic;
    public BossDoorTrigger bossDoorTrigger;

    [Header("Death Sound")]
    public AudioSource deathAudioSource; // Assign in Inspector
    public AudioClip deathSound;         // Assign in Inspector

    [Header("Events")]
    public UnityEvent onDeath;

    private bool isDead;

    [Header("Boss Info")]
    public string bossName = "Le Géant";

    public string BossName => bossName;
    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;

        if (animator == null)
            animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        accumulatedDamage += damageAmount;

        Debug.Log("Boss HP: " + currentHealth + " | Accumulated: " + accumulatedDamage);

        if (accumulatedDamage >= hitThreshold)
        {
            if (animator != null)
                animator.SetTrigger(hitTriggerName);

            accumulatedDamage -= hitThreshold;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        currentHealth = 0;

        Debug.Log("Boss died.");

        // 🎵 Stop boss music
        if (bossMusic != null)
            bossMusic.Stop();

        // 🔊 Play death sound (LOUD + 2D)
        if (deathAudioSource != null && deathSound != null)
        {
            deathAudioSource.PlayOneShot(deathSound, 2f); // 2f = louder
        }
        else
        {
            Debug.LogWarning("Death sound or AudioSource not assigned!");
        }

        // 🚪 Open doors
        if (bossDoorTrigger != null)
            bossDoorTrigger.UnlockAndOpenDoors();

        // ❌ Disable scripts
        foreach (MonoBehaviour script in scriptsToDisable)
        {
            if (script != null)
                script.enabled = false;
        }

        // ❌ Disable hitboxes
        DamageHitbox[] hitboxes = GetComponentsInChildren<DamageHitbox>();
        foreach (var hitbox in hitboxes)
        {
            hitbox.gameObject.SetActive(false);
        }

        // 🎬 Play death animation
        if (animator != null)
            animator.SetTrigger(deathTriggerName);

        onDeath?.Invoke();
    }
}