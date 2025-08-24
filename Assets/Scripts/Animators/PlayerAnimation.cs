using UnityEngine;
using Zenject;

public class PlayerAnimator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController _player;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Animator _animator;

    [Header("Settings")]
    [SerializeField] private float _landingThreshold = 0.5f;
    [SerializeField] private float _startRunSpeed = 7f;

    private const float GroundCheckOffset = 0.1f;
    private GameSpeedProvider _speedManager;
    private bool _isGroundNear;

    // Animator parameter hashes
    private static readonly int IsGroundNearHash = Animator.StringToHash("IsGroundNear");
    private static readonly int IsRunningHash = Animator.StringToHash("IsRunning");
    private static readonly int VerticalSpeedHash = Animator.StringToHash("VerticalSpeed");

    [Inject]
    private void Construct(GameSpeedProvider speedManager)
    {
        _speedManager = speedManager;
    }

    private void Awake()
    {
        if (_player == null) Debug.LogError("PlayerController reference not set.", this);
        if (_rb == null) Debug.LogError("Rigidbody2D reference not set.", this);
        if (_animator == null) Debug.LogError("Animator reference not set.", this);
    }

    private void Update()
    {
        _isGroundNear = _player.IsGroundNear(_landingThreshold, GroundCheckOffset);
        _animator.SetBool(IsGroundNearHash, _isGroundNear);
        _animator.SetBool(IsRunningHash, IsRunning);
        _animator.SetFloat(VerticalSpeedHash, _rb.linearVelocity.y);
    }

    private bool IsRunning => _speedManager != null && _speedManager.GameSpeed >= _startRunSpeed;
}