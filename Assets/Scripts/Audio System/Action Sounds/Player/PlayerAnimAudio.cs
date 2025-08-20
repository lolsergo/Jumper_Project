using UnityEngine;
using Zenject;

public class PlayerAnimAudio : MonoBehaviour
{
    [SerializeField] private SoundGroupID footstepGroupId = SoundGroupID.Footstep_Default;
    [SerializeField] private SoundGroupID jumpGroupId = SoundGroupID.Jump_Default;

    [Inject] private AudioManager _audio;
    [Inject] private GameSpeedManager _gsm;

    // Animation Event: ���
    public void AnimFootstep()
    {
        // �������� ���� ������ �� ���� �����
        _audio.PlayFromGroup(
            footstepGroupId,
            loop: false,
            pitch: 1f,
            speed: 1f // ������������� ��������
        );
    }

    // Animation Event: ������
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