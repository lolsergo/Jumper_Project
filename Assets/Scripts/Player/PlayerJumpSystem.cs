using UnityEngine;

public class JumpSystem
{
    private readonly Rigidbody2D _rb;
    private readonly GravitySystem _gravitySystem;
    private readonly float _minJumpForce;
    private readonly float _maxJumpForce;
    private readonly float _maxJumpTime;

    public bool IsJumping { get; private set; }
    private float _jumpTimer;
    private float _currentJumpForce;

    public JumpSystem(Rigidbody2D rb, GravitySystem gravitySystem, GravityConfig config)
    {
        _rb = rb;
        _gravitySystem = gravitySystem;
        _minJumpForce = config.MinJumpForce;
        _maxJumpForce = config.MaxJumpForce;
        _maxJumpTime = config.MaxJumpTime;
    }

    public void StartJump()
    {
        IsJumping = true;
        _jumpTimer = 0f;
        _currentJumpForce = _minJumpForce;
        _gravitySystem.ApplyJumpGravity();
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _currentJumpForce);
    }

    public void HandleJump(bool isHolding)
    {
        if (!IsJumping) return;

        _jumpTimer += Time.fixedDeltaTime;
        float jumpProgress = Mathf.Clamp01(_jumpTimer / _maxJumpTime);
        _currentJumpForce = Mathf.Lerp(_minJumpForce, _maxJumpForce, jumpProgress);

        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _currentJumpForce);

        if (_jumpTimer >= _maxJumpTime || !isHolding)
        {
            EndJump();
        }
    }

    public void EndJump()
    {
        IsJumping = false;
    }
}