using UnityEngine;
using Zenject;

public class PlayerAnimAudio : MonoBehaviour
{
    [SerializeField] private string footstepGroupId = "Footstep_Default";
    [SerializeField] private string jumpGroupId = "Jump_Default";

    [Inject] private AudioManager _audio;
    [Inject] private GameSpeedManager _gsm;

    // Animation Event: ���
    public void AnimFootstep()
    {
        if (!string.IsNullOrEmpty(footstepGroupId))
        {
            // �������� ���� ������ �� ���� �����
            _audio.PlayFromGroup(
    footstepGroupId,
    loop: false,
    pitch: 1f,
    speed: 1f // ������������� ��������
);
        }
    }

    // Animation Event: ������
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
