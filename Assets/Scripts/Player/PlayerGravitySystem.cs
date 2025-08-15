using UnityEngine;

public class GravitySystem
{
    private readonly Rigidbody2D _rb;
    private readonly GravityConfig _config;

    public float NormalGravity => _config.NormalGravity;
    public float JumpGravity => _config.JumpGravity;
    public float FastFallGravity => _config.FastFallGravity;
    public float FloatFallSpeed => _config.FloatFallSpeed;

    public GravitySystem(Rigidbody2D rb, GravityConfig config)
    {
        _rb = rb;
        _config = config;
        _rb.gravityScale = NormalGravity;
    }

    public void ApplyNormalGravity() => _rb.gravityScale = NormalGravity;
    public void ApplyJumpGravity() => _rb.gravityScale = JumpGravity;
    public void ApplyFastFallGravity() => _rb.gravityScale = FastFallGravity;

    public void ApplyFloatFall()
    {
        _rb.gravityScale = JumpGravity;
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, Mathf.Max(_rb.linearVelocity.y, -FloatFallSpeed));
    }
}
