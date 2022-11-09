using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Game Off/Scriptable Objects/Audio Event Channel",
    fileName = "AudioEventChannel")]
public class AudioEventSO : ScriptableObject
{
    public UnityAction<AudioCueSO> OnPlaybackRequested;

    public void Raise(AudioCueSO audioCue)
    {
        if (OnPlaybackRequested != null)
        {
            OnPlaybackRequested.Invoke(audioCue);
        }
        else
        {
            Debug.LogWarning("Audio playback event raised but nothing " +
                "listens...");
        }
    }
}