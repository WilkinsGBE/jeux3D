using UnityEngine;
using UnityEngine.AI;

public class BossChase : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent agent;
    public Animator animator;

    public float stopDistance = 3f;

    private bool isChasing = false;

    void Start()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        if (animator == null)
            animator = GetComponent<Animator>();

        agent.stoppingDistance = stopDistance;
    }

    void Update()
    {
        if (!isChasing || player == null)
        {
            agent.isStopped = true;
            return;
        }

        agent.isStopped = false;
        agent.SetDestination(player.position);

        if (animator != null)
        {
            animator.SetBool("isWalking", agent.velocity.magnitude > 0.1f);
        }
    }

    public void StartChasing()
    {
        isChasing = true;
    }

    public void StopChasing()
    {
        isChasing = false;

        if (animator != null)
            animator.SetBool("isWalking", false);
    }
}