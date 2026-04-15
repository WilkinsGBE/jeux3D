using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 180f;
    public float gravity = -20f;
    public float jumpHeight = 1.5f;

    private CharacterController controller;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float moveInput = Input.GetAxis("Vertical");     // W / S
        float turnInput = Input.GetAxis("Horizontal");   // A / D

        // Rotate character
        transform.Rotate(0f, turnInput * turnSpeed * Time.deltaTime, 0f);

        // Move forward/backward
        Vector3 move = transform.forward * moveInput;
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Ground check
        //if (controller.isgrounded && velocity.y < 0f)
        //{
        //    velocity.y = -2f;
        //}

        // Jump
        if (Input.GetButtonDown("Jump")) //&& controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}