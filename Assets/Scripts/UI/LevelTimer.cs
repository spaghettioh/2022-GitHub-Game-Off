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

    [Header("Listening to...")]
    [SerializeField] private VoidEventSO _curtainsOpened;
    [SerializeField] private FloatEventSO _secondsToWin;

    [Header("Broadcasting to...")]
    [SerializeField] private VoidEventSO _timerFinished;

    private void OnEnable()
    {
        _secondsToWin.OnEventRaised += SetTimer;
        _curtainsOpened.OnEventRaised += StartTimer;
    }

    private void OnDisable()
    {
        _secondsToWin.OnEventRaised -= SetTimer;
        _curtainsOpened.OnEventRaised -= StartTimer;
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
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}