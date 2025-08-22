using UnityEngine;


public class FloorScroller : BackgroundScroller
{
    [SerializeField] private float _yPosition = -2.5f;

    protected override Vector3 GetPiecePosition(int index)
    {
        Vector3 basePosition = base.GetPiecePosition(index);
        return new Vector3(basePosition.x, _yPosition, basePosition.z);
    }

    protected override Vector3 GetRepositionPosition(Transform furthestPiece)
    {
        Vector3 basePosition = base.GetRepositionPosition(furthestPiece);
        return new Vector3(basePosition.x, _yPosition, basePosition.z);
    }

    protected override void CreateBackgroundPieces()
    {
        _backgroundPieces = new Transform[_bufferCount];
        for (int i = 0; i < _bufferCount; i++)
        {
            GameObject floorObject = new($"Floor_{i}");
            SpriteRenderer sr = floorObject.AddComponent<SpriteRenderer>();
            sr.sprite = _backgroundSprite;
            sr.sortingLayerName = "Foreground"; // Foreground layer
            sr.sortingOrder = 1; // Above background
            _backgroundPieces[i] = floorObject.transform;
        }
    }
}
