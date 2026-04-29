using UnityEngine;
using UnityEngine.AI;

public class ControlleurAgent : MonoBehaviour
{
    [Header("Patrol")]
    public Transform[] points;
    private int currentIndex = 0;

    [Header("Components")]
    private NavMeshAgent agent;
    public Animator animator;

    [Header("Stats")]
    public int health = 3;
    private bool isDead = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (agent == null)
        {
            Debug.LogError("No NavMeshAgent found on " + gameObject.name);
            enabled = false;
            return;
        }

        if (!agent.isOnNavMesh)
        {
            Debug.LogWarning(gameObject.name + " is not on a NavMesh.");
            return;
        }

        if (points != null && points.Length > 0)
            agent.SetDestination(points[0].position);

        Debug.Log("Agent spawn | HP = " + health);
    }

    void Update()
    {
        if (isDead) return;
        if (agent == null || !agent.enabled || !agent.isOnNavMesh) return;
        if (points == null || points.Length == 0) return;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentIndex++;

            if (currentIndex >= points.Length)
                currentIndex = 0;

            agent.SetDestination(points[currentIndex].position);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        health -= damage;
        Debug.Log("Agent hit | HP = " + health);

        if (health <= 0)
            Die();
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;

        if (agent != null && agent.enabled && agent.isOnNavMesh)
        {
            agent.ResetPath();
            agent.isStopped = true;
        }

        if (animator != null)
            animator.SetTrigger("Die");

        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        Destroy(gameObject, 3f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

        if (playerHealth != null && !playerHealth.IsDead())
        {
            playerHealth.TakeDamage(10);
        }
    }
}