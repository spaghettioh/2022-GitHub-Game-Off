using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class WinSummaryManager : MonoBehaviour
{
    [SerializeField] private ScoreSO _score;
    [SerializeField] private TMP_Text _totalScore;
    [SerializeField] private TMP_Text _totalTime;
    [SerializeField] private float _timeUntilStarting;
    [SerializeField] private float _timeBetweenPosts;
    [SerializeField] private AudioEventSO _audioEvent;
    [SerializeField] private AudioCueSO _postSound;
    [SerializeField] private UnityEvent _onWinSummaryFinished;
    [SerializeField] private bool _hasRushed;

    private void Start()
    {
        _totalScore.gameObject.SetActive(false);
        _totalScore.text = "";
        _totalTime.gameObject.SetActive(false);
        _totalTime.text = "";
    }

    public void RushWinSummary()
    {
        if (_hasRushed)
        {
            _onWinSummaryFinished.Invoke();
        }
        else
        {
            _timeBetweenPosts = 0f;
            _timeUntilStarting = 0f;
            _hasRushed = true;
        }
    }

    public void StartWinSummary() => StartCoroutine(StartWinSummaryRoutine());
    private IEnumerator StartWinSummaryRoutine()
    {
        yield return new WaitForSeconds(_timeUntilStarting);
        _totalScore.gameObject.SetActive(true);
        _audioEvent.RaisePlayback(_postSound);
        yield return new WaitForSeconds(_timeBetweenPosts);
        _totalScore.text = $"{_score.TotalScore}";
        _audioEvent.RaisePlayback(_postSound);
        yield return new WaitForSeconds(_timeBetweenPosts);
        _totalTime.gameObject.SetActive(true);
        _audioEvent.RaisePlayback(_postSound);
        yield return new WaitForSeconds(_timeBetweenPosts);
        _totalTime.text = _score.GetFormattedTime(_score.TotalTime);
        _audioEvent.RaisePlayback(_postSound);
        _hasRushed = true;
    }
}
