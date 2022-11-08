using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    //[Header("Size to roll up")]
    [field: SerializeField]
    public float Mass { get; private set; }

    [SerializeField] private string _clumpTag;
    [SerializeField] private ClumpMassSO _clumpMass;
    [SerializeField] private TransformAnchorSO _clumpTransform;

    [SerializeField] private string _defaultLayer;
    [SerializeField] private string _collectedLayer;

    [Header("Broadcasting to...")]
    [SerializeField] private CollectEventSO _collectEvent;
    public bool IsCollected { get; private set; }
    [SerializeField] private VoidEventSO _knockEvent;

    private Collider _collider;

    private void Start()
    {
        TryGetComponent(out _collider);
    }

    private void OnEnable()
    {
        _clumpMass.OnMassChanged += CheckMassThreshold;
    }

    private void OnDisable()
    {
        _clumpMass.OnMassChanged -= CheckMassThreshold;
    }

    private void CheckMassThreshold(float clumpMass)
    {
        if (clumpMass >= Mass)
        {
            SetColliderToTrigger();
        }
        // TODO Add something to make the thing shake if it's within some threshold
    }

    private void SetColliderToTrigger()
    {
        _collider.isTrigger = true;
        _clumpMass.OnMassChanged -= CheckMassThreshold;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(_clumpTag))
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
