using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenTransitionManager : MonoBehaviour
{
    [SerializeField] private GameObject _scrim;
    [SerializeField] private float _transitionTime;

    [Header("Next scene requested, close curtains")]
    [SerializeField] private VoidEventSO _closeCurtains;
    [Header("Curtains closed, load the next scene")]
    [SerializeField] private VoidEventSO _curtainsClosed;
    [Header("Next scene loaded, open curtains")]
    [SerializeField] private VoidEventSO _openCurtains;
    [Header("Curtains opened, begin mini game")]
    [SerializeField] private VoidEventSO _curtainsOpened;
    [Header("Skip curtains (used by initializers)")]
    [SerializeField] private VoidEventSO _skipCurtains;

    [Header("For pausing the game")]
    [SerializeField] private PauseGameplayEventSO _pauseEvent;

    private void OnEnable()
    {
        _openCurtains.OnEventRaised += OpenCurtains;
        _closeCurtains.OnEventRaised += CloseCurtains;
        _skipCurtains.OnEventRaised += SkipCurtains;
        _pauseEvent.OnEventRaised += PauseGame;
    }

    private void OnDisable()
    {
        _openCurtains.OnEventRaised -= OpenCurtains;
        _closeCurtains.OnEventRaised -= CloseCurtains;
        _skipCurtains.OnEventRaised -= SkipCurtains;
        _pauseEvent.OnEventRaised -= PauseGame;
    }

    private void CloseCurtains() => StartCoroutine(CloseCurtainsRoutine());
    private IEnumerator CloseCurtainsRoutine()
    {
        _scrim.SetActive(true);
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(_transitionTime);

        _curtainsClosed.Raise(name);
    }

    private void OpenCurtains() => StartCoroutine(OpenCurtainsRoutine());
    private IEnumerator OpenCurtainsRoutine()
    {
        yield return new WaitForSecondsRealtime(_transitionTime);
        _scrim.SetActive(false);
        _curtainsOpened.Raise(name);
        Time.timeScale = 1f;
    }

    private void SkipCurtains()
    {
        _scrim.SetActive(false);
        _curtainsOpened.Raise(name);
    }

    private void PauseGame(bool pause, bool shouldStopMovement)
    {
        _scrim.SetActive(pause);
    }
}
