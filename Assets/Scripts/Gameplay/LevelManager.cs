using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int _winCollectAmount;
    [SerializeField] private float _winTimerSeconds;
    [SerializeField] private string _nextScene;
    [SerializeField] private string _retryScene;
    [SerializeField] private ClumpDataSO _clumpData;

    [Header("Listening to...")]
    [SerializeField] private VoidEventSO _timerFinished;
    [SerializeField] private VoidEventSO _sceneLoaded;

    [Header("Broadcasting to...")]
    [SerializeField] private IntEventSO _propsToWin;
    [SerializeField] private FloatEventSO _secondsToWin;
    [SerializeField] private VoidEventSO _winEvent;
    [SerializeField] private VoidEventSO _loseEvent;
    [SerializeField] private PauseGameplayEventSO _pauseEvent;
    [SerializeField] private LoadEventSO _loadEvent;
    [SerializeField] private VoidEventSO _openCurtains;

    private void OnEnable()
    {
        _sceneLoaded.OnEventRaised += FireEvents;
        _timerFinished.OnEventRaised += TimeIsUp;
    }

    private void OnDisable()
    {
        _sceneLoaded.OnEventRaised -= FireEvents;
        _timerFinished.OnEventRaised -= TimeIsUp;
    }

    private void FireEvents()
    {
        _propsToWin.Raise(_winCollectAmount, name);
        _secondsToWin.Raise(_winTimerSeconds, name);
        _openCurtains.Raise(name);
    }

    private void TimeIsUp()
    {
        _pauseEvent.Raise(true, false, name);

        if (HasReachedWinAmount())
        {
            _winEvent.Raise(name);
            StartCoroutine(WinConditionMet());
        }
        else
        {
            _loseEvent.Raise(name);
            StartCoroutine(LoseConditionMet());
        }
    }

    private bool HasReachedWinAmount() => _clumpData.Size >= _winCollectAmount;

    private IEnumerator WinConditionMet()
    {
        yield return new WaitForSeconds(2f);
        _loadEvent.Raise(_nextScene, name);
    }

    private IEnumerator LoseConditionMet()
    {
        yield return new WaitForSeconds(2f);
        _loadEvent.Raise(_retryScene, name);
    }
}
