using UnityEngine;
using UnityEngine.AI;

public class PatrolSkeletonAI : MonoBehaviour
{
    public Transform player;
    public Transform[] patrolPoints;

    public float pointReachDistance = 0.5f;

    private NavMeshAgent agent;
    private Animator animator;
    private int patrolIndex;
    private bool chasing;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.stoppingDistance = 1.5f;
        agent.isStopped = false;

        if (patrolPoints.Length > 0)
        {
            agent.SetDestination(patrolPoints[patrolIndex].position);
        }
    }

    void Update()
    {
        if (chasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }

        UpdateAnimation();
    }

    void UpdateAnimation()
    {
        bool shouldRun = agent.hasPath && agent.remainingDistance > agent.stoppingDistance + 0.2f;

        animator.SetBool("isPatrolRunning", shouldRun);
    }

    void ChasePlayer()
    {
        if (player == null) return;

        agent.isStopped = false;
        agent.SetDestination(player.position);
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        agent.isStopped = false;
        agent.SetDestination(patrolPoints[patrolIndex].position);

        if (Vector3.Distance(transform.position, patrolPoints[patrolIndex].position) <= pointReachDistance)
        {
            patrolIndex++;

            if (patrolIndex >= patrolPoints.Length)
            {
                patrolIndex = 0;
            }
        }
    }

    public void StartChasing()
    {
        chasing = true;
        agent.isStopped = false;

        if (player != null)
        {
            agent.SetDestination(player.position);
        }
    }

    public void StopChasing()
    {
        chasing = false;
        agent.isStopped = false;

        if (patrolPoints.Length > 0)
        {
            agent.SetDestination(patrolPoints[patrolIndex].position);
        }
    }
}