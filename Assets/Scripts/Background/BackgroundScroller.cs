using UnityEngine;
using Zenject;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] protected Sprite _backgroundSprite;
    [SerializeField] protected int _bufferCount = 3;

    [Header("Background Variants")]
    [SerializeField] private Sprite[] _backgroundVariants = new Sprite[5];

    private int _resetCounter = 0;
    private Sprite _currentActiveSprite;

    protected Transform[] _backgroundPieces;
    protected float _spriteWidth;
    private float _cameraLeftEdge;
    private float _cameraRightEdge;
    private float _nextRepositionX;

    private GameSpeedManager _speedManager;
    private Camera _mainCamera;

    [Inject]
    private void Construct(GameSpeedManager speedManager)
    {
        _speedManager = speedManager;
    }

    private void Awake()
    {
        _mainCamera = Camera.main;
        if (_mainCamera == null)
            Debug.LogError("Main Camera not found!", this);
    }

    private void Start()
    {
        // Выбор первого спрайта случайно, если есть варианты
        if (_backgroundVariants != null && _backgroundVariants.Length > 0)
        {
            _currentActiveSprite = _backgroundVariants[Random.Range(0, _backgroundVariants.Length)];
            if (_currentActiveSprite != null)
                _backgroundSprite = _currentActiveSprite;
        }
        else
        {
            _currentActiveSprite = _backgroundSprite;
        }

        if (_backgroundSprite == null)
        {
            Debug.LogError($"{nameof(_backgroundSprite)} is not assigned!", this);
            enabled = false;
            return;
        }

        _spriteWidth = _backgroundSprite.bounds.size.x;
        CreateBackgroundPieces();
        UpdateCameraEdges();
        _nextRepositionX = _cameraLeftEdge - _spriteWidth;
        PositionBackgroundPieces();
    }

    protected virtual void CreateBackgroundPieces()
    {
        _backgroundPieces = new Transform[_bufferCount];
        for (int i = 0; i < _bufferCount; i++)
        {
            GameObject bgObject = new($"Background_{i}");
            SpriteRenderer sr = bgObject.AddComponent<SpriteRenderer>();
            sr.sprite = _currentActiveSprite;
            sr.sortingLayerName = "Background";
            sr.sortingOrder = 0;
            _backgroundPieces[i] = bgObject.transform;
        }
    }

    private void UpdateCameraEdges()
    {
        if (_mainCamera == null) return;
        _cameraLeftEdge = _mainCamera.ViewportToWorldPoint(Vector3.zero).x;
        _cameraRightEdge = _mainCamera.ViewportToWorldPoint(Vector3.one).x;
    }

    protected virtual void PositionBackgroundPieces()
    {
        for (int i = 0; i < _backgroundPieces.Length; i++)
        {
            _backgroundPieces[i].position = GetPiecePosition(i);
        }
    }

    protected virtual Vector3 GetPiecePosition(int index)
    {
        if (_mainCamera == null) return Vector3.zero;
        return new Vector3(
            _mainCamera.ViewportToWorldPoint(Vector3.zero).x + (index * _spriteWidth),
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
        if (_speedManager == null) return;
        for (int i = 0; i < _backgroundPieces.Length; i++)
        {
            _backgroundPieces[i].Translate(-_speedManager.GameSpeed * Time.deltaTime, 0, 0);
        }
    }

    private void CheckReposition()
    {
        if (_backgroundPieces[0].position.x <= _nextRepositionX)
        {
            RepositionLeadingPiece();
            _nextRepositionX = _backgroundPieces[0].position.x - _spriteWidth;
        }
    }

    protected virtual void RepositionLeadingPiece()
    {
        _resetCounter++;

        if (_resetCounter % 10 == 0 && _backgroundVariants != null && _backgroundVariants.Length > 0)
        {
            Sprite newSprite;
            do
            {
                newSprite = _backgroundVariants[Random.Range(0, _backgroundVariants.Length)];
            } while (newSprite == _currentActiveSprite && _backgroundVariants.Length > 1);

            if (newSprite != null)
            {
                _currentActiveSprite = newSprite;
                _spriteWidth = _currentActiveSprite.bounds.size.x;
            }
        }

        Transform furthestPiece = GetFurthestRightPiece();
        _backgroundPieces[0].position = GetRepositionPosition(furthestPiece);

        var sr = _backgroundPieces[0].GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.sprite = _currentActiveSprite;

        ShiftPiecesArray();
    }

    protected Transform GetFurthestRightPiece()
    {
        Transform furthestPiece = _backgroundPieces[0];
        float maxX = furthestPiece.position.x;
        for (int i = 1; i < _backgroundPieces.Length; i++)
        {
            if (_backgroundPieces[i].position.x > maxX)
            {
                maxX = _backgroundPieces[i].position.x;
                furthestPiece = _backgroundPieces[i];
            }
        }
        return furthestPiece;
    }

    protected virtual Vector3 GetRepositionPosition(Transform furthestPiece)
    {
        return new Vector3(
            furthestPiece.position.x + _spriteWidth,
            0f,
            0f
        );
    }
    private void ShiftPiecesArray()
    {
        Transform firstPiece = _backgroundPieces[0];
        for (int i = 0; i < _backgroundPieces.Length - 1; i++)
        {
            _backgroundPieces[i] = _backgroundPieces[i + 1];
        }
        _backgroundPieces[^1] = firstPiece;
    }
}