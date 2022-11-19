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
    [SerializeField] private List<Collectible> _props;
    [SerializeField] private List<Collectible> _currentCollection;

    [Header("Audio")]
    [SerializeField] private AudioCueSO _collectSound;
    [SerializeField] private AudioCueSO _crashSoundSmall;
    [SerializeField] private AudioCueSO _crashSoundLarge;

    [Header("Listening to...")]
    [SerializeField] private PropCollisionEventSO _propCollisionEvent;

    [Header("Broadcasting to...")]
    [SerializeField] private AudioEventSO _sfxChannel;
    [SerializeField] private VoidEventSO _crashEvent;

    private void OnEnable()
    {
        _clumpDataSO.OnSizeChanged += AdjustPropsCollectable;
        _propCollisionEvent.OnCollisionRaised += ProcessPropCollision;
    }

    private void OnDisable()
    {
        _clumpDataSO.OnSizeChanged -= AdjustPropsCollectable;
        _propCollisionEvent.OnCollisionRaised -= ProcessPropCollision;
    }

    private void Start()
    {
        // Build the list of all collectibles
        new List<GameObject>(
            GameObject.FindGameObjectsWithTag(_propTag))
            .ForEach(p => _props.Add(p.GetComponent<Collectible>()));
        AdjustPropsCollectable();
    }

    private void ProcessPropCollision(Collectible collectible)
    {
        if (collectible.IsCollectable)
        {
            AddPropToClump(collectible);
        }
        else
        {
            ShowPropReaction(collectible);
        }
    }

    private void ShowPropReaction(Collectible collidingProp)
    {
        if (_clumpDataSO.Velocity >= (_clumpDataSO.MaxSpeed / 2))
        {
            _sfxChannel.RaisePlayback(_crashSoundLarge);
            _crashEvent.Raise(name);

            if (_currentCollection.Count > 0)
            {
                var lastPropCollected
                    = _currentCollection[_currentCollection.Count - 1];

                _currentCollection.Remove(lastPropCollected);
                _clumpDataSO.DecreaseSize(lastPropCollected.ClumpIncrease);
                lastPropCollected.UnCollect(_defaultPropLayer);
            }
            collidingProp.Shake();
        }
        else
        {
            _sfxChannel.RaisePlayback(_crashSoundSmall);
        }
    }

    private void AdjustPropsCollectable(float clumpSize = default)
    {
        if (clumpSize == default) clumpSize = _clumpDataSO.Size;

        _props.FindAll(p => p.Size <= clumpSize)
            .ForEach(p => p.ToggleCollectable(true));
    }

    private void AddPropToClump(Collectible collectible)
    {
        _sfxChannel.RaisePlayback(_collectSound);
        _currentCollection.Add(collectible);
        _clumpDataSO.IncreaseSize(collectible.ClumpIncrease);
        collectible.Collect(_clumpDataSO.Transform, _clumpDataSO.Collider, _collectedPropLayer);
    }
}
