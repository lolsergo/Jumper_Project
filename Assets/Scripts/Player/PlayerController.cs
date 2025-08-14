using UnityEngine;
using Zenject;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GravityConfig GravityConfig;

    private float minJumpForce;
    private float maxJumpForce;
    private float maxJumpTime;
    private float fastFallGravity;
    private float jumpGravity;
    private float normalGravity;
    private float floatFallSpeed;

    [Header("Ground Check")]
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private LayerMask groundLayer;
    private readonly float groundCheckDistance = 0.5f;
    private readonly float groundCheckOffset = 0.1f;

    private Rigidbody2D rb;
    private InputController _inputController;
    private bool isGrounded;
    private bool isJumping;
    private bool isHoldingJump;
    private float jumpTimer;
    private float currentJumpForce;

    [Inject]
    private void Construct(InputController inputController)
    {
        _inputController = inputController;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        InitializeGravity();
        rb.gravityScale = normalGravity;
    }

    private void OnEnable()
    {
        if (_inputController != null)
        {
            // Находим действие "Jump" в списке и подписываемся
            var jumpAction = _inputController.actions.Find(a => a.name == "Jump");
            if (jumpAction != null)
            {
                jumpAction.OnPressed += StartJump;
                jumpAction.OnReleased += EndJump;
                jumpAction.OnHolding += SetJumpHolding;
            }
        }
    }

    private void OnDisable()
    {
        if (_inputController != null)
        {
            var jumpAction = _inputController.actions.Find(a => a.name == "Jump");
            if (jumpAction != null)
            {
                jumpAction.OnPressed -= StartJump;
                jumpAction.OnReleased -= EndJump;
                jumpAction.OnHolding -= SetJumpHolding;
            }
        }
    }

    private void Update()
    {
        CheckGrounded();
        HandleJump();
        HandleGravity();
    }

    private void CheckGrounded()
    {
        Vector2 rayStart = transform.position + new Vector3(0, -groundCheckOffset, 0);
        RaycastHit2D hit = Physics2D.Raycast(
            rayStart,
            Vector2.down,
            groundCheckDistance,
            groundLayer
        );

        isGrounded = hit.collider != null;

        if (isGrounded && !isJumping)
        {
            rb.gravityScale = normalGravity;
        }

        Debug.DrawRay(rayStart, Vector2.down * groundCheckDistance, isGrounded ? Color.green : Color.red);
    }

    private void SetJumpHolding()
    {
        isHoldingJump = true;
    }

    private void StartJump()
    {
        if (isGrounded)
        {
            isJumping = true;
            isHoldingJump = true;
            jumpTimer = 0f;
            currentJumpForce = minJumpForce;
            rb.gravityScale = jumpGravity;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, currentJumpForce);
        }
    }

    private void HandleJump()
    {
        if (isJumping)
        {
            jumpTimer += Time.deltaTime;

            float jumpProgress = Mathf.Clamp01(jumpTimer / maxJumpTime);
            currentJumpForce = Mathf.Lerp(minJumpForce, maxJumpForce, jumpProgress);

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, currentJumpForce);

            if (jumpTimer >= maxJumpTime)
            {
                EndJump();
            }
        }
    }

    private void EndJump()
    {
        isJumping = false;
        isHoldingJump = false;

        if (!isGrounded)
        {
            rb.gravityScale = fastFallGravity;
        }
    }

    private void HandleGravity()
    {
        if (!isJumping && !isGrounded && rb.linearVelocity.y < 0)
        {
            // Плавное падение при удержании кнопки
            if (isHoldingJump)
            {
                rb.gravityScale = jumpGravity;
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -floatFallSpeed));
            }
            else // Быстрое падение
            {
                rb.gravityScale = fastFallGravity;
            }
        }
    }

    private void InitializeGravity()
    {
        minJumpForce = GravityConfig.MinJumpForce;
        maxJumpForce = GravityConfig.MaxJumpForce;
        maxJumpTime = GravityConfig.MaxJumpTime;
        fastFallGravity = GravityConfig.FastFallGravity;
        jumpGravity = GravityConfig.JumpGravity;
        normalGravity = GravityConfig.NormalGravity;
        floatFallSpeed = GravityConfig.FloatFallSpeed;
    }
}