using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Level config")]
    [SerializeField] private int _winCollectAmount;
    [SerializeField] private float _winTimerSeconds;
    [field: SerializeField] public string NextScene { get; private set; }
    [SerializeField] private Prop _finishLine;
    [SerializeField] private Prop _d6;

    [Header("Level manager prefab")]
    [SerializeField] private ClumpDataSO _clumpData;
    [SerializeField] private ScoreSO _score;
    [SerializeField] private float _winLoseWaitTime = 2f;

    [Header("Dialog pops")]
    [SerializeField] private VoidEventSO _winEvent;
    [SerializeField] private VoidEventSO _loseEvent;
    [SerializeField] private VoidEventSO _notEnough;
    [SerializeField] private VoidEventSO _d6Collide;
    [SerializeField] private VoidEventSO _findExit;

    [Header("Listening to...")]
    [SerializeField] private VoidEventSO _timerFinished;
    [SerializeField] private VoidEventSO _sceneLoaded;

    [Header("Broadcasting to...")]
    [SerializeField] private IntEventSO _propsToWin;
    [SerializeField] private FloatEventSO _secondsToWin;
    [SerializeField] private PauseGameplayEventSO _pauseEvent;
    [SerializeField] private LoadEventSO _loadEvent;
    [SerializeField] private VoidEventSO _openCurtains;
    [SerializeField] private bool _hasReachedWinAmount;

    private void OnEnable()
    {
        _sceneLoaded.OnEventRaised += SetUpScene;
        _timerFinished.OnEventRaised += TimeIsUp;
        _clumpData.OnPropCountChanged += CheckIfCollectedEnough;
        _finishLine.OnCollisionEvent += OnFinishLineCollision;
        _d6.OnCollisionEvent += OnD6Collision;
    }

    private void OnDisable()
    {
        _sceneLoaded.OnEventRaised -= SetUpScene;
        _timerFinished.OnEventRaised -= TimeIsUp;
        _clumpData.OnPropCountChanged -= CheckIfCollectedEnough;
        _finishLine.OnCollisionEvent -= OnFinishLineCollision;
        _d6.OnCollisionEvent -= OnD6Collision;
    }

    private void OnD6Collision(Prop d6)
    {
        _d6Collide.Raise(name);
    }

    private void CheckIfCollectedEnough(int collected)
    {
        _hasReachedWinAmount = collected >= _winCollectAmount;

        if (_hasReachedWinAmount)
        {
            _findExit.Raise(name);
        }
    }

    private void SetUpScene()
    {
        _propsToWin.Raise(_winCollectAmount, name);
        _secondsToWin.Raise(_winTimerSeconds, name);
        _openCurtains.Raise(name);
        _score.SetTimeThisForLevel(_winTimerSeconds);
        _finishLine.SetFinishLineSize(_winCollectAmount);
    }

    private void TimeIsUp()
    {
        StartCoroutine(WinLoseCheckRoutine(false));
    }

    private void OnFinishLineCollision(Prop finishLine)
    {
        StartCoroutine(WinLoseCheckRoutine(true));
    }

    private IEnumerator WinLoseCheckRoutine(bool isFinishLine)
    {
        if (isFinishLine)
        {
            if (_hasReachedWinAmount)
            {
                _winEvent.Raise(name);
                _pauseEvent.Raise(true, false, name);
                yield return new WaitForSeconds(_winLoseWaitTime);
                _loadEvent.RaiseWinScene(NextScene, name);
            }
            else
            {
                _notEnough.Raise(name);
                yield break;
            }
        }
        else
        {
            _loseEvent.Raise(name);
            _pauseEvent.Raise(true, false, name);
            yield return new WaitForSeconds(_winLoseWaitTime);
            _loadEvent.Raise(SceneManager.GetActiveScene().name, name);
        }
    }
}
