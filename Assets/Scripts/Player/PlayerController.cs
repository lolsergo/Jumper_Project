using UnityEngine;
using Zenject;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GravityConfig GravityConfig;
    [SerializeField] private LayerMask groundLayer;

    private readonly float _groundCheckDistance = 0.5f;
    private readonly float _groundCheckOffset = 0.1f;

    private Rigidbody2D _rb;
    private InputController _inputController;
    private readonly InputActionState _jumpState = new();

    private GravitySystem _gravitySystem;
    private GroundChecker _groundChecker;
    public GroundChecker GroundChecker => _groundChecker;
    private JumpSystem _jumpSystem;

    private bool _isGrounded;
    private System.Action _jumpPressedHandler;
    private System.Action _jumpReleasedHandler;

    [Inject]
    private void Construct(InputController inputController)
    {
        _inputController = inputController;
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        _gravitySystem = new GravitySystem(_rb, GravityConfig);
        _groundChecker = new GroundChecker(transform, groundLayer);
        _jumpSystem = new JumpSystem(_rb, _gravitySystem, GravityConfig);

        InitializeInputHandlers();
    }

    private void OnEnable() => EnableInputHandlers();
    private void OnDisable() => DisableInputHandlers();

    private void Update()
    {
        _isGrounded = _groundChecker.IsGroundNear(_groundCheckDistance, _groundCheckOffset);
        CacheInput();
        _jumpState.ResetFrameStates();
    }

    private void FixedUpdate()
    {
        HandleGravity();
        _jumpSystem.HandleJump(_jumpState.Holding);
    }

    private void InitializeInputHandlers()
    {
        _jumpPressedHandler = () =>
        {
            _jumpState.Pressed = true;
            _jumpState.Holding = true;
        };

        _jumpReleasedHandler = () =>
        {
            _jumpState.Released = true;
            _jumpState.Holding = false;
        };
    }

    private void CacheInput()
    {
        if (_jumpState.Pressed && _isGrounded)
            _jumpSystem.StartJump();

        if (_jumpState.Released && _jumpSystem.IsJumping)
            _jumpSystem.EndJump();
    }
    public bool IsGroundNear(float distance, float checkOffset)
        => _groundChecker.IsGroundNear(distance, checkOffset);


    private void HandleGravity()
    {
        if (_isGrounded)
        {
            _gravitySystem.ApplyNormalGravity();
            return;
        }

        if (_jumpState.Holding && _rb.linearVelocity.y < 0)
            _gravitySystem.ApplyFloatFall();
        else if (_rb.linearVelocity.y < 0)
            _gravitySystem.ApplyFastFallGravity();
    }

    private void EnableInputHandlers() =>
        EnableInputAction(InputController.InputActionType.Jump, _jumpPressedHandler, _jumpReleasedHandler);

    private void DisableInputHandlers() =>
        DisableInputAction(InputController.InputActionType.Jump, _jumpPressedHandler, _jumpReleasedHandler);

    private void EnableInputAction(InputController.InputActionType actionType, System.Action pressed, System.Action released)
    {
        if (_inputController == null) return;
        var action = _inputController.GetAction(actionType);
        if (action == null) return;

        action.OnPressed += pressed;
        action.OnReleased += released;
        action.Enable();
    }

    private void DisableInputAction(InputController.InputActionType actionType, System.Action pressed, System.Action released)
    {
        if (_inputController == null) return;
        var action = _inputController.GetAction(actionType);
        if (action == null) return;

        action.OnPressed -= pressed;
        action.OnReleased -= released;
        action.Disable();
    }
}