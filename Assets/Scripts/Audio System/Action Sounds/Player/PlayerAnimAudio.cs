using UnityEngine;
using Zenject;

public class PlayerAnimAudio : MonoBehaviour
{
    [SerializeField] private string footstepGroupId = "Footstep_Default";
    [SerializeField] private string jumpGroupId = "Jump_Default";

    [Inject] private AudioManager _audio;

    // Animation Event: ראד
    public void AnimFootstep()
    {
        if (!string.IsNullOrEmpty(footstepGroupId))
            _audio.PlayFromGroup(footstepGroupId);
    }

    // Animation Event: ןנûזמך
    public void AnimJump()
    {
        if (!string.IsNullOrEmpty(jumpGroupId))
            _audio.PlayFromGroup(jumpGroupId);
    }
}