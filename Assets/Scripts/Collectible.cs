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

    [SerializeField] private string _defaultLayer;
    [SerializeField] private string _collectedLayer;

    [SerializeField] private CollectEventSO _collectEvent;
    public bool IsCollected { get; private set; }

    [SerializeField] private VoidEventSO _knockEvent;

    private Collider _collider;

    private void Start()
    {
        TryGetComponent(out _collider);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(_clumpTag))
        {
            if (_mass < _clumpMass.Mass)
            {
                SetCollected(true);
            }
            else if (_clumpMass.Mass == _mass)
            {
            }
            else
            {
                _knockEvent.Raise();
            }
        }
    }

    public void SetCollected(bool collect)
    {
        if (collect)
        {
            _collectEvent.Raise(this);
            transform.SetParent(_clumpTransform.Transform);
            IsCollected = true;
            //gameObject.layer = LayerMask.NameToLayer(_collectedLayer);
            _collider.enabled = false;
        }
        else
        {
            IsCollected = false;
            transform.SetParent(null);
            StartCoroutine(ResetLayer());
            //Rigidbody body = componen
        }
    }

    private IEnumerator ResetLayer()
    {
        yield return new WaitForSeconds(2f);
        gameObject.layer = LayerMask.NameToLayer(_defaultLayer);
    }
}
