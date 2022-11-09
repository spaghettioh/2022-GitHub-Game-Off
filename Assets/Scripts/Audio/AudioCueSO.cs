using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to determine how the cue will be configured for playback
/// </summary>
public enum AudioCueType
{
    SoundEffect,
    Music,
}

[CreateAssetMenu(menuName = "Game Off/Scriptable Objects/Audio Cue",
    fileName = "SFXorMusic_NAME")]
public class AudioCueSO : ScriptableObject
{
    public AudioCueType CueType;
    public List<AudioClip> Clips;
}