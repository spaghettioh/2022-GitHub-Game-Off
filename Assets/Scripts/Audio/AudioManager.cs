using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Emitter pooling")]
    [SerializeField] private int _poolSize;
    [SerializeField] private AudioEmitterPoolSO _audioEmitterPool;

    [Header("Listening to...")]
    [SerializeField] private AudioEventSO _audioEventChannel;

    private AudioEmitter _musicEmitter;
    private AudioEmitter _fadingMusicEmitter;

    private void Awake()
    {
        _audioEmitterPool.PreWarm(_poolSize, transform);
    }

    private void OnEnable()
    {
        _audioEventChannel.OnPlaybackRequested += ActivateEmitter;
        _audioEventChannel.OnMusicFadeRequested += FadeOutMusic;
    }

    private void OnDisable()
    {
        _audioEventChannel.OnPlaybackRequested -= ActivateEmitter;
        _audioEventChannel.OnMusicFadeRequested -= FadeOutMusic;
    }

    /// <summary>
    /// Pulls an emitter from the pool and starts playback on it
    /// </summary>
    /// <param name="audioCue"></param>
    private void ActivateEmitter(AudioCueSO audioCue)
    {
        if (audioCue.CueType == AudioCueType.Music)
        {
            // Create a music emitter if one doesn't exist
            if (_musicEmitter == null)
            {
                _musicEmitter = _audioEmitterPool.RequestEmitter();
                _musicEmitter.gameObject.SetActive(true);
            }

            _musicEmitter.name = audioCue.name;
            _musicEmitter.PlayMusic(audioCue);
        }
        else
        {
            // Enable an available emitter and send it the audio to play
            AudioEmitter emitter = _audioEmitterPool.RequestEmitter();
            emitter.name = audioCue.name;
            emitter.gameObject.SetActive(true);
            emitter.PlaySoundEffect(audioCue);
            emitter.OnEmitterFinished += ReturnEmitterToPool;
        }
    }

    private void FadeOutMusic(float fadeLength)
    {
        if (_musicEmitter == null)
        {
            Debug.LogWarning($"{name} heard Fade event but there's " +
                $"no music emitter");
            return;
        }

        _fadingMusicEmitter = _musicEmitter;
        _musicEmitter = null;
        _fadingMusicEmitter.FadeMusic(fadeLength);
        _fadingMusicEmitter.OnEmitterFinished += ReturnEmitterToPool;
    }

    /// <summary>
    /// Adds the emitter back to the pool when it's done playing
    /// </summary>
    /// <param name="emitter"></param>
    private void ReturnEmitterToPool(AudioEmitter emitter)
    {
        emitter.gameObject.SetActive(false);
        _audioEmitterPool.ReturnEmitter(emitter);
        emitter.OnEmitterFinished -= ReturnEmitterToPool;
    }
}