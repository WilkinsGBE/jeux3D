using UnityEngine;
using UnityEngine.AI;

public class BossConeAttack : MonoBehaviour
{
    public Transform player;

    public float attackRange = 4f;
    public float attackAngle = 60f;
    public float attackCooldown = 2f;

    public float rotationSpeed = 6f; // control how fast boss turns

    public Animator animator;
    public NavMeshAgent agent;

    private float nextAttackTime;
    private bool isAttacking = false;

    void Start()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (agent == null)
            agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (player == null) return;

        // ALWAYS face player (even during attack)
        FacePlayer();

        // Stop movement logic during attack (but still rotate)
        if (isAttacking)
            return;

        // --- Detection ---
        Vector3 origin = transform.position + Vector3.up * 1.5f;
        Vector3 target = player.position + Vector3.up * 1.0f;

        Vector3 directionToPlayer = target - origin;
        float distance = directionToPlayer.magnitude;

        if (distance > attackRange)
            return;

        directionToPlayer.y = 0f;
        directionToPlayer.Normalize();

        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        if (angle <= attackAngle * 0.5f && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    void FacePlayer()
    {
        if (player == null) return;

        Vector3 dir = player.position - transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.01f)
            return;

        Quaternion targetRot = Quaternion.LookRotation(dir);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            rotationSpeed * Time.deltaTime
        );
    }

    void Attack()
    {
        if (isAttacking) return;

        isAttacking = true;

        // Stop movement
        if (agent != null)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }

        if (animator != null)
            animator.SetTrigger("Attack");

        Debug.Log("Boss cone attack!");

        // Backup reset (in case animation event fails)
        Invoke(nameof(EndAttack), 1.2f);
    }

    // Called via Animation Event at END of attack
    public void EndAttack()
    {
        CancelInvoke(nameof(EndAttack));

        isAttacking = false;

        if (agent != null)
            agent.isStopped = false;

        Debug.Log("Attack ended");
    }

    // Called via Animation Event at HIT frame
    public void DealDamage()
    {
        Debug.Log("Boss HIT!");

        Collider[] hits = Physics.OverlapSphere(
            transform.position + transform.forward * 2f,
            1.5f
        );

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                Debug.Log("Player takes damage");
                // Apply damage here
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector3 origin = transform.position + Vector3.up * 1.5f;

        Quaternion left = Quaternion.Euler(0, -attackAngle / 2f, 0);
        Quaternion right = Quaternion.Euler(0, attackAngle / 2f, 0);

        Gizmos.DrawRay(origin, transform.forward * attackRange);
        Gizmos.DrawRay(origin, left * transform.forward * attackRange);
        Gizmos.DrawRay(origin, right * transform.forward * attackRange);
    }
}