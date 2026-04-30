using UnityEngine;
using UnityEngine.Events;
using System.Collections;

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

    [Header("Ambient Music Resume")]
    public float ambientResumeDelay = 3f;

    [Header("Death Sound")]
    public AudioSource deathAudioSource;
    public AudioClip deathSound;

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
            Die();
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        currentHealth = 0;

        Debug.Log("Boss died.");

        if (GameManager.instance != null)
            GameManager.instance.AddScore(1250);

        if (bossMusic != null)
            bossMusic.Stop();

        if (deathAudioSource != null && deathSound != null)
            deathAudioSource.PlayOneShot(deathSound, 2f);
        else
            Debug.LogWarning("Death sound or AudioSource not assigned!");

        if (bossDoorTrigger != null)
            bossDoorTrigger.UnlockAndOpenDoors();

        foreach (MonoBehaviour script in scriptsToDisable)
        {
            if (script != null)
                script.enabled = false;
        }

        DamageHitbox[] hitboxes = GetComponentsInChildren<DamageHitbox>();
        foreach (var hitbox in hitboxes)
            hitbox.gameObject.SetActive(false);

        if (animator != null)
            animator.SetTrigger(deathTriggerName);

        StartCoroutine(ResumeAmbientMusicDelayed());

        onDeath?.Invoke();
    }

    private IEnumerator ResumeAmbientMusicDelayed()
    {
        yield return new WaitForSecondsRealtime(ambientResumeDelay);

        if (GameManager.instance != null)
            GameManager.instance.PlayAmbientMusic();
    }
}