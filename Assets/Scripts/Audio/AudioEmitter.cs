using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioEmitter : MonoBehaviour
{
    public UnityAction<AudioEmitter> OnEmitterFinished;
    private AudioSource _audioSource;
    private AudioClip _currentClip;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Plays a random sound effect clip from an audio cue list
    /// </summary>
    /// <param name="audioCue">The sound effect cue</param>
    public void PlaySoundEffect(AudioCueSO audioCue)
    {
        // Grab a random clip
        AudioClip newClip = GetRandomClip(audioCue.Clips);
        _currentClip = newClip;
        _audioSource.PlayOneShot(newClip);

        // Let the audio manager know when it's done
        StartCoroutine(WrapUp());
    }

    /// <summary>
    /// Plays a music clip from an audio cue
    /// </summary>
    /// <param name="audioCue"></param>
    public void PlayMusic(AudioCueSO audioCue)
    {
        // Grab a random clip
        AudioClip newClip = GetRandomClip(audioCue.Clips);
        _audioSource.loop = true;

        // Restart the music only if a different track is requested
        if (_currentClip != newClip)
        {
            _audioSource.clip = newClip;
            _currentClip = newClip;
            _audioSource.Play();
        }
    }

    public void FadeMusic(float fadeLength)
    {
        StartCoroutine(FadeOutMusic(fadeLength));
    }

    /// <summary>
    /// Grabs a random clip from the list in the Audio Cue
    /// </summary>
    /// <param name="clips">The list of clips</param>
    /// <returns></returns>
    private AudioClip GetRandomClip(List<AudioClip> clips)
    {
        AudioClip clip = clips[Random.Range(0, clips.Count)];
        return clip;
    }

    private void ClearEmitter()
    {
        _audioSource.clip = null;
        _currentClip = null;
        OnEmitterFinished.Invoke(this);
    }

    /// <summary>
    /// Waits for the clip to finish playing, then resets the emitter object
    /// </summary>
    /// <returns></returns>
    private IEnumerator WrapUp()
    {
        yield return new WaitForSeconds(_currentClip.length);
        ClearEmitter();
    }

    private IEnumerator FadeOutMusic(float fadeLength)
    {
        while (_audioSource.volume > 0)
        {
            float dt = Time.deltaTime;
            _audioSource.volume -= dt;
            yield return new WaitForSeconds(dt);
        }

        ClearEmitter();
    }
}