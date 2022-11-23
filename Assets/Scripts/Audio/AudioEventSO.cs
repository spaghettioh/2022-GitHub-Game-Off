using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Game Off/Audio Event Channel",
    fileName = "AudioEventChannel")]
public class AudioEventSO : ScriptableObject
{
    public UnityAction<AudioCueSO> OnPlaybackRequested;
    public UnityAction<float> OnMusicFadeRequested;
    public UnityAction OnStopMusicRequested;

    public void RaisePlayback(AudioCueSO audioCue) =>
        RaisePlayback(audioCue, "An inspector event, probably,");
    public void RaisePlayback(AudioCueSO audioCue,
        string elevator = "(Unknown)")
    {
        if (OnPlaybackRequested != null)
        {
            OnPlaybackRequested.Invoke(audioCue);
        }
        else
        {
            Debug.LogWarning($"{elevator} raised {name} playback for " +
                $"{audioCue.name} but no one listens.");
        }
    }

    public void RaiseMusicFade(float fadeLength) =>
        RaiseMusicFade(fadeLength, "An inspector event, probably,");
    public void RaiseMusicFade(float fadeLength, string elevator = "(Unknown)")
    {
        if (OnMusicFadeRequested != null)
        {
            OnMusicFadeRequested.Invoke(fadeLength);
        }
        else
        {
            Debug.LogWarning($"{elevator} raised {name} fade for " +
                $"{fadeLength} seconds but no one listens.");
        }
    }

    public void RaiseStopMusic(string elevator = "(Unknown)")
    {
        if (OnStopMusicRequested != null)
        {
            OnStopMusicRequested.Invoke();
        }
        else
        {
            Debug.LogWarning($"{elevator} raised stop music " +
                $"but no one listens");
        }
    }
}