using UnityEngine;
using Zenject;

public class FootstepPlayerGroup : MonoBehaviour
{
    [SerializeField] private string _groupId = "Footstep_Default";

    [Header("Темп шагов")]
    [SerializeField] private float _baseStepsPerSecond = 2f;
    [SerializeField] private float _tempoScaleLow = 0.7f;
    [SerializeField] private float _tempoScaleHigh = 1.5f;

    [Header("Питч и скорость клипа")]
    [SerializeField] private float _minPitch = 0.95f;
    [SerializeField] private float _maxPitch = 1.25f;
    [SerializeField] private float _pitchJitter = 0.04f;
    [SerializeField] private float _minClipSpeed = 0.95f;
    [SerializeField] private float _maxClipSpeed = 1.25f;

    private float _stepTimer;

    [Inject] private AudioManager _audio;
    [Inject] private AudioLibrary _library;
    [Inject] private GameSpeedManager _speedManager;

    private void Update()
    {
        if (_library.GetGroup(_groupId) == null) return;

        float gs = Mathf.Max(0f, _speedManager.GameSpeed);
        if (gs <= 0.01f) { _stepTimer = 0f; return; }

        float t = Mathf.Clamp01(gs);

        float sps = _baseStepsPerSecond * Mathf.Lerp(_tempoScaleLow, _tempoScaleHigh, t);
        float interval = 1f / Mathf.Max(0.01f, sps);

        _stepTimer += Time.deltaTime;
        if (_stepTimer < interval) return;
        _stepTimer -= interval;

        float tonalPitch = Mathf.Lerp(_minPitch, _maxPitch, t) + Random.Range(-_pitchJitter, _pitchJitter);
        float clipSpeed = Mathf.Lerp(_minClipSpeed, _maxClipSpeed, t);

        _audio.PlayFromGroup(_groupId, loop: false, pitch: tonalPitch, speed: clipSpeed);
    }
}