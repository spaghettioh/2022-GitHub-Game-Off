using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private float _mass;
    public float Mass { get { return _mass; } }

    [SerializeField] private string _clumpTag;
    [SerializeField] private ClumpMassSO _clumpMass;
    [SerializeField] private TransformAnchorSO _clumpTransform;

    private LayerMask _defaultLayer;
    [SerializeField] private string _collectedLayer;

    [SerializeField] private CollectEventSO _collectEvent;
    public bool IsCollected { get; private set; }

    private void Start()
    {
        _defaultLayer = gameObject.layer;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(_clumpTag) && _clumpMass.Mass > _mass)
        {
            SetCollected(true);
        }
    }

    public void SetCollected(bool collect)
    {
        if (collect)
        {
            _collectEvent.Raise(this);
            transform.SetParent(_clumpTransform.Transform);
            IsCollected = true;
            gameObject.layer = LayerMask.NameToLayer(_collectedLayer);
        }
        else
        {
            IsCollected = false;
            gameObject.layer = LayerMask.NameToLayer(_defaultLayer.ToString());
        }
    }
}
