using UnityEngine;
using UnityEngine.AI;

public class SkeletonAttackPatrol : MonoBehaviour
{
    public Transform player;

    public float attackRange = 2.5f;
    public float attackAngle = 70f;
    public float attackCooldown = 1.5f;
    public int damage = 10;

    public float rotationSpeed = 6f;

    public Animator animator;
    public NavMeshAgent agent;

    private float nextAttackTime;
    private bool isAttacking = false;
    public bool IsAttacking => isAttacking;

    void Start()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        if (isAttacking)
        {
            FacePlayer();
            return;
        }

        Vector3 origin = transform.position + Vector3.up * 1.2f;
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

        if (agent != null && agent.enabled && agent.isOnNavMesh)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }

        if (animator != null)
            animator.SetTrigger("Attack");

        Invoke(nameof(EndAttack), 1.0f);
    }

    public void EndAttack()
    {
        CancelInvoke(nameof(EndAttack));

        isAttacking = false;

        if (agent != null && agent.enabled && agent.isOnNavMesh)
            agent.isStopped = false;

        if (animator != null)
            animator.ResetTrigger("Attack");
    }

    public void DealDamage()
    {
        Collider[] hits = Physics.OverlapSphere(
            transform.position + transform.forward * 1.5f,
            1.2f
        );

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player") || hit.transform.root.CompareTag("Player"))
            {
                IDamageable damageable = hit.GetComponentInParent<IDamageable>();

                if (damageable != null)
                    damageable.TakeDamage(damage);
            }
        }
    }
}