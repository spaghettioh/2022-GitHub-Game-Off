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

    private void Awake()
    {
        _audioEmitterPool.PreWarm(_poolSize, transform);
    }

    private void OnEnable()
    {
        _audioEventChannel.OnPlaybackRequested += ActivateEmitter;
    }

    private void OnDisable()
    {
        _audioEventChannel.OnPlaybackRequested -= ActivateEmitter;
    }

    /// <summary>
    /// Pulls an emitter from the pool and starts playback on it
    /// </summary>
    /// <param name="audioCue"></param>
    private void ActivateEmitter(AudioCueSO audioCue)
    {
        if (audioCue.CueType == AudioCueType.Music)
        {
            if (_musicEmitter == null)
            {
                _musicEmitter = _audioEmitterPool.RequestEmitter();
                _musicEmitter.gameObject.SetActive(true);
            }

            _musicEmitter.name = audioCue.name;
            _musicEmitter.PlayMusic(audioCue);
            //_musicEmitter.OnEmitterFinished += ReturnEmitterToPool;
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