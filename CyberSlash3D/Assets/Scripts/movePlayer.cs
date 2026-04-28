using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float rotationSmoothTime = 0.12f;
    public float gravity = -20f;
    public float jumpHeight = 2.5f;

    public Transform cameraTransform;
    public LayerMask groundMask;

    public float groundCheckDistance = 1.0f;
    public float maxGroundAngle = 65f;
    public float groundedGraceTime = 0.2f;
    public float groundSnapForce = -8f;

    public float airSteerSpeed = 4f;

    private CharacterController controller;
    private Animator animator;

    private Vector3 horizontalVelocity;
    private Vector3 jumpHorizontalVelocity;
    private float verticalVelocity;

    private float turnSmoothVelocity;
    private float groundedTimer;

    private bool isGrounded;
    private bool isJumping;
    private bool useRootMotion = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        if (animator != null)
            animator.applyRootMotion = false;
    }

    void Update()
    {
        isGrounded = CheckGrounded();

        if (isGrounded && !isJumping)
            groundedTimer = groundedGraceTime;
        else
            groundedTimer -= Time.deltaTime;

        bool recentlyGrounded = groundedTimer > 0f;

        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        Vector3 inputDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        if (!useRootMotion)
        {
            if (!isJumping)
                HandleGroundMovement(inputDirection);
            else
                HandleAirMovement(inputDirection);
        }
        else
        {
            horizontalVelocity = Vector3.zero;

            if (!isJumping)
                jumpHorizontalVelocity = Vector3.zero;
        }

        if (!useRootMotion && Input.GetButtonDown("Jump") && recentlyGrounded && !isJumping)
        {
            jumpHorizontalVelocity = horizontalVelocity;
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

            isJumping = true;
            groundedTimer = 0f;

            if (animator != null)
                animator.SetTrigger("Jump");
        }

        if (recentlyGrounded && !isJumping && verticalVelocity < 0f)
            verticalVelocity = groundSnapForce;

        verticalVelocity += gravity * Time.deltaTime;

        if (!useRootMotion)
        {
            Vector3 finalHorizontalVelocity = isJumping ? jumpHorizontalVelocity : horizontalVelocity;

            Vector3 finalVelocity = new Vector3(
                finalHorizontalVelocity.x,
                verticalVelocity,
                finalHorizontalVelocity.z
            );

            controller.Move(finalVelocity * Time.deltaTime);
        }

        if (controller.isGrounded && isJumping && verticalVelocity <= 0f)
        {
            isJumping = false;
            verticalVelocity = groundSnapForce;
            jumpHorizontalVelocity = Vector3.zero;
        }

        bool isRunning = inputDirection.magnitude >= 0.1f && !isJumping && !useRootMotion;
        bool isJumpingAnim = isJumping || !recentlyGrounded;

        if (animator != null)
        {
            animator.SetBool("isRunning", isRunning);
            animator.SetBool("isJumping", isJumpingAnim);
        }
    }

    public void SetRootMotion(bool value)
    {
        useRootMotion = value;

        if (animator != null)
            animator.applyRootMotion = value;

        horizontalVelocity = Vector3.zero;

        if (!isJumping)
            jumpHorizontalVelocity = Vector3.zero;
    }

    void OnAnimatorMove()
    {
        if (!useRootMotion || animator == null) return;

        Vector3 motion = animator.deltaPosition;

        if (isJumping)
            motion += jumpHorizontalVelocity * Time.deltaTime;

        motion.y = verticalVelocity * Time.deltaTime;

        controller.Move(motion);
    }

    void HandleGroundMovement(Vector3 inputDirection)
    {
        if (inputDirection.magnitude >= 0.1f)
        {
            float targetAngle =
                Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg
                + cameraTransform.eulerAngles.y;

            RotatePlayer(targetAngle);

            Vector3 moveDirection =
                Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            horizontalVelocity = moveDirection.normalized * moveSpeed;
        }
        else
        {
            horizontalVelocity = Vector3.zero;
        }
    }

    void HandleAirMovement(Vector3 inputDirection)
    {
        if (inputDirection.magnitude < 0.1f)
            return;

        float currentSpeed = jumpHorizontalVelocity.magnitude;

        float targetAngle =
            Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg
            + cameraTransform.eulerAngles.y;

        RotatePlayer(targetAngle);

        Vector3 targetDirection =
            Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

        Vector3 currentDirection = jumpHorizontalVelocity.normalized;

        Vector3 newDirection = Vector3.RotateTowards(
            currentDirection,
            targetDirection.normalized,
            airSteerSpeed * Time.deltaTime,
            0f
        );

        jumpHorizontalVelocity = newDirection.normalized * currentSpeed;
    }

    void RotatePlayer(float targetAngle)
    {
        float angle = Mathf.SmoothDampAngle(
            transform.eulerAngles.y,
            targetAngle,
            ref turnSmoothVelocity,
            rotationSmoothTime
        );

        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    bool CheckGrounded()
    {
        Vector3 bottom = transform.position + controller.center;
        bottom.y -= controller.height / 2f;
        bottom.y += controller.radius + 0.05f;

        bool hitGround = Physics.SphereCast(
            bottom,
            controller.radius * 0.85f,
            Vector3.down,
            out RaycastHit hit,
            groundCheckDistance,
            groundMask,
            QueryTriggerInteraction.Ignore
        );

        if (!hitGround)
            return controller.isGrounded;

        float groundAngle = Vector3.Angle(hit.normal, Vector3.up);

        return groundAngle <= maxGroundAngle || controller.isGrounded;
    }

    void OnDrawGizmosSelected()
    {
        CharacterController cc = GetComponent<CharacterController>();
        if (cc == null) return;

        Vector3 bottom = transform.position + cc.center;
        bottom.y -= cc.height / 2f;
        bottom.y += cc.radius + 0.05f;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(bottom, cc.radius * 0.85f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(bottom, bottom + Vector3.down * groundCheckDistance);
    }
}