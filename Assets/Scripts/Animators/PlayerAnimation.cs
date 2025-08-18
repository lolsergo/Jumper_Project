using UnityEngine;
using Zenject;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField]
    private PlayerController _player;
    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    private Animator _animator;
    private readonly float groundCheckOffset = 0.1f;
    private GameSpeedManager _speedManager;

    [Inject]
    private void Construct(GameSpeedManager speedManager)
    {
        _speedManager = speedManager;
    }

    [Header("Настройки")]
    [SerializeField]
    private float _landingThreshold = 0.5f; // Дистанция для начала анимации приземления
    [SerializeField]
    private float _startRunSpeed = 7f;
    private bool _groundDistance;

    private void Update()
    {
        _groundDistance = _player.IsGroundNear(_landingThreshold, groundCheckOffset);
        _animator.SetBool("IsGroundNear", _groundDistance);
        _animator.SetBool("IsRuning", IsRuning());
        _animator.SetFloat("VerticalSpeed", _rb.linearVelocity.y);
    }

    private bool IsRuning()
    {
        return _speedManager.GameSpeed >= _startRunSpeed;
    }
}
