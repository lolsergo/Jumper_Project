using UnityEngine;
using Zenject;

public class PlayerAnimAudio : MonoBehaviour
{
    [Header("Sound Groups")]
    [SerializeField] private SoundGroupID _footstepGroupId = SoundGroupID.Footstep_Default;
    [SerializeField] private SoundGroupID _jumpGroupId = SoundGroupID.Jump_Default;

    [Inject] private AudioManager _audio;

    private void Awake()
    {
        if (_audio == null)
            Debug.LogError("AudioManager dependency not injected.", this);
    }
    public void AnimFootstep()
    {
        _audio.PlayFromGroup(
            _footstepGroupId,
            loop: false,
            pitch: 1f,
            speed: 1f
        );
    }

    public void AnimJump()
    {
        _audio.PlayFromGroup(
            _jumpGroupId,
            loop: false,
            pitch: 1f,
            speed: 1f
        );
    }
}