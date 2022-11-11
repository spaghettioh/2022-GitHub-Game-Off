using System;
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
    [field: SerializeField]
    public AudioCueType CueType { get; private set; }

    [field: SerializeField]
    public List<AudioClip> Clips { get; private set; }

    [Range(0, 1)]
    [SerializeField]
    [Tooltip("Variation in pitch for each playback.")]
    private float _pitchVariation;

    /// <summary>
    /// The amount of randomness to vary playback of the asset
    /// </summary>
    public float PitchVariation
    {
        get { return _pitchVariation; }
    }
}
