using UnityEngine;

[CreateAssetMenu(fileName = "CharacterScriptableObject", menuName = "Scriptable Objects/CharacterScriptableObject")]
public class CharacterScriptableObject : ScriptableObject
{
    [SerializeField]
    private float _minJumpForce;
    public float MinJumpForce { get => _minJumpForce; private set=>_minJumpForce = value; }

    [SerializeField]
    private float _maxJumpForce;
    public float MaxJumpForce { get => _maxJumpForce; private set => _maxJumpForce = value; }

    [SerializeField]
    private float _chargeSpeed;
    public float ÑhargeSpeed { get => _chargeSpeed; private set => _chargeSpeed = value; }
}
