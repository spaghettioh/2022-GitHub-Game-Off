using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private float SizeInMeters;

    [field: SerializeField]
    public float CollectedSize { get; private set; }

    [SerializeField] private ClumpDataSO _clumpData;

    [SerializeField] private string _defaultLayer;
    [SerializeField] private string _collectedLayer;

    [SerializeField] private CollectEventSO _collectEvent;
    [SerializeField] private VoidEventSO _collectVoidEvent;
    public bool IsCollected { get; private set; }

    [SerializeField] private VoidEventSO _knockEvent;

    private List<CollectibleCollider> _colliders;
    
    private void Awake()
    {
        _colliders = new List<CollectibleCollider>(
            GetComponentsInChildren<CollectibleCollider>());
    }

    private void OnEnable()
    {
        _clumpData.OnSizeChanged +=
            (float clumpSize) => StartCoroutine(CompareSizeRoutine(clumpSize));

        _colliders.ForEach((CollectibleCollider c) =>
        {
            c.OnTrigger += TriggerEntered;
        });
    }

    private void OnDisable()
    {
        _clumpData.OnSizeChanged -=
            (float clumpSize) => StartCoroutine(CompareSizeRoutine(clumpSize));

        _colliders.ForEach((CollectibleCollider c) =>
        {
            c.OnTrigger -= TriggerEntered;
        });
    }

    private void OnCollisionEnter(Collision collision)
    {
        _knockEvent.Raise();
    }

    /// <summary>
    /// Necessary because of the setup time from scene loading for some reason
    /// </summary>
    /// <param name="clumpSize"></param>
    /// <returns></returns>
    private IEnumerator CompareSizeRoutine(float clumpSize)
    {
        yield return new WaitForSeconds(.1f);
        if (clumpSize >= SizeInMeters)
        {
            _colliders.ForEach((CollectibleCollider c) =>
            {
                c.SetIsTrigger(true);
            });
            _clumpData.OnSizeChanged -=
                (float clumpSize) => StartCoroutine(CompareSizeRoutine(clumpSize));
        }
    }

    private void TriggerEntered(Collider other)
    {
        print("foo");
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
            gameObject.layer = LayerMask.NameToLayer(_collectedLayer);
            _colliders.ForEach((CollectibleCollider c) =>
            {
                c.gameObject.SetActive(false);
            });
        }
        else
        {
            IsCollected = false;
            transform.SetParent(null);
            StartCoroutine(ResetLayer());
        }
    }

    private IEnumerator ResetLayer()
    {
        yield return new WaitForSeconds(2f);
        gameObject.layer = LayerMask.NameToLayer(_defaultLayer);
    }
}
