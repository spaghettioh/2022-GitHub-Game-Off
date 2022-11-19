using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PropCollider : MonoBehaviour
{
    public UnityAction<Collision> OnCollision;
    public UnityAction<Collider> OnTrigger;

    private BoxCollider _collider;
    private Vector3 _size;

    private void Awake()
    {
        TryGetComponent(out _collider);
        _size = _collider.size;
        _size.z = 5f;
        _collider.size = _size;
    }

    public void AdjustSizeAndTrigger(bool isTrigger)
    {
        var size = _collider.size;
        if (isTrigger)
        {
            size.z = .01f;
        }
        else
        {
            size.z = 2f;
        }

        _collider.size = size;
        _collider.isTrigger = isTrigger;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (OnCollision != null)
        {
            OnCollision.Invoke(collision);
        }
        else
        {
            Debug.LogWarning("No subscribers to collision event");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (OnTrigger != null)
        {
            OnTrigger.Invoke(other);
        }
        else
        {
            Debug.LogWarning("No subscribers to trigger event");
        }
    }

    private void OnDrawGizmos()
    {
        TryGetComponent(out _collider);
        var color = Color.red;
        color.a = .1f;
        Gizmos.color = color;
        Gizmos.DrawCube(_collider.bounds.center, _collider.bounds.extents * 2f);
    }
}
