using UnityEngine;

public class FloorScroller : BackgroundScroller
{
    [SerializeField]
    private float yPosition = -2.5f;

    protected override Vector3 GetPiecePosition(int index)
    {
        Vector3 basePosition = base.GetPiecePosition(index);
        return new Vector3(basePosition.x, yPosition, basePosition.z);
    }

    protected override Vector3 GetRepositionPosition(Transform furthestPiece)
    {
        Vector3 basePosition = base.GetRepositionPosition(furthestPiece);
        return new Vector3(basePosition.x, yPosition, basePosition.z);
    }

    protected override void CreateBackgroundPieces()
    {
        backgroundPieces = new Transform[bufferCount];
        for (int i = 0; i < bufferCount; i++)
        {
            GameObject bgObject = new($"Floor_{i}");
            SpriteRenderer sr = bgObject.AddComponent<SpriteRenderer>();
            sr.sprite = backgroundSprite;
            sr.sortingLayerName = "Foreground"; // Другой слой
            sr.sortingOrder = 1; // Выше, чем фон
            backgroundPieces[i] = bgObject.transform;
        }
    }
}
