using System;
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
        _audioEventChannel.OnStopMusicRequested += StopMusic;
        _audioEventChannel.OnPauseUnpauseMusicRequested += PauseUnpauseMusic;
    }

    private void OnDisable()
    {
        _audioEventChannel.OnPlaybackRequested -= ActivateEmitter;
        _audioEventChannel.OnMusicFadeRequested -= FadeOutMusic;
        _audioEventChannel.OnStopMusicRequested -= StopMusic;
        _audioEventChannel.OnPauseUnpauseMusicRequested -= PauseUnpauseMusic;
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
                _musicEmitter = _audioEmitterPool.Request();
                _musicEmitter.gameObject.SetActive(true);
            }

            _musicEmitter.name = audioCue.name;
            _musicEmitter.PlayMusic(audioCue);
        }
        else
        {
            // Enable an available emitter and send it the audio to play
            AudioEmitter emitter = _audioEmitterPool.Request();
            emitter.name = audioCue.name;
            emitter.gameObject.SetActive(true);
            emitter.OnEmitterFinished += ReturnEmitterToPool;
            emitter.PlaySoundEffect(audioCue);
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
        _fadingMusicEmitter.OnEmitterFinished += ReturnEmitterToPool;
        _fadingMusicEmitter.FadeMusic(fadeLength);
    }

    private void StopMusic()
    {
        if (_musicEmitter == null)
        {
            Debug.LogWarning($"{name} heard Stop event but there's " +
                $"no music emitter");
            return;
        }

        _musicEmitter.OnEmitterFinished += ReturnEmitterToPool;
        _musicEmitter.Stop();
    }

    private void PauseUnpauseMusic(bool pauseUnpause)
    {
        if (_musicEmitter == null)
        {
            Debug.LogWarning($"{name} heard Pause event but there's " +
                $"no music emitter");
            return;
        }

        _musicEmitter.PauseUnpause(pauseUnpause);
    }

    /// <summary>
    /// Adds the emitter back to the pool when it's done playing
    /// </summary>
    /// <param name="emitter"></param>
    private void ReturnEmitterToPool(AudioEmitter emitter)
    {
        emitter.gameObject.SetActive(false);
        _audioEmitterPool.Return(emitter);
        emitter.OnEmitterFinished -= ReturnEmitterToPool;
    }
}