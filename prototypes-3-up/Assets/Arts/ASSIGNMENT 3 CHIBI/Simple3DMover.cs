using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Simple3DMover : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed = 4.5f;
    public float rotationSpeed = 12f;
    public bool cameraRelative = false;

    [Header("Gravity")]
    public float gravity = -9.81f;
    public float groundedGravity = -2f;

    [Header("Animation")]
    public Animator animator;
    public string speedParam = "Speed";
    public string walkingBool = "IsWalking";

    [Header("Optional: Direct Binding (无 PlayerInput 也可)")]
    public InputActionReference moveActionRef; // 可空；若赋值则自动订阅

    private CharacterController controller;
    private Vector2 moveInput;
    private float verticalVelocity;
    private InputAction _moveAction; // 缓存

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (!animator) animator = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        // 兼容：若填了 moveActionRef，则直接启用并订阅（不依赖 PlayerInput）
        if (moveActionRef != null && moveActionRef.action != null)
        {
            _moveAction = moveActionRef.action;
            _moveAction.Enable();
            _moveAction.performed += OnMoveCtx;
            _moveAction.canceled  += OnMoveCtx;
        }
    }

    private void OnDisable()
    {
        if (_moveAction != null)
        {
            _moveAction.performed -= OnMoveCtx;
            _moveAction.canceled  -= OnMoveCtx;
            _moveAction.Disable();
            _moveAction = null;
        }
    }

    // ====== 兼容三种回调 ======
    // 1) Invoke Unity Events / C# 代码订阅
    public void OnMove(InputAction.CallbackContext ctx) => OnMoveCtx(ctx);

    // 2) Send Messages（方法名必须与 Action 名 + "On" 规则匹配，这里也兜底支持）
    public void OnMove(InputValue v) // 需要 using UnityEngine.InputSystem;
    {
        moveInput = v.Get<Vector2>();
    }

    // 实际统一处理
    private void OnMoveCtx(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }
    // ==========================

    private void Update()
    {
        // 水平方向
        Vector3 dir = new Vector3(moveInput.x, 0f, moveInput.y);
        if (dir.sqrMagnitude > 1f) dir.Normalize();

        if (cameraRelative && Camera.main)
        {
            Vector3 camF = Camera.main.transform.forward; camF.y = 0f; camF.Normalize();
            Vector3 camR = Camera.main.transform.right;   camR.y = 0f; camR.Normalize();
            dir = camF * moveInput.y + camR * moveInput.x;
            if (dir.sqrMagnitude > 1f) dir.Normalize();
        }

        // 朝向
        if (dir.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }

        // 重力
        if (controller.isGrounded && verticalVelocity < 0f) verticalVelocity = groundedGravity;
        else verticalVelocity += gravity * Time.deltaTime;

        // 位移
        Vector3 velocity = dir * moveSpeed;
        Vector3 move = velocity + Vector3.up * verticalVelocity;
        controller.Move(move * Time.deltaTime);

        // 动画
        float horizontalSpeed = new Vector2(velocity.x, velocity.z).magnitude;
        if (animator)
        {
            animator.SetFloat(speedParam, horizontalSpeed);
            if (!string.IsNullOrEmpty(walkingBool))
                animator.SetBool(walkingBool, horizontalSpeed > 0.1f);
        }
    }
}
