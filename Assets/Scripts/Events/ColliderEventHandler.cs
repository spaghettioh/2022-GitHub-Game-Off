using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColliderEventHandler : MonoBehaviour
{
    private enum CollisionType {
        CollisionEnter,
        CollisionStay,
        CollisionExit,
        TriggerEnter,
        TriggerStay,
        TriggerExit,
    }

    [SerializeField] private CollisionType _collisionType;
    [SerializeField] private UnityEvent _event;

    private void OnCollisionEnter(Collision collision)
    {
        if (_collisionType == CollisionType.CollisionEnter)
        {
            _event.Invoke();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (_collisionType == CollisionType.CollisionStay)
        {
            _event.Invoke();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (_collisionType == CollisionType.CollisionExit)
        {
            _event.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_collisionType == CollisionType.TriggerEnter)
        {
            _event.Invoke();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_collisionType == CollisionType.TriggerStay)
        {
            _event.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_collisionType == CollisionType.TriggerExit)
        {
            _event.Invoke();
        }
    }
}
