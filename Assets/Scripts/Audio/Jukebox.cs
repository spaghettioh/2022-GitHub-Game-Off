using UnityEngine;

public class Jukebox : MonoBehaviour
{
    [SerializeField] private AudioCueSO _music;
    [Header("Listening for...")]
    [SerializeField] private VoidEventSO _waxOff;

    [Header("Broadcasting to...")]
    [SerializeField] private AudioEventSO _audioEventChannel;

    private void OnEnable()
    {
        _waxOff.OnEventRaised += TriggerMusic;
    }

    private void OnDisable()
    {
        _waxOff.OnEventRaised -= TriggerMusic;
    }

    private void TriggerMusic()
    {
        _audioEventChannel.RaisePlayback(_music);
    }
}