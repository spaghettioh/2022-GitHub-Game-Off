using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class PropScaleManager : MonoBehaviour
{
    [SerializeField] private Transform _scaler;
    [SerializeField] private float _scaleFactor = .5f;
    [SerializeField] private float _scaleDuration = 2f;

    [Header("Listening to...")]
    [SerializeField] private ClumpDataSO _clumpData;

    [Header("Broadcasting to...")]
    [SerializeField] private PauseGameplayEventSO _pauseGameplay;

    [Header("DEBUG")]
    [SerializeField] private List<Transform> _scaleGroups;
    [SerializeField] private PropScaleCategory _currentClumpScale;
    [SerializeField] private PropManager _propManager;
    [SerializeField] private ClumpPropCollection _collectionObject;

    private void Awake()
    {
        TryGetComponent(out _propManager);
    }

    private void OnEnable()
    {
        _clumpData.OnScaleChanged += CheckForScaleUp;
    }

    private void OnDisable()
    {
        _clumpData.OnScaleChanged -= CheckForScaleUp;
    }

    private void Start()
    {
        BuildScaleGroups();
        _currentClumpScale = _clumpData.Scale;
    }

    private void CheckForScaleUp(PropScaleCategory newScale)
    {
        if (_currentClumpScale != newScale)
        {
            // Pause and stop movement to start scaling everything
            _pauseGameplay.Raise(true, true, name);

            // Scale all the objects already collected
            _propManager.CurrentCollection.ForEach(p =>
            {
                // Forces stuff to reach the attach point if still travelling
                p.transform.DOComplete();
                //p.transform.DOScale(
                //    p.transform.localScale.x * _scaleFactor, _scaleDuration);
            });

            // Move the scaler to the clump position
            // to scale everything relative to that
            var pos = _clumpData.Transform.position;
            pos.y = 0;
            _scaler.transform.position = pos;

            // switch the scale group parents to scale relative to the clump
            _scaleGroups.ForEach(group => group.SetParent(_scaler.transform));

            // scale everything down
            _scaler.DOScale(_scaleFactor, _scaleDuration).OnComplete(() =>
            {
                // reparent the groups
                _scaleGroups.ForEach(group =>
                {
                    if (group.localScale.x < .25f)
                    {
                        group.gameObject.SetActive(false);
                    }
                    else
                    {
                        group.SetParent(transform);
                    }
                });

                // Unpause and resume gameplay
                _pauseGameplay.Raise(false, name);

                // Reset the scaler for next time
                _scaler.DOScale(1f, 0);
            });

            _currentClumpScale = newScale;
        }
    }

    private void BuildScaleGroups()
    {
        _scaleGroups = new List<Transform>(
            GetComponentsInChildren<Transform>().ToList()
            .FindAll(t => t.GetComponent<PropScaleObjectGroup>() != null)
        );
    }

    private void OnValidate()
    {
        BuildScaleGroups();
    }
}
