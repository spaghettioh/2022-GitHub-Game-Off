using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PropScaleManager : MonoBehaviour
{
    [Header("Listening to...")]
    [SerializeField] private ClumpDataSO _clumpData;
    [SerializeField] private ScaleEventSO _scaleEvent;

    [Header("Broadcasting to...")]
    [SerializeField] private PauseGameplayEventSO _pauseGameplay;

    [Space]
    [SerializeField] private string _scaleGroupTag;
    [SerializeField] private List<Transform> _scaleGroups;

    private float _scaleDuration = 2f;
    private PropScaleCategory _currentClumpScale;
    [SerializeField] private Transform _scaler;

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
        _currentClumpScale = _clumpData.Scale;
    }

    private void CheckForScaleUp(PropScaleCategory newScale)
    {
        if (_currentClumpScale != newScale)
        {
            // Pause and stop movement to start scaling everything
            _pauseGameplay.Raise(true, true, name);

            // Scale all the objects already collected
            PropManager.CurrentCollection.ForEach(c => c.transform.DOScale(
                c.transform.localScale.x / 1f, _scaleDuration));
            _scaler.transform.position = _clumpData.Transform.position;

            // switch the scale group parents to scale relative to the clump
            _scaleGroups.ForEach(group => group.parent = _scaler.transform);

            // scale everything down
            _scaler.DOScale(1f, _scaleDuration).OnComplete(() =>
            {
                // reparent the groups
                _scaleGroups.ForEach(group =>
                {
                    if (group.localScale.x < 1f)
                    {
                        group.gameObject.SetActive(false);
                        }
                    else
                    {
                        group.parent = transform;
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

    private void OnValidate()
    {
        _scaleGroups = new List<Transform>();
        new List<PropScaleObjectGroup>(
            FindObjectsOfType<PropScaleObjectGroup>()
            ).ForEach(g => _scaleGroups.Add(g.transform));
    }
}
