using UnityEngine;
using UnityEngine.AI;

public class WilkinsAIZombie : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("NavMesh")]
    public NavMeshAgent navMeshAgent;

    [Header("Animation")]
    public Animator animator;

    [Header("Stats")]
    public int health = 1;
    public int damage = 10;

    private bool isDead = false;
    private bool hasDamaged = false;

    const string STAND_STATE  = "Stand";
    const string WALK_STATE   = "Walk";
    const string ATTACK_STATE = "Attack";
    const string DEFEATED_STATE = "Defeated";

    public string currentAction;

    void Awake()
    {
        currentAction = STAND_STATE;
        animator      = GetComponentInChildren<Animator>();
        navMeshAgent  = GetComponent<NavMeshAgent>();
        FindPlayer();
    }

    void Update()
    {
        if (isDead) return;

        FindPlayer();
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= 2.5f)
        {
            navMeshAgent.isStopped = false;
            Vector3 dir     = (transform.position - player.position).normalized;
            Vector3 stopPos = player.position + dir * 1.8f;
            navMeshAgent.SetDestination(stopPos);
            ChangeState(ATTACK_STATE);
            Attacking();
            return;
        }

        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(player.position);
        ChangeState(WALK_STATE);
    }

    void Attacking()
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        if (state.IsName(ATTACK_STATE))
        {
            if (state.normalizedTime >= 0.5f && !hasDamaged)
            {
                PlayerHealth ph = player.GetComponent<PlayerHealth>();
                if (ph != null && !ph.IsDead())
                    ph.TakeDamage(damage);

                hasDamaged = true;
            }

            if (state.normalizedTime >= 0.95f)
                hasDamaged = false;
        }
    }

    public void TakeDamage(int dmg)
    {
        if (isDead) return;
        health -= dmg;
        if (health <= 0) Die();
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;
        ChangeState(DEFEATED_STATE);
        navMeshAgent.isStopped = true;
        navMeshAgent.ResetPath();
        Destroy(gameObject, 2f);
    }

    void ChangeState(string newState)
    {
        if (currentAction == newState) return;
        currentAction = newState;
        ResetAnimation();
        animator.SetBool(newState, true);
    }

    void ResetAnimation()
    {
        animator.SetBool(STAND_STATE,   false);
        animator.SetBool(WALK_STATE,    false);
        animator.SetBool(ATTACK_STATE,  false);
        animator.SetBool(DEFEATED_STATE, false);
    }

    void FindPlayer()
    {
        if (player != null) return;
        GameObject p = GameObject.FindWithTag("Player");
        if (p != null) player = p.transform;
    }
}
