using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ClumpPropCollection : MonoBehaviour
{
    [SerializeField] private ClumpDataSO _clumpData;
    [SerializeField] private TransformAnchorSO _clumpPropCollection;
    [field: SerializeField]
    public List<Prop> CollectedProps { get; private set; }
    private Transform _t;

    [Header("Listening to...")]
    [SerializeField] private PropCollectEventSO _collectEvent;

    private void Awake()
    {
        _clumpPropCollection.Set(transform);
        TryGetComponent(out _t);
    }

    private void OnEnable()
    {
        _clumpData.OnScaleChanged += ScaleDown;
        _collectEvent.OnEventRaised += Collect;
    }

    private void OnDisable()
    {
        _clumpData.OnScaleChanged -= ScaleDown;
    }

    private void Collect(Prop collectedProp)
    {
        CollectedProps.Add(collectedProp);
    }

    private void Update() => _t.SetPositionAndRotation(
        _clumpData.Transform.position, _clumpData.Transform.rotation);

    private void ScaleDown(PropScaleCategory DGAF)
    {
        //transform.DOScale(transform.localScale * .5f, 2f);
    }
}
