using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Collectible : MonoBehaviour
{
    //public UnityAction<Collectible, bool> OnCollectibleEvent;
    [field: SerializeField]
    public float Size { get; private set; }

    [field: SerializeField]
    public float ClumpIncrease { get; private set; }

    [field: SerializeField]
    public bool IsCollected { get; private set; }

    [field: SerializeField]
    public bool CanBeCollected { get; private set; }

    [Header("Broadcasting to...")]
    [SerializeField] private PropCollisionEventSO _propCollisionEvent;

    private List<CollectibleCollider> _colliders;

    private void Awake()
    {
        _colliders = new List<CollectibleCollider>(
            GetComponentsInChildren<CollectibleCollider>());
    }

    private void OnEnable()
    {
        _colliders.ForEach(c =>
        {
            c.OnTrigger += TriggerEntered;
            c.OnCollision += CollisionEntered;
        });
    }

    private void OnDisable()
    {
        _colliders.ForEach(c =>
        {
            c.OnTrigger -= TriggerEntered;
            c.OnCollision -= CollisionEntered;
        });
    }

    public void SetCollidersIsTrigger(bool isTrigger)
    {
        _colliders.ForEach(c => c.SetIsTrigger(isTrigger));
        CanBeCollected = isTrigger;
    }

    public void DisableColliders()
    {
        _colliders.ForEach(c => c.enabled = false);
        CanBeCollected = false;
    }

    public void SetLayer(string collectedPropLayer)
    {
        gameObject.layer = LayerMask.NameToLayer(collectedPropLayer);
        _colliders.ForEach(c =>
        {
            c.gameObject.layer = LayerMask.NameToLayer(collectedPropLayer);
            c.gameObject.SetActive(false);
        });
    }

    private void CollisionEntered()
    {
        _propCollisionEvent.Raise(this);
    }

    private void TriggerEntered()
    {
        _propCollisionEvent.Raise(this);
    }
}
