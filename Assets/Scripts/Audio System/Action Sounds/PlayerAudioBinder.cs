using UnityEngine;
using Zenject;

public class PlayerAudioSimple : MonoBehaviour
{
    [Header("ID ������ ����� � ������ � AudioLibrary")]
    [SerializeField] private string footstepGroupId = "Footstep_Default";
    [SerializeField] private string jumpGroupId = "Jump_Default";

    [Header("���������� ������")]
    [SerializeField] private float walkMinSpeed = 0.1f;   // ���� � ������
    [SerializeField] private float runMinSpeed = 8f;      // ���� � ������� �����

    [Header("��������� �����")]
    [SerializeField] private float walkStepInterval = 0.4f; // ��� ����� ������ ��� ������
    [SerializeField] private float runStepInterval = 4f;  // ��� ����� ������ ��� ����

    private PlayerController _player;
    private float _stepTimer;
    private bool _prevJumping;

    [Inject] private AudioManager _audio;
    [Inject] private GameSpeedManager _gsm;

    private void Awake() => _player = GetComponent<PlayerController>();

    private void OnEnable()
    {
        _prevJumping = _player.IsJumping;
        _stepTimer = 0f;
    }

    private void Update()
    {
        float speed = Mathf.Abs(_gsm.GameSpeed);
        bool grounded = _player.IsGrounded;
        bool jumping = _player.IsJumping;

        // ������
        if (!_prevJumping && jumping && !string.IsNullOrEmpty(jumpGroupId))
        {
            _audio.PlayFromGroup(jumpGroupId);
        }

        // ���� (���� ������, �� ������ ����)
        if (grounded && !jumping && speed >= walkMinSpeed)
        {
            bool isRunning = speed >= runMinSpeed;
            float currentInterval = isRunning ? runStepInterval : walkStepInterval;

            _stepTimer += Time.deltaTime;
            if (_stepTimer >= currentInterval)
            {
                _stepTimer = 0f;
                _audio.PlayFromGroup(footstepGroupId);
            }
        }
        else
        {
            _stepTimer = 0f;
        }

        _prevJumping = jumping;
    }
}