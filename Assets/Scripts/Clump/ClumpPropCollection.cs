using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClumpPropCollection : MonoBehaviour
{
    [SerializeField] private ClumpDataSO _clumpData;
    [SerializeField] private TransformAnchorSO _clumpPropCollection;
    private Transform _t;

    private void Awake()
    {
        _clumpPropCollection.Set(transform);
        TryGetComponent(out _t);
    }

    private void Update()
    {
        _t.SetPositionAndRotation(
            _clumpData.Transform.position, _clumpData.Transform.rotation);
    }
}
