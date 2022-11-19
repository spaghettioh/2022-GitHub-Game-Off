using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class Collectible : MonoBehaviour
{
    [field: SerializeField]
    public float Size { get; private set; }

    [field: SerializeField]
    public float ClumpIncrease { get; private set; }

    [field: SerializeField]
    public bool IsCollectable { get; private set; }

    [Header("Broadcasting to...")]
    [SerializeField] private PropCollisionEventSO _propCollisionEvent;
    //[SerializeField] private CollectEventSO _collectEvent;

    private Transform OriginalParent;

    private List<CollectibleCollider> _colliders;
    private GameObject _attachPoint;
    private float _attachDuration = 10f;
    private float _flickerDuration = 2f;
    private float _shakeDuration = 1f;

    private void Awake()
    {
        _colliders = new List<CollectibleCollider>(
            GetComponentsInChildren<CollectibleCollider>());
        OriginalParent = transform.parent;
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

    public void SetLayer(string collectedPropLayer) =>
        gameObject.layer = LayerMask.NameToLayer(collectedPropLayer);

    public void ToggleCollectable(bool onOff)
    {
        _colliders.ForEach(c => c.SetIsTrigger(onOff));
        IsCollectable = onOff;
    }

    private void CollisionEntered() => _propCollisionEvent.Raise(this);
    private void TriggerEntered() => _propCollisionEvent.Raise(this);

    public void Collect(
        Transform newParent, SphereCollider collider, string collectedLayer)
    {
        transform.SetParent(newParent);
        SetLayer(collectedLayer);
        ToggleCollectable(false);

        // Disable colliders
        _colliders.ForEach(c => c.gameObject.SetActive(false));

        // Create an object at the closest point to the collider
        _attachPoint = new GameObject();
        _attachPoint.transform.position =
            collider.ClosestPoint(transform.position);
        _attachPoint.transform.SetParent(collider.transform);

        // Then move towards it
        var endPosition = _attachPoint.transform.localPosition;
        transform.DOLocalMove(endPosition, _attachDuration);

        // Clean up
        Destroy(_attachPoint);
    }

    public void UnCollect(string defaultLayer)
    {
        StopAllCoroutines();
        StartCoroutine(UnCollectRoutine(defaultLayer));
    }

    private IEnumerator UnCollectRoutine(string defaultLayer)
    {
        // Destroy the attach point in case that coroutine hasn't finished
        Destroy(_attachPoint);
        transform.SetParent(OriginalParent);

        // Moves to a seemingly random position
        var p = transform.position;
        var randomX = p.x + UnityEngine.Random.Range(-1f, 1f);
        var randomZ = p.z + UnityEngine.Random.Range(-1f, 1f);
        var endPos = new Vector3(randomX, 0, randomZ);
        transform.DOJump(endPos, 1, 2, 1);

        // Flickers the sprite for some time
        var flickerTime = 0f;
        var sprite = GetComponent<SpriteRenderer>();
        var alphaChange = Color.white;
        while (flickerTime < _flickerDuration)
        {
            alphaChange.a = alphaChange.a == 1 ? 0 : 1;
            sprite.color = alphaChange;
            var waitTime = Time.deltaTime + .1f;
            yield return new WaitForSeconds(waitTime);
            flickerTime += waitTime;
        }
        sprite.color = Color.white;

        // Resets the prop
        SetLayer(defaultLayer);
        ToggleCollectable(true);
        _colliders.ForEach(c => c.gameObject.SetActive(true));
    }

    public void Shake()
    {
        transform.DOShakePosition(_shakeDuration, .1f);
    }
}
