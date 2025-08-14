using UnityEngine;
using Zenject;

public class BackgroundScroller : MonoBehaviour
{
    private GameSpeedManager _speedManager;
    public Sprite backgroundSprite;
    public int bufferCount = 3;

    protected Transform[] backgroundPieces;
    protected float spriteWidth;
    private float cameraLeftEdge;
    private float cameraRightEdge;
    private float nextRepositionX;

    [Inject]
    private void Construct(GameSpeedManager speedManager)
    {
        _speedManager = speedManager;
    }

    private void Start()
    {
        if (backgroundSprite == null)
        {
            Debug.LogError("Background sprite is not assigned!");
            return;
        }

        spriteWidth = backgroundSprite.bounds.size.x;
        CreateBackgroundPieces();
        UpdateCameraEdges();
        nextRepositionX = cameraLeftEdge - spriteWidth;
        PositionBackgroundPieces();
    }

    protected virtual void CreateBackgroundPieces()
    {
        backgroundPieces = new Transform[bufferCount];
        for (int i = 0; i < bufferCount; i++)
        {
            GameObject bgObject = new($"Background_{i}");
            SpriteRenderer sr = bgObject.AddComponent<SpriteRenderer>();
            sr.sprite = backgroundSprite;
            sr.sortingLayerName = "Background";
            sr.sortingOrder = 0;
            backgroundPieces[i] = bgObject.transform;
        }
    }

    private void UpdateCameraEdges()
    {
        cameraLeftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero).x;
        cameraRightEdge = Camera.main.ViewportToWorldPoint(Vector3.one).x;
    }

    protected virtual void PositionBackgroundPieces()
    {
        for (int i = 0; i < backgroundPieces.Length; i++)
        {
            backgroundPieces[i].position = GetPiecePosition(i);
        }
    }

    protected virtual Vector3 GetPiecePosition(int index)
    {
        return new Vector3(
            Camera.main.ViewportToWorldPoint(Vector3.zero).x + (index * spriteWidth),
            0f,
            0f
        );
    }

    private void Update()
    {
        UpdateCameraEdges();
        MovePieces();
        CheckReposition();
    }

    protected virtual void MovePieces()
    {
        for (int i = 0; i < backgroundPieces.Length; i++)
        {
            backgroundPieces[i].Translate(-_speedManager.GameSpeed * Time.deltaTime, 0, 0);
        }
    }

    private void CheckReposition()
    {
        if (backgroundPieces[0].position.x <= nextRepositionX)
        {
            RepositionLeadingPiece();
            nextRepositionX = backgroundPieces[0].position.x - spriteWidth;
        }
    }

    protected virtual void RepositionLeadingPiece()
    {
        Transform furthestPiece = GetFurthestRightPiece();
        backgroundPieces[0].position = GetRepositionPosition(furthestPiece);
        ShiftPiecesArray();
    }

    protected Transform GetFurthestRightPiece()
    {
        Transform furthestPiece = backgroundPieces[0];
        float maxX = furthestPiece.position.x;
        for (int i = 1; i < backgroundPieces.Length; i++)
        {
            if (backgroundPieces[i].position.x > maxX)
            {
                maxX = backgroundPieces[i].position.x;
                furthestPiece = backgroundPieces[i];
            }
        }
        return furthestPiece;
    }

    protected virtual Vector3 GetRepositionPosition(Transform furthestPiece)
    {
        return new Vector3(
            furthestPiece.position.x + spriteWidth,
            0f,
            0f
        );
    }

    private void ShiftPiecesArray()
    {
        Transform firstPiece = backgroundPieces[0];
        for (int i = 0; i < backgroundPieces.Length - 1; i++)
        {
            backgroundPieces[i] = backgroundPieces[i + 1];
        }
        backgroundPieces[^1] = firstPiece;
    }
}