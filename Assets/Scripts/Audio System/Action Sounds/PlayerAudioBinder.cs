using UnityEngine;
using Zenject;

public class PlayerAudioSimple : MonoBehaviour
{
    [Header("ID группы шагов и прыжка в AudioLibrary")]
    [SerializeField] private string footstepGroupId = "Footstep_Default";
    [SerializeField] private string jumpGroupId = "Jump_Default";

    [Header("—коростные пороги")]
    [SerializeField] private float walkMinSpeed = 0.1f;   // ниже Ч тишина
    [SerializeField] private float runMinSpeed = 8f;      // выше Ч считаем бегом

    [Header("»нтервалы шагов")]
    [SerializeField] private float walkStepInterval = 0.4f; // сек между шагами при ходьбе
    [SerializeField] private float runStepInterval = 4f;  // сек между шагами при беге

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

        // ѕрыжок
        if (!_prevJumping && jumping && !string.IsNullOrEmpty(jumpGroupId))
        {
            _audio.PlayFromGroup(jumpGroupId);
        }

        // Ўаги (одна группа, но разный темп)
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