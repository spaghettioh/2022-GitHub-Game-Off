using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int _winCollectAmount;
    [SerializeField] private float _winTimerSeconds;
    [SerializeField] private string _nextScene;
    [SerializeField] private string _retryScene;
    [SerializeField] private ClumpDataSO _clumpData;
    [SerializeField] private ScoreSO _score;

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
        _clumpData.OnPropCountChanged += CheckForCollectionWin;
    }

    private void OnDisable()
    {
        _sceneLoaded.OnEventRaised -= FireEvents;
        _timerFinished.OnEventRaised -= TimeIsUp;
        _clumpData.OnPropCountChanged -= CheckForCollectionWin;
    }

    private bool HasReachedWinAmount() => _clumpData.CollectedCount >= _winCollectAmount;

    private void FireEvents()
    {
        _propsToWin.Raise(_winCollectAmount, name);
        _secondsToWin.Raise(_winTimerSeconds, name);
        _openCurtains.Raise(name);
        _score.SetTimeThisForLevel(_winTimerSeconds);
    }

    private void TimeIsUp()
    {
        _pauseEvent.Raise(true, false, name);
        StartCoroutine(WinLoseCheckRoutine());
    }

    private void CheckForCollectionWin(int count)
    {
        if (HasReachedWinAmount()) StartCoroutine(WinLoseCheckRoutine());
    }

    private IEnumerator WinLoseCheckRoutine()
    {
        var won = HasReachedWinAmount();
        if (won)
            _winEvent.Raise(name);
        else
            _loseEvent.Raise(name);
        _pauseEvent.Raise(true, true, name);

        yield return new WaitForSeconds(2f);

        _loadEvent.RaiseWinScene(won ? _nextScene : _retryScene, name);
    }
}
