using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using TMPro;

public class WinScreenManager : MonoBehaviour
{
    [Header("Win Screen")]
    [SerializeField] private float _startTime;
    [Tooltip("How long counting each score should take")]
    [SerializeField] private float _tallyTime;
    [Tooltip("How long score increments should take")]
    [SerializeField] private float _tallyIncrementTime;

    [Header("Prop spawning")]
    [SerializeField] private ClumpPropCollectionSO _propCollection;
    [SerializeField] private WinScreenPropPoolSO _propPool;
    [SerializeField] private Transform _propParent;
    [SerializeField] private Transform _bottleLeft;
    [SerializeField] private Transform _bottleRight;
    [SerializeField] private List<WinScreenProp> _spawnedProps;

    [Header("Score")]
    [SerializeField] private ScoreSO _scoreSO;
    [Tooltip("How long to wait before starting to count the score")]
    [SerializeField] private AudioCueSO _scoreSound;
    [SerializeField] private AudioCueSO _scoreFinishedSound;

    [Header("UI")]
    [SerializeField] private TMP_Text _itemsText;
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private TMP_Text _scoreText;

    [Header("Starts when...")]
    [SerializeField] private VoidEventSO _curtainsOpened;

    [Header("Broadcasting to...")]
    [SerializeField] private VoidEventSO _winScreenFinished;
    [SerializeField] private AudioEventSO _audioEvent;

    [Header("DEBUG ==========")]
    [SerializeField] private bool _test;
    [SerializeField] private float _testRemainingTime;

    [Space]
    [SerializeField] private List<PropData> _propsCollected;
    [SerializeField] private int _poolSize;
    [SerializeField] private float _timeBetweenTallies;
    [SerializeField] private int _totalScore;
    [Space]
    [SerializeField] private int _scoreForCountingUp;
    [SerializeField] private float _thisLevelRemainingTime;
    [SerializeField] private float _timeForCountingDown;

    private void OnEnable()
    {
        _curtainsOpened.OnEventRaised += StartWinScreen;
    }

    private void OnDisable()
    {
        _curtainsOpened.OnEventRaised -= StartWinScreen;
    }

    public void RushWinScreen()
    {
        _tallyIncrementTime = 0f;
        _timeBetweenTallies = 0f;
        _scoreSound = null;
        _scoreFinishedSound = null;
    }

    private void StartWinScreen()
    {
        // Use test data when no props are present
        if (_test)
        {
            _propCollection.Reset();
            GetComponentsInChildren<Prop>(true).ToList()
                .ForEach(prop => _propsCollected.Add(
                new PropData(
                    prop.Sprite,
                    prop.transform.localScale.x,
                    prop.ScorePoints)));
            _timeForCountingDown = _testRemainingTime;
            _totalScore = 1000;
        }
        else
        {
            _totalScore = _scoreSO.TotalScore;
            _propsCollected = _propCollection.PropsWon;
            _timeForCountingDown = _scoreSO.ThisLevelTimeRemaining;
            _timeForCountingDown = _scoreSO.ThisLevelTimeRemaining;
        }

        _poolSize = _propsCollected.Count();
        _spawnedProps = _propPool.PreWarm(_poolSize, _propParent);

        _itemsText.text = _poolSize.ToString();
        _timeText.text = _scoreSO.GetFormattedTime(_timeForCountingDown);
        _scoreText.text = $"{_totalScore}";

        _timeBetweenTallies = _tallyTime / 3f;

        PrepProps();
        StartCoroutine(TallyPropsRoutine());
    }

    private void PrepProps()
    {
        for (int i = 0; i < _spawnedProps.Count; i++)
        {
            var spawnedProp = _spawnedProps[i];
            var matchingPropData = _propsCollected[i];
            spawnedProp.Initialize(matchingPropData, GetRandomPosition());
        }
    }

    private Vector3 GetRandomPosition()
    {
        var lPos = _bottleLeft.position;
        var rPos = _bottleRight.position;
        var randomXPos = Random.Range(lPos.x, rPos.x);
        var randomYPos = Random.Range(lPos.y, lPos.y + 2);
        float randomZPos = lPos.z += Random.Range(-.5f, .5f);
        return new Vector3(randomXPos, randomYPos, randomZPos);
    }

    private IEnumerator TallyPropsRoutine()
    {
        yield return new WaitForSeconds(_startTime);

        while (_spawnedProps.Count > 0)
        {
            int randomPropIndex = Random.Range(0, _spawnedProps.Count);
            WinScreenProp prop = _spawnedProps[randomPropIndex];
            _spawnedProps.Remove(prop);

            prop.gameObject.SetActive(true);
            prop.Drop();
            _itemsText.text = _spawnedProps.Count.ToString();
            AddToScore(prop.ScorePoints);

            yield return new WaitForSeconds(_tallyIncrementTime);
        }

        StartCoroutine(TallyTimeRoutine());
    }

    private IEnumerator TallyTimeRoutine()
    {
        yield return new WaitForSeconds(_timeBetweenTallies);
        //_thisLevelRemainingTime = _timeForCountingDown;
        while (_timeForCountingDown > 0f)
        {
            AddToScore(1000);
            _timeForCountingDown--;
            _timeText.text = _scoreSO.GetFormattedTime(_timeForCountingDown);

            yield return new WaitForSeconds(_tallyIncrementTime);
        }
        _timeText.text = _scoreSO.GetFormattedTime(-1f);

        StartCoroutine(WrapUpPropScreenRoutine());
    }


    private void AddToScore(int amount)
    {
        _scoreForCountingUp += amount;
        _totalScore += amount;
        _scoreText.text = _totalScore.ToString();

        if (_scoreSound != null)
        {
            _audioEvent.RaisePlayback(_scoreSound, name);
        }
    }

    private IEnumerator WrapUpPropScreenRoutine()
    {
        yield return new WaitForSeconds(_timeBetweenTallies);

        if (_scoreFinishedSound != null)
        {
            _audioEvent.RaisePlayback(_scoreFinishedSound, name);
        }

        _winScreenFinished.Raise(name);
        UpdateScoreValues();

        var offTime = .2f;
        var onTime = .3f;
        int loopPrevention = 0;
        
        // Blinks the text
        while (loopPrevention < 1000f)
        {
            _scoreText.gameObject.SetActive(false);
            yield return new WaitForSeconds(offTime);
            _scoreText.gameObject.SetActive(true);
            yield return new WaitForSeconds(onTime);
            loopPrevention++;
        }

        _scoreText.gameObject.SetActive(true);
    }

    private void UpdateScoreValues()
    {
        _scoreSO.UpdateTotalScore(_scoreForCountingUp);
        _scoreSO.UpdateTotalTime();
    }
}
