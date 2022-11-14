using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PropScaleManager : MonoBehaviour
{
    [SerializeField] private ClumpDataSO _clumpData;
    [SerializeField] private ScaleEventSO _scaleEvent;
    [SerializeField] private string _scaleGroupTag;
    [SerializeField] private List<Transform> _scaleGroups;

    private float _scaleDuration = 2f;
    private PropScaleCategory _currentClumpScale;

    private void OnEnable()
    {
        //_scaleEvent.OnScaleChanged += ResizeCategories;
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
            // Scale all the objects already collected
            new List<Collectible>(
                _clumpData.Transform.GetComponentsInChildren<Collectible>())
                .ForEach(c => c.transform.DOScale(
                    c.transform.localScale.x / 2f, _scaleDuration));

            // unparent the groups
            _scaleGroups.ForEach(c => c.transform.parent = null);

            // move this empty thing to the clump position
            transform.position = _clumpData.Transform.position;

            // reparent the groups
            _scaleGroups.ForEach(c => c.transform.parent = transform);

            // scale everything down
            transform.DOScale(.5f, _scaleDuration).OnComplete(() =>
            {
                // unparent the groups
                _scaleGroups.ForEach(c => c.transform.parent = null);

                // scale the parent back up
                transform.localScale = Vector3.one;

                // reparent the groups
                _scaleGroups.ForEach(c => c.transform.parent = transform);
            });
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
