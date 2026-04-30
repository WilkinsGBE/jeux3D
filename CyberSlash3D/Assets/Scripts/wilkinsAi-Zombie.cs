using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class WilkinsAIZombie : MonoBehaviour
{
    [Header("Detection")]
    public float detectionRadius = 8f;
    public float loseRadius = 12f;

    private NavMeshAgent agent;
    private Animator animator;
    private Transform player;
    private bool isChasing = false;

    void Start()
    {
        agent    = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;

        SetAnim(false);
        agent.isStopped = true;
    }

    void Update()
    {
        if (player == null || agent == null || !agent.isOnNavMesh) return;

        float dist = Vector3.Distance(transform.position, player.position);

        if (!isChasing && dist <= detectionRadius)
        {
            isChasing = true;
            agent.isStopped = false;
            SetAnim(true);
        }
        else if (isChasing && dist > loseRadius)
        {
            isChasing = false;
            agent.isStopped = true;
            agent.ResetPath();
            SetAnim(false);
        }

        if (isChasing)
            agent.SetDestination(player.position);
    }

    void SetAnim(bool walking)
    {
        if (animator == null) return;
        animator.SetBool("Walk", walking);
        animator.SetBool("Stand", !walking);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, loseRadius);
    }
}
