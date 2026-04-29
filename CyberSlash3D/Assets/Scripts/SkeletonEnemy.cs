using UnityEngine;
using UnityEngine.AI;

public class SkeletonAI : MonoBehaviour
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
            animator = GetComponentInChildren<Animator>();

        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
                player = playerObject.transform;
        }

        agent.stoppingDistance = stopDistance;
        agent.isStopped = true;
    }

    void Update()
    {
        if (!isChasing || player == null)
        {
            StopMoving();
            return;
        }

        SkeletonAttack attack = GetComponent<SkeletonAttack>();
        if (attack != null && attack.IsAttacking)
        {
            StopMoving();
            FacePlayer();
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= stopDistance)
        {
            StopMoving();
            FacePlayer();
            return;
        }

        agent.isStopped = false;
        agent.SetDestination(player.position);

        if (animator != null)
            animator.SetBool("isRunning", agent.velocity.magnitude > 0.1f);
    }

    void StopMoving()
    {
        if (agent != null)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }

        if (animator != null)
            animator.SetBool("isRunning", false);
    }

    void FacePlayer()
    {
        if (player == null) return;

        Vector3 dir = player.position - transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.01f)
            return;

        transform.rotation = Quaternion.LookRotation(dir);
    }

    public void StartChasing()
    {
        isChasing = true;
    }

    public void StopChasing()
    {
        isChasing = false;
        StopMoving();
    }
}