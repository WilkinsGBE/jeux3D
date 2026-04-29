using UnityEngine;
using UnityEngine.AI;

// Ce script gère un agent ennemi : patrouille + vie + dégâts + mort
public class ControlleurAgent : MonoBehaviour
{
    [Header("Patrol")]
    public Transform[] points;        // Points de patrouille
    private int currentIndex = 0;     // Index du point actuel

    [Header("Components")]
    private NavMeshAgent agent;       // Navigation IA
    public Animator animator;        // Animation

    [Header("Stats")]
    public int health = 3;           // Vie de l'agent
    private bool isDead = false;     // État mort

    // ===================== START =====================
    void Start()
    {
        // Récupère le NavMeshAgent
        agent = GetComponent<NavMeshAgent>();

        // Récupère Animator si pas assigné
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        // Va vers le premier point
        if (points.Length > 0)
            agent.SetDestination(points[0].position);

        Debug.Log("🤖 Agent spawn | HP = " + health);
    }

    // ===================== PATROL =====================
    void Update()
    {
        if (isDead) return;
        if (points.Length == 0) return;

        // Si arrivé au point actuel
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentIndex++;

            // boucle des points
            if (currentIndex >= points.Length)
                currentIndex = 0;

            agent.SetDestination(points[currentIndex].position);
        }
    }

    // ===================== DAMAGE =====================
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        Debug.Log("⚔️ Agent touché | dégâts = " + damage);

        health -= damage;

        Debug.Log("❤️ HP restant = " + health);

        // si mort
        if (health <= 0)
        {
            Die();
        }
    }

    // ===================== DEATH =====================
    private void Die()
    {
        if (isDead) return;

        isDead = true;

        Debug.Log("💀 Agent mort");

        // stop mouvement
        agent.ResetPath();
        agent.isStopped = true;

        // animation mort
        if (animator != null)
            animator.SetTrigger("Die");

        // désactive collider pour éviter encore des hits
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        // détruit après animation
        Destroy(gameObject, 3f);
    }

    // ===================== HIT PLAYER =====================
    void OnTriggerEnter(Collider other)
    {
        // si touche joueur
        if (!other.CompareTag("Player")) return;

        Debug.Log("👤 Agent touche le joueur !");

        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

        if (playerHealth != null && !playerHealth.IsDead())
        {
            playerHealth.TakeDamage(10); // dégâts au joueur
        }
    }
}