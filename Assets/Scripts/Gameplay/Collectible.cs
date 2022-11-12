using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Collectible : MonoBehaviour
{
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

    public void MoveTowardAttachPoint(SphereCollider collider)
    {
        StartCoroutine(MoveTowardsPositionRoutine(collider));
    }

    private IEnumerator MoveTowardsPositionRoutine(SphereCollider collider)
    {
        var attachPoint = Instantiate(
            new GameObject(),
            collider.ClosestPoint(transform.position),
            Quaternion.identity,
            collider.transform).transform;
        attachPoint.name = $"TempAttachPointFor{name}";

        var duration = 0f;


        var startPosition = transform.localPosition;
        var endPosition = attachPoint.localPosition;

        while (transform.localPosition != endPosition)
        {
            // Take some seconds to move toward the collision point
            transform.localPosition =
                Vector3.Lerp(startPosition, endPosition, duration / 10f);
            duration += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        Destroy(attachPoint.gameObject);
    }
}
