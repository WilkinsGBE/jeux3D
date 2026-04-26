using System;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
   
    public GameObject player;

    NavMeshAgent navMeshAgent;
    Animator animator;

    const string STAND_STATE = "Stand";
    const string TAKE_DAMAGE_STATE = "Damage";
    public const string DEFEATED_STATE = "Defeated";
    const string WALK_STATE = "Walk";
    const string ATTACK_STATE = "Attack";

    public int health = 3;
    private bool isDead = false;

    public string currentAction;

    private void Awake()
    {
        currentAction = STAND_STATE;

        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (isDead) return;

        if (currentAction == DEFEATED_STATE)
        {
            navMeshAgent.ResetPath();
            return;
        }

        if (currentAction == TAKE_DAMAGE_STATE)
        {
            navMeshAgent.ResetPath();
            TakingDamage();
            return;
        }

        if (player == null) return;

        if (MovingToTarget())
        {
            return;
        }
        else
        {
            if (currentAction != ATTACK_STATE && currentAction != TAKE_DAMAGE_STATE)
            {
                Attack();
                return;
            }

            if (currentAction == ATTACK_STATE)
            {
                Attacking();
                return;
            }

            Stand();
        }
    }

    // ========================
    // STATES
    // ========================

    private void Stand()
    {
        ResetAnimation();
        currentAction = STAND_STATE;
        animator.SetBool(STAND_STATE, true);
    }

    private void Attack()
    {
        ResetAnimation();
        currentAction = ATTACK_STATE;
        animator.SetBool(ATTACK_STATE, true);
    }

    private void Walk()
    {
        ResetAnimation();
        currentAction = WALK_STATE;
        animator.SetBool(WALK_STATE, true);
    }

    public void TakeDamageAnimation()
    {
        ResetAnimation();
        currentAction = TAKE_DAMAGE_STATE;
        animator.SetBool(TAKE_DAMAGE_STATE, true);
    }

    public void Defeated()
    {
        ResetAnimation();
        currentAction = DEFEATED_STATE;
        animator.SetBool(DEFEATED_STATE, true);
    }

    // ========================
    // DAMAGE SYSTEM
    // ========================

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        health -= damage;

        TakeDamageAnimation();

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        ResetAnimation();
        currentAction = DEFEATED_STATE;

        animator.SetBool(DEFEATED_STATE, true);

        navMeshAgent.ResetPath();
        navMeshAgent.isStopped = true;

        Destroy(gameObject, 2f);
    }

    // ========================
    // ANIMATIONS CONTROL
    // ========================

    private void TakingDamage()
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        if (state.IsName(TAKE_DAMAGE_STATE))
        {
            if (state.normalizedTime >= 1f)
            {
                Stand();
            }
        }
    }

    private void Attacking()
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        if (state.IsName(ATTACK_STATE))
        {
            if (state.normalizedTime >= 1f)
            {
                Stand();
            }
        }
    }

    // ========================
    // MOVEMENT
    // ========================

    private bool MovingToTarget()
    {
        navMeshAgent.SetDestination(player.transform.position);

        if (navMeshAgent.pathPending)
            return true;

        if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
        {
            if (currentAction != WALK_STATE)
                Walk();
        }
        else
        {
            RotateToTarget(player.transform);
            return false;
        }

        return true;
    }

    private void RotateToTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 3f);
    }

    // ========================
    // UTIL
    // ========================

    private void ResetAnimation()
    {
        animator.SetBool(STAND_STATE, false);
        animator.SetBool(TAKE_DAMAGE_STATE, false);
        animator.SetBool(DEFEATED_STATE, false);
        animator.SetBool(WALK_STATE, false);
        animator.SetBool(ATTACK_STATE, false);
    }
}