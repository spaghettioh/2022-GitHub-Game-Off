using UnityEngine;

public class Jukebox : MonoBehaviour
{
    [SerializeField] private AudioCueSO _music;
    [Header("Listening for...")]
    [SerializeField] private VoidEventSO _beginPlaybackOn;

    [Header("Broadcasting to...")]
    [SerializeField] private AudioEventSO _audioEventChannel;

    private void OnEnable()
    {
        _beginPlaybackOn.OnEventRaised += TriggerMusic;
    }

    private void OnDisable()
    {
        _beginPlaybackOn.OnEventRaised -= TriggerMusic;
    }

    private void TriggerMusic()
    {
        _audioEventChannel.RaisePlayback(_music);
    }
}