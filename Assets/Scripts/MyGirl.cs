//using UnityEngine;

//[RequireComponent(typeof(CharacterController))]
//public class ZombieController : MonoBehaviour
//{
//    public float speed = 5f;
//    public float jumpHeight = 2f;
//    public float gravity = -9.81f;
//    public Animator animator;
//    public Transform cameraTransform;

//    private CharacterController controller;
//    private Vector3 velocity;
//    private bool isGrounded;

//    void Start()
//    {
//        controller = GetComponent<CharacterController>();
//    }

//    void Update()
//    {
//        // Horizontal movement input
//        float x = Input.GetAxis("Horizontal");
//        float z = Input.GetAxis("Vertical");

//        // Camera-relative directions
//        Vector3 camForward = cameraTransform.forward;
//        Vector3 camRight = cameraTransform.right;

//        camForward.y = 0;
//        camRight.y = 0;
//        camForward.Normalize();
//        camRight.Normalize();

//        Vector3 move = camRight * x + camForward * z;

//        // Rotate zombie toward movement direction
//        if (move != Vector3.zero)
//        {
//            Quaternion toRotation = Quaternion.LookRotation(move, Vector3.up);
//            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 10f * Time.deltaTime);
//        }

//        // Check if grounded
//        isGrounded = controller.isGrounded;
//        if (isGrounded && velocity.y < 0)
//        {
//            velocity.y = -2f; // small downward force to keep grounded
//        }

//        // Jumping
//        if (Input.GetButtonDown("Jump") && isGrounded)
//        {
//            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
//            animator.SetTrigger("Jump");
//        }

//        // Apply gravity
//        velocity.y += gravity * Time.deltaTime;

//        // Combine horizontal and vertical movement
//        Vector3 finalMove = move * speed + new Vector3(0, velocity.y, 0);

//        // Move the controller once per frame
//        controller.Move(finalMove * Time.deltaTime);

//        // Animate based on horizontal speed only
//        animator.SetFloat("Speed", move.magnitude);
//    }
//}

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ZombieControllerRigidbody : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 5f;
    public Animator animator;
    public Transform cameraTransform;

    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Get input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Camera-relative movement direction
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 move = camRight * x + camForward * z;
        move.Normalize();

        // Rotate towards movement direction
        if (move != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(move, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 10f * Time.deltaTime);
        }

        // Move by setting velocity (only horizontal)
        Vector3 velocity = move * speed;
        velocity.y = rb.linearVelocity.y; // keep existing vertical velocity
        rb.linearVelocity = velocity;

        // Ground check (simple way)
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            animator.SetTrigger("Jump");
        }

        // Animate based on horizontal speed
        Vector3 horizontalVelocity = rb.linearVelocity;
        horizontalVelocity.y = 0;
        animator.SetFloat("Speed", horizontalVelocity.magnitude);
    }
}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class SimpleKeyInputJump : MonoBehaviour
//{
//    public Animator characterAnimator;

//    void Start()
//    {
//        characterAnimator = GetComponent<Animator>();
//    }

//    void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.Space))
//        {
//            characterAnimator.SetTrigger("JumpT");
//        }
//        if (Input.GetKeyDown(KeyCode.W))
//        {
//            characterAnimator.SetTrigger("Walk");
//        }
//    }
//}
