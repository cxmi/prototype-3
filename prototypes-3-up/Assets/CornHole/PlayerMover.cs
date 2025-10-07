using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviour
{
   private CharacterController controller;

    [Header("Input")]
    public InputActionReference moveActionRef; // Vector2 input from new input system

    [Header("Movement")]
    public float moveSpeed = 4.5f;
    public float rotationSpeed = 12f;

    [Header("Gravity")]
    public float gravity = -9.81f;
    public float groundedGravity = -2f;

    [Header("Animation")]
    public Animator animator;
    public string speedParam = "Speed";
    public string walkingBool = "IsWalking";

    private float _rotationVelocity;
    private Vector3 _verticalVelocity; // Holds vertical movement (gravity)

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (!animator) animator = GetComponentInChildren<Animator>();

        if (moveActionRef != null && !moveActionRef.action.enabled)
        {
            moveActionRef.action.Enable();
        }
    }

    void Update()
    {
        Vector2 input = moveActionRef.action.ReadValue<Vector2>();
        Vector3 inputDir = new Vector3(input.x, 0f, input.y).normalized;

        // Gravity
        if (controller.isGrounded)
        {
            _verticalVelocity.y = groundedGravity;
        }
        else
        {
            _verticalVelocity.y += gravity * Time.deltaTime;
        }

        // Movement + rotation
        Vector3 move = Vector3.zero;

        if (inputDir.magnitude >= 0.1f)
        {
            // Rotation
            float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _rotationVelocity, 1f / rotationSpeed);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Move direction in world space
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            move = moveDir * moveSpeed;

            // Animation
            if (animator)
            {
                animator.SetBool(walkingBool, true);
                animator.SetFloat(speedParam, moveDir.magnitude);
            }
        }
        else
        {
            if (animator)
            {
                animator.SetBool(walkingBool, false);
                animator.SetFloat(speedParam, 0f);
            }
        }

        // Apply gravity
        move += _verticalVelocity;

        // Move character
        controller.Move(move * Time.deltaTime);
    }
}