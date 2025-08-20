using UnityEngine;
using Zenject;

public class PlayerAnimAudio : MonoBehaviour
{
    [SerializeField] private SoundGroupID footstepGroupId = SoundGroupID.Footstep_Default;
    [SerializeField] private SoundGroupID jumpGroupId = SoundGroupID.Jump_Default;

    [Inject] private AudioManager _audio;
    [Inject] private GameSpeedManager _gsm;

    // Animation Event: шаг
    public void AnimFootstep()
    {
        // скорость игры влияет на темп звука
        _audio.PlayFromGroup(
            footstepGroupId,
            loop: false,
            pitch: 1f,
            speed: 1f // фиксированная скорость
        );
    }

    // Animation Event: прыжок
    public void AnimJump()
    {
        _audio.PlayFromGroup(
            jumpGroupId,
            loop: false,
            pitch: 1f,
            speed: 1f
        );
    }
}