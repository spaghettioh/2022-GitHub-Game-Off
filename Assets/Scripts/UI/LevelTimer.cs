using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private float _timeRemainingSeconds = 10;
    [SerializeField] private bool _timerIsRunning = false;
    [SerializeField] private ScoreSO _scoreSO;

    [Header("Listening to...")]
    [SerializeField] private VoidEventSO _curtainsOpened;
    [SerializeField] private FloatEventSO _secondsToWin;
    [SerializeField] private VoidEventSO _winCondition;

    [Header("Broadcasting to...")]
    [SerializeField] private VoidEventSO _timerFinished;

    private void OnEnable()
    {
        _secondsToWin.OnEventRaised += SetTimer;
        _curtainsOpened.OnEventRaised += StartTimer;
        _winCondition.OnEventRaised += CaptureWinTime;
    }

    private void OnDisable()
    {
        _secondsToWin.OnEventRaised -= SetTimer;
        _curtainsOpened.OnEventRaised -= StartTimer;
        _winCondition.OnEventRaised -= CaptureWinTime;
    }

    private void SetTimer(float seconds)
    {
        _timeRemainingSeconds = seconds;
    }

    private void StartTimer()
    {
        _timerIsRunning = true;
    }

    private void Update()
    {
        if (_timerIsRunning)
        {
            if (_timeRemainingSeconds > 0)
            {
                _timeRemainingSeconds -= Time.deltaTime;
                DisplayTime(_timeRemainingSeconds);
            }
            else
            {
                _timeRemainingSeconds = 0;
                _timerIsRunning = false;
                _timerFinished.Raise(name);
            }
        }
    }

    private void DisplayTime(float timeToDisplay)
    {
        timeText.text = _scoreSO.GetFormattedTime(timeToDisplay);
    }

    private void CaptureWinTime()
    {
        _timerIsRunning = false;
        _scoreSO.SetTimeThisLevel(_timeRemainingSeconds);
    }
}