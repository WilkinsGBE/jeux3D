using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    public Animator animator;
    public PlayerMovement movementScript;

    [Header("Camera")]
    public Transform cameraTransform;

    [Header("Attack Steering")]
    public float attackTurnSpeed = 220f;

    [Header("Attack 2 Dash")]
    public float attack2DashDistance = 3f;
    public float attack2DashDuration = 0.15f;

    private bool isAttacking = false;

    void Awake()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (movementScript == null)
            movementScript = GetComponent<PlayerMovement>();

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        // While attacking → allow steering
        if (isAttacking)
        {
            SteerAttackTowardCamera();
            return;
        }

        // Inputs
        if (Input.GetMouseButtonDown(0))
            Attack1();

        if (Input.GetMouseButtonDown(1))
            Attack2();
    }

    void Attack1()
    {
        isAttacking = true;

        movementScript.SetRootMotion(true);
        FaceCameraForward();

        animator.ResetTrigger("Attack2");
        animator.SetTrigger("Attack1");
    }

    void Attack2()
    {
        isAttacking = true;

        movementScript.SetRootMotion(true);
        FaceCameraForward();

        animator.ResetTrigger("Attack1");
        animator.SetTrigger("Attack2");
    }

    // Called at the END of animations
    public void EnableMovement()
    {
        isAttacking = false;

        movementScript.SetRootMotion(false);

        animator.ResetTrigger("Attack1");
        animator.ResetTrigger("Attack2");
    }

    // Called by Animation Event (Attack 2)
    public void StartAttack2Dash()
    {
        StartCoroutine(Attack2Dash());
    }

    private IEnumerator Attack2Dash()
    {
        CharacterController controller = GetComponent<CharacterController>();

        float timer = 0f;
        Vector3 dashDirection = transform.forward;

        float speed = attack2DashDistance / attack2DashDuration;

        while (timer < attack2DashDuration)
        {
            controller.Move(dashDirection * speed * Time.deltaTime);

            timer += Time.deltaTime;
            yield return null;
        }
    }

    void FaceCameraForward()
    {
        if (cameraTransform == null) return;

        Vector3 lookDirection = cameraTransform.forward;
        lookDirection.y = 0f;

        if (lookDirection.sqrMagnitude < 0.01f) return;

        transform.rotation = Quaternion.LookRotation(lookDirection);
    }

    void SteerAttackTowardCamera()
    {
        if (cameraTransform == null) return;

        Vector3 lookDirection = cameraTransform.forward;
        lookDirection.y = 0f;

        if (lookDirection.sqrMagnitude < 0.01f) return;

        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            attackTurnSpeed * Time.deltaTime
        );
    }
}