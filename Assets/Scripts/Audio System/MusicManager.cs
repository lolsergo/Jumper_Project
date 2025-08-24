using UnityEngine;
using Zenject;

public class MusicManager : MonoBehaviour
{
    [Inject] private AudioManager _audio;
    [Inject] private AudioLibrary _library;

    [SerializeField] private SoundGroupID _musicGroupId = SoundGroupID.Music;

    private int _lastTrackIndex = -1;
    private AudioSource _currentSource;

    private void Start()
    {
        PlayNextRandomTrack();
    }

    private void PlayNextRandomTrack()
    {
        var tracks = _library.GetGroupItems(_musicGroupId);
        if (tracks.Length == 0) return;

        int newIndex;
        do
        {
            newIndex = Random.Range(0, tracks.Length);
        } while (tracks.Length > 1 && newIndex == _lastTrackIndex);

        _lastTrackIndex = newIndex;

        if (_currentSource != null)
            _audio.Stop(_currentSource);

        _currentSource = _audio.PlayFromGroupByIndex(_musicGroupId, newIndex, loop: false);

        if (_currentSource != null)
            StartCoroutine(WaitAndPlayNext(_currentSource.clip.length / _currentSource.pitch));
    }

    private System.Collections.IEnumerator WaitAndPlayNext(float delay)
    {
        yield return new WaitForSeconds(delay);
        PlayNextRandomTrack();
    }

    private void OnDestroy()
    {
        if (_currentSource != null)
            _audio.Stop(_currentSource);
    }
}
