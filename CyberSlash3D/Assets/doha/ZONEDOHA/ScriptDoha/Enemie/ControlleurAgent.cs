using UnityEngine;
using UnityEngine.AI;

public class ControlleurAgent : MonoBehaviour
{
    public Transform[] points;
    private int currentIndex = 0;

    private NavMeshAgent agent;
  public int health = 3;
    private bool isDead = false;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (points.Length > 0)
        {
            agent.SetDestination(points[0].position);
        }
    }

    void Update()
    {
         if (isDead) return;
        if (points.Length == 0) return;

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

        if (health <= 0)
        {
            Die();
        }
    }
     private void Die()
    {
        isDead = true;

        agent.ResetPath();
        agent.isStopped = true;

        Destroy(gameObject, 2f); // ou animation avant
    }
}
