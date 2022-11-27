using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class WinScreenManager : MonoBehaviour
{
    [SerializeField] private ClumpPropCollectionSO _propCollection;
    [SerializeField] private WinScreenPropPoolSO _propPool;
    [SerializeField] private Transform _propParent;
    [SerializeField] private Transform _bottleLeft;
    [SerializeField] private Transform _bottleRight;
    [SerializeField] private List<WinScreenProp> _spawnedProps;
    [SerializeField] private float _dropTime;

    [Header("Starts when...")]
    [SerializeField] private VoidEventSO _curtainsOpened;

    [Header("Broadcasting to...")]
    [SerializeField] private VoidEventSO _allPropsDropped;
    [SerializeField] private AudioEventSO _audioEvent;

    [Header("DEBUG / TESTING")]
    [SerializeField] private List<PropData> _propsCollected;
    [SerializeField] private int _poolSize;
    [SerializeField] private bool _test;

    private void OnEnable()
    {
        _curtainsOpened.OnEventRaised += StartWinScreen;
    }

    private void OnDisable()
    {
        _curtainsOpened.OnEventRaised -= StartWinScreen;
    }

    public void RushWinScreen()
    {
        StopAllCoroutines();
        PropsFinishedDropping();
    }

    private void StartWinScreen()
    {
        // Use test data when no props are present - useful for testing
        if (_test)
        {
            _propCollection.Reset();
            GetComponentsInChildren<Prop>(true).ToList()
                .ForEach(prop => _propsCollected.Add(
                new PropData(
                    prop.Sprite,
                    prop.transform.localScale.x,
                    prop.PropCollectSound)));
        }
        else  _propsCollected = _propCollection.GetPropsWon();

        _poolSize = _propsCollected.Count();
        _spawnedProps = _propPool.PreWarm(_poolSize, _propParent);
        PrepProps();
        StartCoroutine(DropProps());
    }

    private void PrepProps()
    {
        for (int i = 0; i < _spawnedProps.Count; i++)
        {
            var spawnedProp = _spawnedProps[i];
            var matchingPropData = _propsCollected[i];
            spawnedProp.Initialize(matchingPropData, GetRandomPosition());
        }
    }

    private Vector3 GetRandomPosition()
    {
        var lPos = _bottleLeft.position;
        var rPos = _bottleRight.position;
        var randomXPos = Random.Range(lPos.x, rPos.x);
        var randomYPos = Random.Range(lPos.y, lPos.y + 2);
        float randomZPos = lPos.z += Random.Range(-1, 1);
        return new Vector3(randomXPos, randomYPos, randomZPos);
    }

    private IEnumerator DropProps()
    {
        var waitTime = _dropTime / _spawnedProps.Count;
        while (_spawnedProps.Count > 0)
        {
            int randomPropIndex = Random.Range(0, _spawnedProps.Count);
            WinScreenProp prop = _spawnedProps[randomPropIndex];
            _spawnedProps.Remove(prop);

            prop.gameObject.SetActive(true);
            prop.Drop();
            if (prop.PropCollectSound != null)
                _audioEvent.RaisePlayback(prop.PropCollectSound, name);

            yield return new WaitForSeconds(waitTime);
        }

        PropsFinishedDropping();
    }

    private void PropsFinishedDropping() =>
        StartCoroutine(PropsFinishedDroppingRoutine());
    private IEnumerator PropsFinishedDroppingRoutine()
    {
        yield return new WaitForSeconds(_dropTime);
        _allPropsDropped.Raise(name);
    }
}
