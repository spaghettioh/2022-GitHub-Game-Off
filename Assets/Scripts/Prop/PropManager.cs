using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PropManager : MonoBehaviour
{
    [Header("Collection config")]
    [SerializeField] private ClumpDataSO _clumpData;
    [SerializeField] private List<Prop> _props;
    [SerializeField] private List<Prop> _collectableProps;
    [SerializeField] private List<Prop> _currentCollection;
    public static List<Prop> CurrentCollection;
    [SerializeField] private TransformAnchorSO _clumpPropCollection;

    [Header("Listening to...")]
    [SerializeField] private PropCollectEventSO _propCollectEvent;
    [SerializeField] private VoidEventSO _crashEvent;

    private void OnEnable()
    {
        _clumpData.OnSizeChanged += AdjustPropsCollectable;
        _propCollectEvent.OnEventRaised += ProcessPropCollect;
        _crashEvent.OnEventRaised += ProcessPropCrash;
    }

    private void OnDisable()
    {
        _clumpData.OnSizeChanged -= AdjustPropsCollectable;
        _propCollectEvent.OnEventRaised -= ProcessPropCollect;
        _crashEvent.OnEventRaised -= ProcessPropCrash;
    }

    private void Start()
    {
        _props = new List<Prop>(GetComponentsInChildren<Prop>());

        _currentCollection = new List<Prop>();
        CurrentCollection = new List<Prop>();
        AdjustPropsCollectable();
    }

    private void ProcessPropCollect(Prop collectedProp)
    {
        _collectableProps.Remove(collectedProp);
        _currentCollection.Add(collectedProp);
        CurrentCollection.Add(collectedProp);
        collectedProp.transform.SetParent(_clumpPropCollection.Transform);
        _clumpData.IncreaseSize(collectedProp.ClumpSizeChangeAmount);
    }

    private void ProcessPropCrash()
    {
        if (_currentCollection.Count > 0)
        {
            var lastPropCollected
                = _currentCollection[_currentCollection.Count - 1];

            _currentCollection.Remove(lastPropCollected);
            CurrentCollection.Remove(lastPropCollected);
            _collectableProps.Add(lastPropCollected);
            _clumpData.DecreaseSize(lastPropCollected.ClumpSizeChangeAmount);
            lastPropCollected.Uncollect();
        }
    }

    private void AdjustPropsCollectable(float clumpSize = default)
    {
        if (clumpSize == default) clumpSize = _clumpData.Size;

        _props.FindAll(p => p.Size <= clumpSize)
            .ForEach(p =>
            {
                _props.Remove(p);
                _collectableProps.Add(p);
                p.ToggleCollectable(true);
            });
    }
}
