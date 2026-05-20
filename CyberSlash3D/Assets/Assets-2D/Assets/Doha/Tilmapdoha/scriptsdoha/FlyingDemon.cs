using UnityEngine;
//gere le demon
public class FlyingDemon : MonoBehaviour
{
    [Header("Health")]
    public float health = 25f;

    [Header("Stats")]
    public float attackRange = 5f;

    public float attackCooldown = 2f;

    public float moveSpeed = 2f;

    [Header("Attack")]
    public GameObject projectilePrefab;

    public Transform firePoint;

    [Header("References")]
    public Transform player;

    private Animator animator;
    private Rigidbody2D rb;
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip attackSound;
    public AudioClip hitSound;
    public AudioClip deathSound;

    // États du démon
    private bool isDead = false;
    private bool isHurt = false;

    // Timer attaque
    private float lastAttackTime;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // Trouve automatiquement le joueur si non assigné
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
        }
    }

    void Update()
    {
        // Ne fait rien si mort, sans joueur ou en état de blessure
        if (isDead || player == null || isHurt)
            return;

        // Distance entre le démon et le joueur
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Si le joueur est proche → attaque
        if (distanceToPlayer <= attackRange)
            AttackPlayer();
        else
            FlyTowardsPlayer();
    }

    void FlyTowardsPlayer()
    {
        // Désactive animation d'attaque
        animator.SetBool("isAttacking", false);

        // Direction vers le joueur
        Vector2 direction = (player.position - transform.position).normalized;

        // Déplacement du démon
        rb.linearVelocity = direction * moveSpeed;

        // Flip du sprite selon direction
        if (direction.x < 0)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    void AttackPlayer()
    {
        // Stop le mouvement
        rb.linearVelocity = Vector2.zero;

        // Animation attaque
        animator.SetBool("isAttacking", true);
        // SOUND ATTACK

        if (audioSource != null && attackSound != null)
            audioSource.PlayOneShot(attackSound);
        // Cooldown attaque
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;

            // Création projectile
            if (projectilePrefab != null && firePoint != null)
            {
                GameObject fb = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

                fireball fbScript = fb.GetComponent<fireball>();

                // Donne direction au projectile vers le joueur
                if (fbScript != null)
                {
                    Vector2 dir = (player.position - firePoint.position).normalized;
                    fbScript.SetDirection(dir);
                }
            }
        }
    }

    public void TakeDamage(float damage)
    {
        // Ignore si déjà mort
        if (isDead) return;

        health -= damage;
        // HIT SOUND
        if (audioSource != null && hitSound != null)
            audioSource.PlayOneShot(hitSound);

        Debug.Log("Flying Demon health: " + health);

        // Mort ou état blessé
        if (health <= 0)
            Die();
        else
            StartCoroutine(HurtRoutine());
    }

    private System.Collections.IEnumerator HurtRoutine()
    {
        // État "touché"
        isHurt = true;

        // Stop mouvement
        rb.linearVelocity = Vector2.zero;

        // Stop attaque + animation hit
        animator.SetBool("isAttacking", false);
        animator.SetTrigger("isHurt");

        // Petit délai de récupération
        yield return new WaitForSeconds(0.5f);

        isHurt = false;
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;
        isHurt = false;

        // Stop mouvement
        rb.linearVelocity = Vector2.zero;
         rb.gravityScale = 0f;     
    rb.bodyType = RigidbodyType2D.Kinematic;

        // Animation mort
        animator.SetBool("isAttacking", false);
        animator.SetTrigger("isDead");

        //  DEATH SOUND
        if (audioSource != null && deathSound != null)
            audioSource.PlayOneShot(deathSound);

        // Désactive collision
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        // Mise à jour GameManager
        GameManager2D.instance.DemonKilled();
        GameManager2D.instance.enemiesRemaining--;
        GameManager2D.instance.AddScore(100);
        GameManager2D.instance.CheckWinConditions();

        // Destruction après animation
        Destroy(gameObject, 1.5f);
    }

    // Dessine la portée d'attaque dans l'éditeur
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}