using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollectibleCollider : MonoBehaviour
{
    public UnityAction OnCollision;
    public UnityAction OnTrigger;

    private Collider _collider;

    private void Awake()
    {
        TryGetComponent(out _collider);
    }

    public void SetIsTrigger(bool isTrigger)
    {
        _collider.isTrigger = isTrigger;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (OnCollision != null)
        {
            OnCollision.Invoke();
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
            OnTrigger.Invoke();
        }
        else
        {
            Debug.LogWarning("No subscribers to trigger event");
        }
    }
}