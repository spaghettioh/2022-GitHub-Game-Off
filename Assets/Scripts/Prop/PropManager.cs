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
    public static readonly List<Prop> CurrentCollection = new List<Prop>();
    [SerializeField] private TransformAnchorSO _clumpPropCollection;

    [Header("Listening to...")]
    [SerializeField] private PropCollectEventSO _propCollectEvent;
    [SerializeField] private VoidEventSO _crashEvent;

    private void OnEnable()
    {
        _clumpData.OnSizeChanged += AdjustPropsCollectable;
        _propCollectEvent.OnEventRaised += CollectProp;
        _crashEvent.OnEventRaised += CrashIntoProp;
    }

    private void OnDisable()
    {
        _clumpData.OnSizeChanged -= AdjustPropsCollectable;
        _propCollectEvent.OnEventRaised -= CollectProp;
        _crashEvent.OnEventRaised -= CrashIntoProp;
    }

    private void Start()
    {
        _props = new List<Prop>(GetComponentsInChildren<Prop>());
        AdjustPropsCollectable();
    }

    private void CollectProp(Prop collectedProp)
    {
        _collectableProps.Remove(collectedProp);
        CurrentCollection.Add(collectedProp);
        collectedProp.transform.SetParent(_clumpPropCollection.Transform);
        _clumpData.IncreaseSize(collectedProp.ClumpSizeChangeAmount);
    }

    private void CrashIntoProp()
    {
        if (CurrentCollection.Count > 0)
        {
            var attaching = CurrentCollection.FindAll(p => p.IsAttaching);
            if (attaching.Count > 0)
            {
                attaching.ForEach(p =>
                {
                    UncollectProp(p);
                });
            }
            else
            {
                var lastPropCollected
                    = CurrentCollection[CurrentCollection.Count - 1];
                UncollectProp(lastPropCollected);
            }
        }
    }

    private void UncollectProp(Prop p)
    {
        CurrentCollection.Remove(p);
        _collectableProps.Add(p);
        _clumpData.DecreaseSize(p.ClumpSizeChangeAmount);
        p.Uncollect();
    }

    private void AdjustPropsCollectable(float clumpSize = default)
    {
        if (clumpSize == default) clumpSize = _clumpData.Size;

        _props.FindAll(p => p.Size <= clumpSize).ForEach(p =>
        {
            _props.Remove(p);
            _collectableProps.Add(p);
            p.ToggleCollectable(true);
        });
    }
}
