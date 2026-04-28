using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class WilkinsAIZombie : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private float detectionRadius = 8f;
    [SerializeField] private float losePlayerRadius = 12f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask obstacleLayer;

    [Header("Chase")]
    [SerializeField] private float chaseSpeed = 4.5f;

    private enum State { Idle, Chase }
    private State currentState = State.Idle;
    private State lastState = State.Idle;

    private NavMeshAgent agent;
    private Transform player;
    private Animator animator;

    private static readonly int AnimStand = Animator.StringToHash("Stand");
    private static readonly int AnimWalk  = Animator.StringToHash("Walk");

    void Start()
    {
        agent    = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        EnterIdle();
    }

    void Update()
    {
        if (player == null) return;

        switch (currentState)
        {
            case State.Idle:
                if (CanSeePlayer())
                    EnterChase();
                break;

            case State.Chase:
                agent.destination = player.position;
                float dist = Vector3.Distance(transform.position, player.position);
                if (dist > losePlayerRadius)
                    EnterIdle();
                break;
        }

        UpdateAnimations();
    }

    private void EnterIdle()
    {
        currentState      = State.Idle;
        agent.isStopped   = true;
        agent.velocity    = Vector3.zero;
    }

    private void EnterChase()
    {
        currentState      = State.Chase;
        agent.isStopped   = false;
        agent.speed       = chaseSpeed;
    }

    private bool CanSeePlayer()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > detectionRadius) return false;

        Vector3 dir = (player.position - transform.position).normalized;
        Vector3 eye = transform.position + Vector3.up * 1.5f;

        if (Physics.Raycast(eye, dir, out RaycastHit hit, detectionRadius, obstacleLayer | playerLayer))
            return hit.transform.CompareTag("Player");

        return false;
    }

    private void UpdateAnimations()
    {
        if (animator == null || currentState == lastState) return;

        if (currentState == State.Idle)
            animator.SetTrigger(AnimStand);
        else
            animator.SetTrigger(AnimWalk);

        lastState = currentState;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, losePlayerRadius);
    }
}
