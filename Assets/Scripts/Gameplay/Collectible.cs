using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private float SizeInMeters;

    [field: SerializeField]
    public float CollectedSize
    {
        get;
        private set;
    }

    [SerializeField] private ClumpDataSO _clumpData;

    [SerializeField] private string _defaultLayer;
    [SerializeField] private string _collectedLayer;

    [SerializeField] private CollectEventSO _collectEvent;
    [SerializeField] private VoidEventSO _collectVoidEvent;
    public bool IsCollected { get; private set; }

    [SerializeField] private VoidEventSO _knockEvent;

    private Collider _collider;

    private void Start()
    {
        //_collider = GetComponent<Collider>();
        TryGetComponent(out _collider);
    }

    private void OnEnable()
    {
        _clumpData.OnSizeChanged += CompareSize;
    }

    private void OnDisable()
    {
        _clumpData.OnSizeChanged -= CompareSize;
    }

    private void OnCollisionEnter(Collision collision)
    {
        _knockEvent.Raise();
    }

    private void CompareSize(float clumpSize)
    {
        StartCoroutine(Co_CompareSize(clumpSize));
    }

    private IEnumerator Co_CompareSize(float clumpSize)
    {
        yield return new WaitForSeconds(.5f);
        if (clumpSize >= SizeInMeters)
        {
            _collider.isTrigger = true;
            _clumpData.OnSizeChanged -= CompareSize;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _clumpData.Transform.gameObject)
        {
            SetCollected(true);
        }
    }

    public void SetCollected(bool collect)
    {
        if (collect)
        {
            _collectEvent.Raise(this);
            _collectVoidEvent.Raise();
            transform.SetParent(_clumpData.Transform);
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
