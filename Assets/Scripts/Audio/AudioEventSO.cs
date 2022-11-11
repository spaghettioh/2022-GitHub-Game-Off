using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Game Off/Scriptable Objects/Audio Event Channel",
    fileName = "AudioEventChannel")]
public class AudioEventSO : ScriptableObject
{
    public UnityAction<AudioCueSO> OnPlaybackRequested;
    public UnityAction<float> OnMusicFadeRequested;

    public void RaisePlayback(AudioCueSO audioCue)
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

    public void RaiseMusicFade(float fadeLength)
    {
        if (OnMusicFadeRequested != null)
        {
            OnMusicFadeRequested.Invoke(fadeLength);
        }
        else
        {
            Debug.LogWarning("Music fade event raised but nothing " +
                "listens...");
        }
    }
}