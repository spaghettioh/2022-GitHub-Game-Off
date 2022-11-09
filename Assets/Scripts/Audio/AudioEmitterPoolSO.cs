using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Scriptable Objects/Audio Emitter Pool",
    fileName = "AudioEmitterPool")]
public class AudioEmitterPoolSO : ScriptableObject
{
    [SerializeField] private AudioEmitter _audioEmitterPrefab;

    private Stack<AudioEmitter> _emitterStack = new Stack<AudioEmitter>();
    private Transform _parent;

    /// <summary>
    /// Spins up the audio emitter pool
    /// </summary>
    /// <param name="count">The number of emitters to create</param>
    /// <param name="parent">The parent object for the emitters</param>
    public void PreWarm(int count, Transform parent)
    {
        _parent = parent;
        // Create a pool of disabled audio emitters
        for (var i = 0; i < count; i++)
        {
            CreateEmitter();
        }
    }

    /// <summary>
    /// Instantiates an emitter and adds it to the pool
    /// </summary>
    /// <returns></returns>
    private AudioEmitter CreateEmitter()
    {
        AudioEmitter emitter = Instantiate(_audioEmitterPrefab);
        emitter.transform.SetParent(_parent);
        emitter.gameObject.SetActive(false);
        _emitterStack.Push(emitter);
        return emitter;
    }

    /// <summary>
    /// Pulls an emitter from the pool
    /// </summary>
    /// <returns></returns>
    public AudioEmitter RequestEmitter()
    {
        // If the stack is empty when requesting an emitter, just add more
        if (_emitterStack.Count == 0)
        {
            CreateEmitter();
        }

        return _emitterStack.Pop();
    }

    /// <summary>
    /// Puts an emitter back into the pool
    /// </summary>
    /// <param name="emitter"></param>
    public void ReturnEmitter(AudioEmitter emitter)
    {
        _emitterStack.Push(emitter);
    }

    /// <summary>
    /// Clears the stack whenever Play stops (because the pool is an asset)
    /// </summary>
    private void OnDisable()
    {
        _emitterStack.Clear();
    }
}