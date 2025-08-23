using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GravityConfig", menuName = "GravityConfig/Gravity")]
public class GravityConfig : ScriptableObject
{
    [SerializeField]
    private float _minJumpForce;
    public float MinJumpForce { get => _minJumpForce; private set => _minJumpForce = value; }

    [SerializeField]
    private float _maxJumpForce;
    public float MaxJumpForce { get => _maxJumpForce; private set => _maxJumpForce = value; }

    [SerializeField] 
    private float _maxJumpTime;
    public float MaxJumpTime { get => _maxJumpTime; private set => _maxJumpTime = value; }

    [SerializeField] 
    private float _fastFallGravity;
    public float FastFallGravity { get => _fastFallGravity; private set => _fastFallGravity = value; }

    [SerializeField] 
    private float _jumpGravity;
    public float JumpGravity { get => _jumpGravity; private set => _jumpGravity = value; }

    [SerializeField] 
    private float _normalGravity;
    public float NormalGravity { get => _normalGravity; private set => _normalGravity = value; }

    [SerializeField]
    private float _floatFallSpeed;
    public float FloatFallSpeed { get => _floatFallSpeed; private set => _floatFallSpeed = value; }


}