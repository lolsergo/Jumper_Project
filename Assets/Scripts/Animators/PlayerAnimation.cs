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
    private GameSpeedManager _speedManager;
    private bool _isGroundNear;

    [Inject]
    private void Construct(GameSpeedManager speedManager)
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
        _animator.SetBool("IsGroundNear", _isGroundNear);
        _animator.SetBool("IsRunning", IsRunning);
        _animator.SetFloat("VerticalSpeed", _rb.linearVelocity.y);
    }

    private bool IsRunning => _speedManager != null && _speedManager.GameSpeed >= _startRunSpeed;
}