using UnityEngine;
using UnityEngine.Events;

public class PropColliderMesh : MonoBehaviour
{
    public UnityAction<Collision> OnCollision;
    public UnityAction<Collider> OnTrigger;

    private Transform _t;
    private MeshCollider _collider;
    private Vector3 _scaler = Vector3.one;

    private void Awake()
    {
        TryGetComponent(out _collider);
        TryGetComponent(out _t);
        _scaler.y *= 50;
        _t.localScale = _scaler;
    }

    public void AdjustSizeAndTrigger(bool isTrigger)
    {
        _scaler = _t.localScale;
        if (isTrigger)
        {
            _scaler.y /= 10f;
        }
        else
        {
            _scaler.y *= 10f;
        }
        _t.localScale = _scaler;
        _collider.isTrigger = isTrigger;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (OnCollision != null) OnCollision.Invoke(collision);
        else Debug.LogWarning("No subscribers to collision event");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (OnTrigger != null) OnTrigger.Invoke(other);
        else Debug.LogWarning("No subscribers to trigger event");
    }
}
