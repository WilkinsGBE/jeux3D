using UnityEngine;
using System.Collections;

public class PlayerComboAttack : MonoBehaviour
{
    public Animator animator;

    [Header("Combo Settings")]
    public float comboWindow = 0.6f;

    [Header("Movement")]
    public PlayerMovement movementScript;

    [Header("Movement Unlock")]
    public float movementUnlockDelay = 0.2f;

    private int comboStep = 0;
    private bool isAttacking = false;
    private bool queuedNextAttack = false;
    private float lastClickTime;

    private Coroutine movementUnlockCoroutine;

    void Awake()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (movementScript == null)
            movementScript = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastClickTime = Time.time;

            if (!isAttacking)
                StartAttack(1);
            else
                queuedNextAttack = true;
        }
    }

    void StartAttack(int attackNumber)
    {
        if (movementUnlockCoroutine != null)
        {
            StopCoroutine(movementUnlockCoroutine);
            movementUnlockCoroutine = null;
        }

        isAttacking = true;
        comboStep = attackNumber;

        if (movementScript != null)
            movementScript.SetRootMotion(true);

        animator.SetInteger("ComboIndex", comboStep);
        animator.SetTrigger("Attack");
    }

    public void EnableNextAttack()
    {
        CheckCombo();
    }

    public void CheckCombo()
    {
        if (queuedNextAttack && Time.time - lastClickTime <= comboWindow)
        {
            queuedNextAttack = false;
            comboStep = 2;

            if (movementUnlockCoroutine != null)
            {
                StopCoroutine(movementUnlockCoroutine);
                movementUnlockCoroutine = null;
            }

            if (movementScript != null)
                movementScript.SetRootMotion(true);

            animator.SetInteger("ComboIndex", 2);
        }
    }

    public void EndAttack()
    {
        ResetCombo();
    }

    public void EnableMovementDelayed()
    {
        if (movementUnlockCoroutine != null)
            StopCoroutine(movementUnlockCoroutine);

        movementUnlockCoroutine = StartCoroutine(EnableMovementAfterDelay());
    }

    private IEnumerator EnableMovementAfterDelay()
    {
        yield return new WaitForSeconds(movementUnlockDelay);

        if (!isAttacking && movementScript != null)
            movementScript.SetRootMotion(false);

        movementUnlockCoroutine = null;
    }

    void ResetCombo()
    {
        isAttacking = false;
        queuedNextAttack = false;
        comboStep = 0;

        animator.SetInteger("ComboIndex", 0);
    }

    public void EnableMovement()
    {
        if (movementUnlockCoroutine != null)
        {
            StopCoroutine(movementUnlockCoroutine);
            movementUnlockCoroutine = null;
        }

        if (movementScript != null)
            movementScript.SetRootMotion(false);
    }
}