using UnityEngine;
//gere l'animation et tous ce qui a un raport avec l'angle
public class Angel : MonoBehaviour
{
    public float health = 100f;

    public bool vulnerable = false;

    [Header("Target")]
    public Transform player;

    [Header("Movement")]
    public float speed = 3f;

    public float attackRange = 2f;

    [Header("Attack")]
    public float damage = 20f;

    public float attackCooldown = 2f;

    private float nextAttackTime;

    public AngleZone zone;

    private Animator anim;
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip attackSound;
    public AudioClip deathSound;

    void Start()
    {
        // Récupère l'Animator du personnage
        anim = GetComponent<Animator>();

        // Si le joueur n’est pas assigné dans l’Inspector
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");

            if (p != null)
                player = p.transform;
        }
    }

    void Update()
    {
        if (player == null || zone == null)
            return;

        // Calcul distance entre ange et joueur
        float dist = Vector2.Distance(transform.position, player.position);

        // Si le joueur est loin → l’ange suit le joueur
        if (dist > attackRange)
        {
            Vector2 nextPos = Vector2.MoveTowards(
                transform.position,
                player.position,
                speed * Time.deltaTime
            );

            // Limite le déplacement dans la zone autorisée
            transform.position = zone.ClampPosition(nextPos);

            // Animation idle / déplacement
            if (anim != null)
                anim.SetBool("isAttacking", false);
        }
        else
        {
            // Si le joueur est proche → attaque
            Attack();
        }
    }

    void Attack()
    {
        //  empêche l'attaque si l'ange est mort
        if (health <= 0) return;

        if (Time.time < nextAttackTime)
            return;

        nextAttackTime = Time.time + attackCooldown;

        Debug.Log(" Angel attacks");
        // SON ATTACK
        if (audioSource != null && attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
        }

        // Déclenche animation attaque
        if (anim != null)
            anim.SetTrigger("Attack");

        // Récupère le script de vie du joueur
        HealthManager ph = player.GetComponent<HealthManager>();

        if (ph != null)
        {
            // Applique les dégâts au joueur
            ph.TakeDamage(damage);

            Debug.Log("Player damaged");
        }
        else
        {
            Debug.LogError("HealthManager missing");
        }
    }

    // Rend l’ange vulnérable (appelé après puzzle)
    public void MakeVulnerable()
    {
        vulnerable = true;
        Debug.Log("Angel vulnerable!");
    }

    // Fonction appelée quand l’ange reçoit des dégâts
    public void TakeDamage(float dmg)
    {
        // Si pas vulnérable → ignore les dégâts
        if (!vulnerable)
            return;

        health -= dmg;

        Debug.Log("Angel HP: " + health);

        // Si mort → destruction
        if (health <= 0)
            Die();
    }

    void Die()
    {
        Debug.Log(" ANGEL DEAD");
        // SON DEATH
        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        if (GameManager2D.instance != null)
            GameManager2D.instance.LevelComplete();

        // Supprime l’ange de la scène
        Destroy(gameObject);
    }
}