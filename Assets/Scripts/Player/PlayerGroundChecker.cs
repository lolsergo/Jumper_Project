using System;
using UnityEngine;

public class GroundChecker
{
    private readonly Transform _characterTransform;
    private readonly LayerMask _groundLayer;

    // Теперь принимаем трансформ персонажа, а не отдельный groundCheck
    public GroundChecker(Transform characterTransform, LayerMask groundLayer)
    {
        _characterTransform = characterTransform ?? throw new ArgumentNullException(nameof(characterTransform));
        _groundLayer = groundLayer;
    }

    public bool IsGroundNear(float distance, float checkOffset)
    {
        Vector2 rayStart = _characterTransform.position + new Vector3(0, -checkOffset, 0);
        RaycastHit2D hit = Physics2D.Raycast(rayStart, Vector2.down, distance, _groundLayer);
        Debug.DrawRay(rayStart, Vector2.down * distance, Color.red);
        return hit.collider != null;
    }
}
