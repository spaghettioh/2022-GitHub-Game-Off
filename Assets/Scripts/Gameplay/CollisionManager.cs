using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    [Header("Collection config")]
    [SerializeField] private ClumpDataSO _clumpDataSO;
    [SerializeField] private string _defaultPropLayer;
    [SerializeField] private string _collectedPropLayer;
    [SerializeField] private string _propTag;
    [SerializeField] private List<Collectible> _collectibles;

    [Header("Audio")]
    [SerializeField] private AudioCueSO _collectSound;
    [SerializeField] private AudioCueSO _crashSound;

    [Header("Listening to...")]
    [SerializeField] private PropCollisionEventSO _propCollisionEvent;

    [Header("Broadcasting to...")]
    [SerializeField] private AudioEventSO _sfxChannel;

    private void OnEnable()
    {
        _clumpDataSO.OnSizeChanged += AdjustColliders;
        _propCollisionEvent.OnCollisionRaised += ProcessPropCollision;
    }

    private void OnDisable()
    {
        _clumpDataSO.OnSizeChanged -= AdjustColliders;
        _propCollisionEvent.OnCollisionRaised -= ProcessPropCollision;
    }

    private void Start()
    {
        var props = new List<GameObject>(
            GameObject.FindGameObjectsWithTag(_propTag));
        props.ForEach(p => _collectibles.Add(p.GetComponent<Collectible>()));
    }

    private void ProcessPropCollision(Collectible collectible)
    {
        if (collectible.CanBeCollected)
        {
            AddCollectibleToClump(collectible);
        }
        else
        {
            ShowCollectibleReaction(collectible);
        }
    }

    private void ShowCollectibleReaction(Collectible collectible)
    {
        _sfxChannel.RaisePlayback(_crashSound);
        // TODO shake object if clump size is close
    }

    private void AdjustColliders(float clumpSize = default)
    {
        if (clumpSize == default) clumpSize = _clumpDataSO.Size;

        _collectibles.FindAll(c => c.Size <= clumpSize)
            .ForEach(c => c.SetCollidersIsTrigger(true));
    }

    private void AddCollectibleToClump(Collectible collectible)
    {
        _sfxChannel.RaisePlayback(_collectSound);
        collectible.transform.SetParent(_clumpDataSO.Transform);
        collectible.SetLayer(_collectedPropLayer);
        collectible.DisableColliders();
        _clumpDataSO.IncreaseSize(collectible.ClumpIncrease);
    }
}
