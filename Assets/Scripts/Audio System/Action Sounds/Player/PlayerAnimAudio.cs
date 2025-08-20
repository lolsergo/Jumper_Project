using UnityEngine;
using Zenject;

public class PlayerAnimAudio : MonoBehaviour
{
    [SerializeField] private string footstepGroupId = "Footstep_Default";
    [SerializeField] private string jumpGroupId = "Jump_Default";

    [Inject] private AudioManager _audio;
    [Inject] private GameSpeedManager _gsm;

    // Animation Event: шаг
    public void AnimFootstep()
    {
        if (!string.IsNullOrEmpty(footstepGroupId))
        {
            // скорость игры влияет на темп звука
            _audio.PlayFromGroup(
    footstepGroupId,
    loop: false,
    pitch: 1f,
    speed: 1f // фиксированная скорость
);
        }
    }

    // Animation Event: прыжок
    public void AnimJump()
    {
        if (!string.IsNullOrEmpty(jumpGroupId))
        {
            _audio.PlayFromGroup(
    jumpGroupId,
    loop: false,
    pitch: 1f,
    speed: 1f
);
        }
    }
}
